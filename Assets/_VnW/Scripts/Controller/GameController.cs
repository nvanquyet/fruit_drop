using DesignPatterns;
using GameEvents;
using UnityEngine;

namespace Controller
{
    public class GameController : Singleton<GameController>
    {
        [SerializeField] private Transform startPoint;
        private ScoreManager _scoreManager;
        private SpawnManager _spawnManager;

        protected override void Awake()
        {
            base.Awake();
            InitializeManagers();
            
        }
        
        private void Start() => StartGame();

        private void InitializeManagers()
        {
            _scoreManager ??= new ScoreManager();
            _spawnManager ??= new SpawnManager(this.transform, startPoint ? startPoint.position : Vector3.zero);
        }

        private void StartGame() => _spawnManager.OnSpawnFruit();
        private void EndGame() { }
    }
}