using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Core.Data {
    [Serializable]
    public class UnityDictionary<Key, Value> : IDictionary<Key, Value>, ISerializationCallbackReceiver
    {
        [System.Serializable]
        public struct Pair
        {
            public Key key;
            public Value value;
        }

        [SerializeField] private List<Pair> dictionary;

        private Dictionary<Key, Value> dict;

        //ctor
        public UnityDictionary()
        {
            dictionary = new List<Pair>();
            dict = new Dictionary<Key, Value>();
        }

        public UnityDictionary(Dictionary<Key, Value> dictionary)
        {
            this.dictionary = new List<Pair>();
            foreach (KeyValuePair<Key,Value> pair in dictionary) {
                this.dictionary.Add(new Pair() { key = pair.Key, value = pair.Value });
            }

            dict = new Dictionary<Key, Value>(dictionary);
        }

        //================= serialization =================
        //dictionary to list
        public void OnBeforeSerialize()
        {
            //initialize values if needed
            dict ??= new Dictionary<Key, Value>();
            dictionary ??= new List<Pair>();

            TryPopulateList();
        }
        private void TryPopulateList()
        {
            if (IsValidList()) {
                dictionary.Clear();

                //populate list from dict
                foreach (KeyValuePair<Key, Value> kvp in dict) {
                    dictionary.Add(new Pair { key = kvp.Key, value = kvp.Value });
                }
            }
        }
        private bool IsValidList()
        {
            Key[] keys = new Key[dictionary.Count];
            for (int i = 0; i < dictionary.Count; i++) {
                if (keys.Contains(dictionary[i].key)) { //found dupe key, list is invalid
                    return false;
                }
                keys[i] = dictionary[i].key;
            }
            return true; //no dupes
        }

        //list to dictionary
        public void OnAfterDeserialize()
        {
            dict = new Dictionary<Key, Value>();
            ListToValidDictionary();
        }

        private void ListToValidDictionary()
        {
            for (int i = 0; i < dictionary.Count; i++) {
                if (!dict.ContainsKey(dictionary[i].key)) {
                    dict.Add(dictionary[i].key, dictionary[i].value);
                }
            }
        }

        //=================== dictionary interfacing ===================
        public Value this[Key key]
        {
            get { return dict[key]; }
            set { dict[key] = value; }
        }

        //=== properties ===
        public int Count { get { return dict.Count; } }
        public ICollection<Key> Keys { get { return dict.Keys; } }
        public ICollection<Value> Values { get { return dict.Values; } }
        public bool IsReadOnly { get { return false; } }

        //=== add/remove/clear ===
        public void Add(Key key, Value value) { dict.Add(key, value); }
        public void Add(KeyValuePair<Key, Value> pair) { dict.Add(pair.Key, pair.Value); }

        public bool Remove(Key key) { return dict.Remove(key); }
        public bool Remove(KeyValuePair<Key, Value> pair) { return dict.Remove(pair.Key); }

        public void Clear() { dict.Clear(); }

        public bool Contains(KeyValuePair<Key, Value> pair) { return dict.Contains(pair); }
        public bool ContainsKey(Key key) { return dict.ContainsKey(key); }
        public bool ContainsValue(Value value) { return dict.ContainsValue(value); }

        public bool TryGetValue(Key key, out Value value)
        {
            if (ContainsKey(key)) {
                value = this[key];
                return true;
            }
            value = default; //not found
            return false;
        }

        //ICollection Method
        public void CopyTo(KeyValuePair<Key, Value>[] pairs, int startIndex)
        {
            pairs = new KeyValuePair<Key, Value>[Count - startIndex];
            int counter = 0;
            foreach (KeyValuePair<Key, Value> pair in dict) {
                if (counter >= startIndex) { 
                    pairs[counter - startIndex] = pair;
                }
                counter++;
            }
        }

        //custom foreach support
        public IEnumerator<KeyValuePair<Key, Value>> GetEnumerator() { return dict.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

        //==== Type Casts ====
        public static implicit operator Dictionary<Key, Value> (UnityDictionary<Key, Value> dict) {
            return dict.dict;
        }
        public static implicit operator UnityDictionary<Key, Value> (Dictionary<Key, Value> dict)
        {
            return new UnityDictionary<Key, Value>(dict);
        }
    }
}
