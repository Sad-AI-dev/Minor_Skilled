using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;
using Game.Core.Data;

namespace Game {
    [CreateAssetMenu(fileName = "SoundMaterialSet", menuName = "ScriptableObjects/Audio/SoundMaterialSet")]
    public class SoundMaterialSetSO : ScriptableObject
    {
        [SerializeField] private RTPC defaultValue;

        [SerializeField] private UnityDictionary<List<TerrainLayer>, RTPC> terrainLookup;

        public RTPC GetParamByTerrain(TerrainLayer layer)
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
