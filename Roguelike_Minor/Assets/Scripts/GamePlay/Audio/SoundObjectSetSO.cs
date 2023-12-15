using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;
using Game.Core.Data;

namespace Game {
    [CreateAssetMenu(fileName = "SoundObjectSet", menuName = "ScriptableObjects/Audio/SoundObjectSet")]
    public class SoundObjectSetSO : ScriptableObject
    {
        [System.Serializable]
        public class RTPCValuePair
        {
            [Range(0f, 100f)] public float value;
            public RTPC parameter;
        }

        [SerializeField] private RTPCValuePair[] defaultValue;

        [SerializeField] private UnityDictionary<List<Material>, RTPCValuePair[]> materialLookup;

        public RTPCValuePair[] GetRTPCValuePairFromMaterial(Material mat)
        {
            foreach (var pair in materialLookup)
            {
                if (pair.Key.Contains(mat))
                {
                    return pair.Value;
                }
            }
            return defaultValue;
        }
    }
}
