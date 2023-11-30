using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Game {
    public class FootstepManager : MonoBehaviour
    {
        [SerializeField] private AK.Wwise.Event footstepSFX;

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

        public void PlayFootstep()
        {
            footstepSFX.Post(gameObject);
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
                        Debug.Log("Hit object");
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

            int primaryIndex = 0;
            for (int i = 0; i < alphamap.Length; i++)
            {
                layersAtPosition.Add(i, alphamap[0,0,i]);
                if (alphamap[0, 0, i] > alphamap[0, 0, primaryIndex])
                {
                    primaryIndex = i;
                }
            }

            //Debug.Log(terrain.terrainData.terrainLayers[primaryIndex]);
            // string msg = "Terrain Data found: \n\n";
            // foreach (var kvp in layersAtPosition)
            // {
            //     msg += $"layer data: layer Index: {kvp.Key}, value {kvp.Value}\n";
            // }
            // Debug.Log(msg);

            AK.Wwise.Switch sound = soundMaterialSet.GetSwitchByTerrain(terrain.terrainData.terrainLayers[primaryIndex]);
            sound.SetValue(gameObject);
        }

        private IEnumerator SetFootstepSFXRender(GameObject obj)
        {
            return null;
        }

    }
}