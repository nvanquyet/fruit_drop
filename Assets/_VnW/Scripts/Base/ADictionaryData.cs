using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Base
{
    public abstract class ADictionaryData<TK, TV> : ScriptableObject
    {
        [SerializeField] protected TV[] dataArray;

        protected readonly Dictionary<TK, TV> DataDictionary = new();

        public virtual bool GetValue(TK key, out TV result)
        {
            if (DataDictionary.Count == 0) InitializeDictionary();
            result = default(TV);
            return DataDictionary.TryGetValue(key, out result);
        }

        public virtual TV[] GetAllValues()
        {
            if (DataDictionary.Count == 0) InitializeDictionary();
            return dataArray;
        }

        protected abstract void InitializeDictionary();
    }
}