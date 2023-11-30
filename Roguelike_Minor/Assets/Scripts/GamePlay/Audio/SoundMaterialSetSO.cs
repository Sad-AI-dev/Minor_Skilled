using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;
using Game.Core.Data;

namespace Game {
    [CreateAssetMenu(fileName = "SoundMaterialSet", menuName = "ScriptableObjects/Audio/SoundMaterialSet")]
    public class SoundMaterialSetSO : ScriptableObject
    {
        [SerializeField] private Switch defaultValue;

        [SerializeField] private UnityDictionary<List<TerrainLayer>, Switch> terrainLookup;

        public Switch GetSwitchByTerrain(TerrainLayer layer)
        {
            foreach (var kvp in terrainLookup)
            {
                foreach (TerrainLayer terrainLayer in kvp.Key)
                {
                    if (terrainLayer == layer)
                    {
                        return kvp.Value;
                    }
                }
            }

            return defaultValue;
        }
    }
}
