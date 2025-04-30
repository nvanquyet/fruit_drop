using GameEvents;
using DesignPatterns;

namespace Controller
{
    public class ScoreManager
    {
        private int _score;
        
        public ScoreManager()
        {
            _score = 0;
            ObserverManager.Instance.Subscribe<GameEvent.OnFruitMerged>(OnIncreaseScore);
        }

        private void OnIncreaseScore(GameEvent.OnFruitMerged param)
        {
            //Logic show score;
            _score += 1;
        }
        
        
        private void Dispose()
        {
            ObserverManager.Instance.Unsubscribe<GameEvent.OnFruitMerged>(OnIncreaseScore);
        }
    }
}