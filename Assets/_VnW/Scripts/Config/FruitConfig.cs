using UnityEngine;
namespace Config
{
    [CreateAssetMenu(fileName = "FruitConfig", menuName = "Configs/Fruit Config")]
    public class FruitConfig : ScriptableObject
    {
        [Header("Drag Limit")]
        public float minX = -3f;
        public float maxX = 3f;
        
        public float smoothSpeed = 5f;
    }
}