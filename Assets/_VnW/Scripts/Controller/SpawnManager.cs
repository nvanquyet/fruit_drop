using System.Collections.Generic;
using _VnW.Scripts.Extension;
using GameEvents;
using Data;
using DesignPatterns;
using DG.Tweening;
using Object;
using UnityEngine;

namespace Controller
{
    public class SpawnManager
    {
        private readonly ObjectPooling<Fruit> _pool;
        private readonly FruitData _fruitData;

        private readonly List<Fruit> _allFruits = new List<Fruit>();
        private readonly Vector3 _spawnPosition;

        public SpawnManager(Transform holder, Vector3 spawnPosition)
        {
            _fruitData = GameData.Instance.FruitData;
            _spawnPosition = spawnPosition;

            ObserverManager.Instance.Subscribe<GameEvent.OnFruitMerged>(OnFruitMerged);
            ObserverManager.Instance.Subscribe<GameEvent.OnFruitDrop>(OnFruitDropped);
            //Pooling 
            _pool = new ObjectPooling<Fruit>(_fruitData.FruitPrefab, 20, holder);
        }

        private void OnFruitMerged(GameEvent.OnFruitMerged param)
        {
            var targetPosition = param.TargetFruit.transform.position;
            param.SourceFruit.transform.DOMove(param.TargetFruit.transform.position, 0.25f).OnComplete(() =>
            {
                //Despawn 2 fruits
                _allFruits.Remove(param.SourceFruit);
                _allFruits.Remove(param.TargetFruit);

                param.SourceFruit.OnDespawn(_pool);
                param.TargetFruit.OnDespawn(_pool);

                //Create and return next level fruit
                var nextLevelId = param.SourceFruit.Data.id + 1;
                var newFruit = OnFruitSpawned(nextLevelId, targetPosition);

                if (!newFruit) return;

                _allFruits.Add(newFruit);
                newFruit.ApplyPhysics(true);
                newFruit.DisableAndReactivateInteraction();

                // Notify about score
                ObserverManager.Instance.Notify(new GameEvent.OnScoreUpdate
                {
                    ScoreToAdd = newFruit.Data.scoreClaim
                });
            });
        }


        private void OnFruitDropped(GameEvent.OnFruitDrop param) =>
            TimerUtility.Instance.CallWithDelay(OnSpawnFruit, 2f);
        
        public void OnSpawnFruit()
        {
            // Get random initial fruit (usually smaller ones)
            var randomFruitId = GetRandomInitialFruitId();

            // Spawn at the top spawn position
            var newFruit = OnFruitSpawned(randomFruitId, _spawnPosition);

            if (newFruit == null) return;
            _allFruits.Add(newFruit);

            newFruit.ApplyPhysics(false);

            // Notify that a fruit was spawned
            ObserverManager.Instance.Notify(new GameEvent.OnFruitSpawned
            {
                Fruit = newFruit
            });
        }

        private static int GetRandomInitialFruitId()
        {
            return Random.Range(0, 3);
        }

        private Fruit OnFruitSpawned(int fruitId, Vector3 position)
        {
            if (!_fruitData.GetValue(fruitId, out var fruitData))
            {
                Debug.LogWarning($"No fruit data found for ID: {fruitId}");
                return null;
            }

            var newFruit = _pool.Get(position);
            newFruit.Initialize(fruitData);
            return newFruit;
        }
      

        private void EnableSpawning() { }

        // Clean up event subscriptions
        public void Dispose()
        {
            ObserverManager.Instance.Unsubscribe<GameEvent.OnFruitMerged>(OnFruitMerged);
            ObserverManager.Instance.Unsubscribe<GameEvent.OnFruitDrop>(OnFruitDropped);
        }
    }
}