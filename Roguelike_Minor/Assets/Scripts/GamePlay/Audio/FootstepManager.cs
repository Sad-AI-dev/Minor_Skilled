using System;
using System.Collections;
using System.Collections.Generic;
using AK.Wwise;
using Unity.VisualScripting;
using UnityEngine;

namespace Game {
    public class FootstepManager : MonoBehaviour
    {

        [SerializeField] private SoundMaterialSetSO soundMaterialSet;

        [SerializeField] private LayerMask floorlayer;

        private CharacterController controller;
        private bool blendTerrainTiles;

        private void Start()
        {
            controller = GetComponent<CharacterController>();
            StartCoroutine(CheckGround());
        }

        private void Update()
        {
            //CheckGround();
        }

     

        private IEnumerator CheckGround()
        {
            while (true)
            {
                if (controller.velocity != Vector3.zero && Physics.Raycast(
                        transform.position - new Vector3(0, 0, 0),
                        Vector3.down, out RaycastHit hit, 0.5f))
                {
                    if (hit.collider.TryGetComponent(out Terrain terrain))
                    {
                        SetFootstepSFXTerrain(terrain, hit.point);
                    }
                    else 
                    {
                        // Debug.Log("Hit object");
                    }
                }

                yield return null;
            }
        }

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
                RTPC param = soundMaterialSet.GetSwitchByTerrain(terrain.terrainData.terrainLayers[kvp.Key]);
                if (!percentagePerLayerType.ContainsKey(param))
                {
                    percentagePerLayerType.Add(param, kvp.Value);
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

        private IEnumerator SetFootstepSFXRender(GameObject obj)
        {
            return null;
        }

    }
}