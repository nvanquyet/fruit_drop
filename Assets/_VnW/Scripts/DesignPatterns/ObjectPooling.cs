// ObjectPool.cs
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DesignPatterns
{
    /// <summary>
    /// Interface for poolable objects
    /// </summary>
    public interface IPoolable
    {
        void OnSpawn();
        void OnDespawn();
    }

    /// <summary>
    /// Generic object pool for any GameObject that implements IPoolable
    /// </summary>
    public class ObjectPooling<T> where T : Component, IPoolable
    {
        private readonly T _prefab;
        private readonly Transform _parent;
        private readonly Queue<T> _pool = new Queue<T>();
        private readonly List<T> _activeObjects = new List<T>();
        private readonly int _initialSize;
        private readonly bool _canGrow;

        /// <summary>
        /// Constructor for ObjectPool
        /// </summary>
        /// <param name="prefab">Prefab to pool</param>
        /// <param name="initialSize">Initial size of the pool</param>
        /// <param name="parent">Parent transform for pooled objects</param>
        /// <param name="canGrow">Whether the pool can grow beyond initialSize</param>
        public ObjectPooling(T prefab, int initialSize, Transform parent = null, bool canGrow = true)
        {
            _prefab = prefab;
            _parent = parent;
            _initialSize = initialSize;
            _canGrow = canGrow;

            Initialize();
        }

        /// <summary>
        /// Initialize the pool with objects
        /// </summary>
        private void Initialize()
        {
            for (var i = 0; i < _initialSize; i++)
            {
                T instance = CreateInstance();
                ReturnToPool(instance);
            }
        }

        /// <summary>
        /// Create a new instance of the pooled object
        /// </summary>
        private T CreateInstance()
        {
            var instance = UnityEngine.Object.Instantiate(_prefab, _parent);
            instance.gameObject.SetActive(false);
            return instance;
        }

        /// <summary>
        /// Get an object from the pool
        /// </summary>
        private T Get()
        {
            T instance;

            if (_pool.Count > 0)
            {
                instance = _pool.Dequeue();
            }
            else if (_canGrow)
            {
                instance = CreateInstance();
            }
            else
            {
                Debug.LogWarning($"Object pool for {typeof(T).Name} is empty and cannot grow.");
                return null;
            }

            instance.gameObject.SetActive(true);
            _activeObjects.Add(instance);
            instance.OnSpawn();
            
            return instance;
        }

        /// <summary>
        /// Get an object from the pool at a specific position and rotation
        /// </summary>
        public T Get(Vector3 position, Quaternion rotation)
        {
            var instance = Get();
            if (instance == null) return instance;
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            return instance;
        }
        
        /// <summary>
        /// Get an object from the pool at a specific position and rotation
        /// </summary>
        public T Get(Vector3 position)
        {
            var instance = Get();
            if (instance == null) return instance;
            instance.transform.position = position;
            return instance;
        }

        /// <summary>
        /// Return an object to the pool
        /// </summary>
        public void ReturnToPool(T instance)
        {
            if (instance == null)
                return;

            instance.OnDespawn();
            instance.gameObject.SetActive(false);
            
            _activeObjects.Remove(instance);
            _pool.Enqueue(instance);
        }

        /// <summary>
        /// Return all active objects to the pool
        /// </summary>
        public void ReturnAllToPool()
        {
            // Create a new list to avoid collection modification issues
            var activeObjectsCopy = new List<T>(_activeObjects);
            
            foreach (var instance in activeObjectsCopy)
            {
                ReturnToPool(instance);
            }
        }

        /// <summary>
        /// Get count of available objects in the pool
        /// </summary>
        public int AvailableCount => _pool.Count;

        /// <summary>
        /// Get count of active objects from this pool
        /// </summary>
        public int ActiveCount => _activeObjects.Count;
    }
}