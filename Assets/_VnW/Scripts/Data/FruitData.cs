using Base;
using Object;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace Data
{
    #region Struct

    [System.Serializable]
    public struct FruitStruct
    {
        public int id;
        public float sizeMultiplier;
        public int scoreClaim;
        public float mass;

        public FruitVisualStruct visualData;
    }
    
    [System.Serializable]
    public struct FruitVisualStruct
    {
        public Sprite fruitSprite;
        // public AnimationClip idleAnimation;
        // public AnimationClip collectAnimation;
        // public Color fruitColor;
        // public ParticleSystem collectEffect;
    }

    #endregion
   

    [CreateAssetMenu(menuName = "Data/Fruit/Fruit Data", fileName = "FruitData")]
    public class FruitData : ADictionaryData<int, FruitStruct>
    {
        [SerializeField] private Fruit fruitPrefab;
        
        public Fruit FruitPrefab => fruitPrefab;

        protected override void InitializeDictionary()
        {
            DataDictionary.Clear();
            foreach (var v in dataArray)
            {
                DataDictionary.Add(v.id, v);
            }
        }


    #if UNITY_EDITOR
        [ContextMenu("Generate data automatically")]
        public void GenerateData()
        {
            dataArray = new FruitStruct[10];

            // Load all sprites from the folder
            var spriteFolderPath = "Assets/_VnW/Sprite/Fruit/";
            var spriteGuids = AssetDatabase.FindAssets("t:Sprite", new[] { spriteFolderPath });
            var sprites = new Sprite[spriteGuids.Length];
            for (var i = 0; i < spriteGuids.Length; i++)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(spriteGuids[i]);
                sprites[i] = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
            }

            for (var i = 0; i < dataArray.Length; i++)
            {
                dataArray[i] = new FruitStruct
                {
                    id = i,
                    sizeMultiplier = 0.5f + i * 0.1f,
                    scoreClaim = (int)Mathf.Pow(2, i + 1),
                    mass = 0.5f + i * 0.2f,
                    visualData = new FruitVisualStruct
                    {
                        fruitSprite = i < sprites.Length ? sprites[i] : null
                    }
                };
            }
        }
    #endif


        
        /// <summary>
        /// Logic to get random fruit data
        /// </summary>
        /// <returns></returns>
        public FruitStruct GetRandomFruitData()
        {
            return dataArray[0];
        }
    }
}