using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CharacterGenerator.Instancing
{
    [Serializable]
    public class Stringtionary : IDictionary<string, string>, ISerializationCallbackReceiver
    {
        #region Private

        [SerializeField, HideInInspector] private List<string> _serializedKeys = new List<string>(), _serializedValues = new List<string>();

        private IDictionary<string, string> _runtimeDictionary = new Dictionary<string, string>();

        #endregion

        #region Custom Public Utilities

        public string this [string key]
        {
            get => _runtimeDictionary[key];
            set => _runtimeDictionary[key] = value;
        }

        public T GetFromJson<T>(string key)
        {
            if (TryGetValue(key, out var value))
            {
                try
                {
                    return JsonUtility.FromJson<T>(value);
                }
                catch
                {
                    Debug.LogError($"Value is valid JSON of type {typeof(T).Name}");
                }
            }

            return default;
        }

        public int GetInt(string key)
        {
            if (TryGetValue(key, out var value))
            {
                if (int.TryParse(value, out var result))
                {
                    return result;
                }
            }
            
            return default;
        }
        
        public float GetFloat(string key)
        {
            if (TryGetValue(key, out var value))
            {
                if (float.TryParse(value, out var result))
                {
                    return result;
                }
            }
            
            return default;
        }
        
        public bool GetBool(string key)
        {
            if (TryGetValue(key, out var value))
            {
                if (bool.TryParse(value, out var result))
                {
                    return result;
                }
            }
            
            return default;
        }

        public void SetOrAdd(string key, string value)
        {
            if (!ContainsKey(key))
            {
                Add(key, value);
            }
            else
            {
                this[key] = value;
            }
        }

        #endregion

        #region Dictionary Implementations 
        
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _runtimeDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<string, string> item)
        {
            _runtimeDictionary.Add(item);
        }

        public void Clear()
        {
            _runtimeDictionary.Clear();
        }

        public bool Contains(KeyValuePair<string, string> item)
        {
            return _runtimeDictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            _runtimeDictionary.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, string> item)
        {
            return _runtimeDictionary.Remove(item);
        }

        
        public void Add(string key, string value)
        {
            _runtimeDictionary.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return _runtimeDictionary.ContainsKey(key);
        }

        public bool Remove(string key)
        {
            return _runtimeDictionary.Remove(key);
        }

        public bool TryGetValue(string key, out string value)
        {
            return _runtimeDictionary.TryGetValue(key, out value);
        }

        public ICollection<string> Keys => _runtimeDictionary.Keys;
        
        public ICollection<string> Values => _runtimeDictionary.Values;

        public int Count => _runtimeDictionary.Count;

        public bool IsReadOnly => _runtimeDictionary.IsReadOnly;
        
        #endregion 
        
        #region Serialization Callback Receiver Implementations
        
        public void OnBeforeSerialize()
        {
            _serializedKeys = _runtimeDictionary.Keys.ToList();
            _serializedValues = _runtimeDictionary.Values.ToList();
        }

        public void OnAfterDeserialize()
        {
            _runtimeDictionary = new Dictionary<string, string>();

            for (int i = 0; i < _serializedKeys.Count; i++)
            {
                _runtimeDictionary.Add(_serializedKeys[i], _serializedValues[i]);
            }

            _serializedKeys.Clear();
            _serializedValues.Clear();
        }
        
        #endregion
    }
}