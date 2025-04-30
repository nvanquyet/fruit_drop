using DesignPatterns;
using Object;
using UnityEngine;

namespace GameEvents
{
    public abstract class GameEvent
    {
        public struct OnScoreUpdate : IEventData
        {
            public int ScoreToAdd;
        }
        
        public struct OnFruitSpawned : IEventData
        {
            public Fruit Fruit;
        }
        
        public struct OnFruitMerged : IEventData
        {
            public Fruit SourceFruit;
            public Fruit TargetFruit;
        }
        
        
        public struct OnFruitDrop : IEventData { }
        
        public struct GameStarted : IEventData { }

        public struct GameEnded : IEventData
        {
            public bool IsWin;
            public int Score;
        }
    }
}