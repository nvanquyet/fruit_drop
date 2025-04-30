using DesignPatterns;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "GameData", menuName = "Data/Game Data")]
    public class GameData : ScriptableObjectSingleton<GameData>
    {
        [SerializeField] private FruitData fruitData;
        
        
        //Getter
        public FruitData FruitData => fruitData;
    }
}