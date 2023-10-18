using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Enemy.Pathfinding
{
    public class WorldPosTest : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                GNode node = GridBuilder.GetInstance().GetNodeClosestToWorldPos(transform.position);
                Debug.Log($"Node pos: {node.x}-{node.y}-{node.z}\n" +
                    $"Node World Pos: {node.worldPosition}" +
                    $"Walkable: {node.walkable}"
                    );
            }
        }
    }
}
