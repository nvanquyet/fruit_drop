using UnityEngine;

namespace DesignPatterns
{
    /// <summary>
    /// Generic Singleton pattern implementation for MonoBehaviour classes
    /// </summary>
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        
        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = FindObjectOfType<T>();
                    
                if (_instance != null) return _instance;
                var singletonObject = new GameObject($"{typeof(T).Name} (Singleton)");
                _instance = singletonObject.AddComponent<T>();

                return _instance;
            }
        }
        
        protected virtual void Awake()
        {
            InitializeSingleton();
        }
        
        protected virtual void InitializeSingleton()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            _instance = this as T;
        }
    }
    
    
    
    public class SingletonDontDestroy<T> : Singleton<T> where T : MonoBehaviour
    {
        protected override void InitializeSingleton()
        {
            base.InitializeSingleton();
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
    }


    public abstract class LazySingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static readonly object _lock = new object();
        private static bool _applicationIsQuitting = false;
        
        public static T Instance
        {
            get
            {
                if (_applicationIsQuitting)
                {
                    Debug.LogWarning($"[{typeof(T)}] Instance will not be returned because the application is quitting.");
                    return null;
                }
                
                lock (_lock)
                {
                    if (_instance != null) return _instance;
                    _instance = FindObjectOfType<T>();

                    if (_instance != null) return _instance;
                    var singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<T>();
                    singletonObject.name = $"{typeof(T)} (Lazy Singleton)";
                            
                    DontDestroyOnLoad(singletonObject);
                            
                    Debug.Log($"[{typeof(T)}] An instance was created with a lazy singleton.");

                    return _instance;
                }
            }
        }
        
        protected virtual void OnApplicationQuit()
        {
            _applicationIsQuitting = true;
        }
    }
    
    public abstract class NonMonoBehaviourSingleton<T> where T : class, new()
    {
        private static T _instance;
        private static readonly object _lock = new object();
        
        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    return _instance ??= new T();
                }
            }
        }
    }

    public abstract class ScriptableObjectSingleton<T> : ScriptableObject where T : ScriptableObject
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;

                _instance = Resources.Load<T>(typeof(T).Name);

                if (_instance == null)
                {
                    Debug.LogError($"Instance of {typeof(T).Name} not found in Resources. Please create one and place it in a Resources folder.");
                }

                return _instance;
            }
        }
    }
}