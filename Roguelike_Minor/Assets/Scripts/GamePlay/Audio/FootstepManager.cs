using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;

using RTPCValuePair = Game.SoundObjectSetSO.RTPCValuePair;

namespace Game {
    public class FootstepManager : MonoBehaviour
    {
        [Header("Sound Settings")]
        [SerializeField] private RTPC[] parameters;
        [SerializeField] private SoundMaterialSetSO soundMaterialSet;
        [SerializeField] private SoundObjectSetSO soundObjectSet;

        [Header("Raycast Settings")]
        [SerializeField] private LayerMask floorlayer;

        public void CheckGround()
        {
            if (TryFindGround(out RaycastHit hit)) {
                ResetVolumes();
                //unmute correct sounds
                if (hit.collider.TryGetComponent(out Terrain terrain))
                {
                    SetFootstepSFXTerrain(terrain, hit.point);
                }
                else if (hit.collider.TryGetComponent(out MeshRenderer meshRenderer))
                {
                    SetFootstepSFXObject(meshRenderer);
                }
            }
        }
        private bool TryFindGround(out RaycastHit hit)
        {
            //raycast
            bool result = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitResult, 0.5f);
            hit = hitResult;
            //return result
            return result;
        }

        //=========== Reset Volumes ============
        private void ResetVolumes()
        {
            //mute all sounds
            Array.ForEach(parameters, (RTPC param) => param.SetGlobalValue(0f));
        }

        //============ Terrain sound fx =============
        private void SetFootstepSFXTerrain(Terrain terrain, Vector3 hitpoint)
        {
            Vector3 TerrainPos = hitpoint - terrain.transform.position;
            Vector3 SplatMapPos = new Vector3(TerrainPos.x / terrain.terrainData.size.x, 0,
                TerrainPos.z / terrain.terrainData.size.z);

            int x = Mathf.FloorToInt(SplatMapPos.x * terrain.terrainData.alphamapWidth);
            int z = Mathf.FloorToInt(SplatMapPos.z * terrain.terrainData.alphamapHeight);

            float[,,] alphamap = terrain.terrainData.GetAlphamaps(x, z, 1, 1);

            Dictionary<int, float> layersAtPosition = new Dictionary<int, float>();

            for (int i = 0; i < alphamap.Length; i++)
            {
                layersAtPosition.Add(i, alphamap[0, 0, i]);
            }

            Dictionary<RTPC, float> percentagePerLayerType = new Dictionary<RTPC, float>();
            foreach (var kvp in layersAtPosition)
            {
                RTPC param = soundMaterialSet.GetParamByTerrain(terrain.terrainData.terrainLayers[kvp.Key]);
                if (!percentagePerLayerType.ContainsKey(param))
                {
                    if (kvp.Value > 0f) { //don't register 0% layers
                        percentagePerLayerType.Add(param, kvp.Value);
                    }
                }
                else
                {
                    percentagePerLayerType[param] += kvp.Value;
                }
            }

             foreach (var kvp in percentagePerLayerType)
             {
                 kvp.Key.SetGlobalValue(kvp.Value * 100f);
             }
        }

        //================ GameObject Sounds ==================
        private void SetFootstepSFXObject(MeshRenderer meshRenderer)
        {
            RTPCValuePair[] parameters = soundObjectSet.GetRTPCValuePairFromMaterial(meshRenderer.sharedMaterial);
            //set volumes based on parameters
            Array.ForEach(parameters, (RTPCValuePair valuePair) => valuePair.parameter.SetGlobalValue(valuePair.value));
        }
    }
}