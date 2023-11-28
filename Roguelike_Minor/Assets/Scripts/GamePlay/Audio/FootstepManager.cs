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
                        transform.position - new Vector3(0, 0.5f * controller.height + 0.5f * controller.radius, 0),
                        Vector3.down, out RaycastHit hit, 1f))
                {
                    if (hit.collider.TryGetComponent(out Terrain terrain))
                    {
                        Debug.Log("Hit Terrain");

                        SetFootstepSFXTerrain(terrain, hit.point);
                    }
                    else if (hit.collider.TryGetComponent(out GameObject obj))
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

            int primaryIndex = 0;
            for (int i = 0; i < alphamap.Length; i++)
            {
                if (alphamap[0, 0, i] > alphamap[0, 0, primaryIndex])
                {
                    primaryIndex = i;
                }
            }

            Debug.Log(primaryIndex);
            Debug.Log(terrain.terrainData.terrainLayers[primaryIndex]);

            AK.Wwise.Switch sound = soundMaterialSet.GetSwitchByTerrain(terrain.terrainData.terrainLayers[primaryIndex]);
            sound.SetValue(gameObject);
        }

        private IEnumerator SetFootstepSFXRender(GameObject obj)
        {
            return null;
        }

    }
}