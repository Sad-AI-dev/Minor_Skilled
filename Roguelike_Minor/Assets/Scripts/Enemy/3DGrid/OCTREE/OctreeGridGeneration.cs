using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace Game.Enemy.Pathfinding {
    public class OctreeGridGeneration : MonoBehaviour
    {
        public int mapSizeX;
        public int mapSizeY;
        public int mapSizeZ;

        public async void CreateGrid()
        {
            Collider[] overlaps = Physics.OverlapBox(transform.position, new Vector3(mapSizeX / 2, mapSizeY / 2, mapSizeZ / 2));

            if(overlaps.Length > 0)
            {

            }

            Task.Delay(2);
        }
    }
}
