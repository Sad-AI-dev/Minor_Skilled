using System.Linq;
using UnityEngine;

namespace Game.Enemy {
    public class NavMeshTerrainTreeHandling : MonoBehaviour
    {
        public Terrain terrain;

        [ContextMenu("Extract")]
        public void Extract()
        {
            Transform[] transforms = terrain.GetComponentsInChildren<Transform>();

            //Skip the first, since its the Terrain Collider
            for (int i = 1; i < transforms.Length; i++)
            {
                //Delete all previously created colliders first
                DestroyImmediate(transforms[i].gameObject);
            }

            for (int i = 0; i < terrain.terrainData.treePrototypes.Length; i++)
            {
                TreePrototype tree = terrain.terrainData.treePrototypes[i];

                //Get all instances matching the prefab index
                TreeInstance[] instances = terrain.terrainData.treeInstances.Where(x => x.prototypeIndex == i).ToArray();

                for (int j = 0; j < instances.Length; j++)
                {
                    //Un-normalize positions so they're in world-space
                    instances[j].position = Vector3.Scale(instances[j].position, terrain.terrainData.size);
                    instances[j].position += terrain.GetPosition();

                    //Fetch the collider from the prefab object parent
                    CapsuleCollider prefabCollider = tree.prefab.GetComponent<CapsuleCollider>();
                    if (!prefabCollider) continue;

                    GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                    obj.name = tree.prefab.name + j;

                    if (terrain.preserveTreePrototypeLayers) obj.layer = tree.prefab.layer;
                    else obj.layer = terrain.gameObject.layer;

                    Vector3 scale = new Vector3(1.5f, 5, 1.5f);
                    obj.transform.localScale = scale;
                    obj.transform.position = instances[j].position;
                    obj.transform.parent = terrain.transform;
                    obj.isStatic = true;
                }
            }
        }
    }
}
