using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Enemy.Pathfinding
{
    public class Pathfinder
    {
        GridBuilder gridBase;
        public GNode startNode;
        public GNode endNode;

        public List<GNode> FindPath()
        {
            gridBase = GridBuilder.GetInstance();
            Debug.Log($"Finding path from {startNode.worldObject.name} to {endNode.worldObject.name}");

            return FindPathActual(startNode, endNode);
        }

        private List<GNode> FindPathActual(GNode start, GNode target)
        {
            List<GNode> foundPath = new List<GNode>();

            List<GNode> openSet = new List<GNode>();
            HashSet<GNode> closedSet = new HashSet<GNode>();

            openSet.Add(start);

            while (openSet.Count > 0)
            {
                GNode currentNode = openSet[0];

                for (int i = 0; i < openSet.Count; i++)
                {
                    if (openSet[i].fcost < currentNode.fcost ||
                        (openSet[i].fcost == currentNode.fcost &&
                        openSet[i].hcost < currentNode.hcost))
                    {
                        if (currentNode != openSet[i])
                        {
                            currentNode = openSet[i];
                        }
                    }
                }
                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode.Equals(target))
                {
                    foundPath = RetracePath(start, currentNode);
                    break;
                }
                foreach (GNode neighbour in GetNeighbours(currentNode, true))
                {
                    if (!closedSet.Contains(neighbour))
                    {
                        float newMovementCostToNeighbour = currentNode.gcost + GetDistance(currentNode, neighbour);

                        if (newMovementCostToNeighbour < neighbour.gcost || !openSet.Contains(neighbour))
                        {
                            neighbour.gcost = newMovementCostToNeighbour;
                            neighbour.hcost = GetDistance(neighbour, target);

                            neighbour.parent = currentNode;

                            if (!openSet.Contains(neighbour))
                            {
                                openSet.Add(neighbour);
                            }
                        }
                    }
                }
            }
            return foundPath;
        }

        public float GetDistance(GNode posA, GNode posB)
        {
            int distX = Mathf.Abs(posA.x - posB.x);
            int distY = Mathf.Abs(posA.y - posB.y);
            int distZ = Mathf.Abs(posA.z - posB.z);

            if (distX > distZ)
            {
                return 14 * distZ + 10 * (distX - distZ) + 10 * distY;
            }

            return 14 * distX + 10 * (distZ - distX) + 10 * distY;
        }

        public List<GNode> GetNeighbours(GNode currentNode, bool getVerticalNeighbors = false)
        {
            List<GNode> retList = new List<GNode>();

            for (int x = -1; x <= 1; x++)
            {
                for (int yIndex = -1; yIndex <= 1; yIndex++)
                {
                    for (int z = -1; z <= 1; z++)
                    {
                        int y = yIndex;

                        if (!getVerticalNeighbors)
                        {
                            y = 0;
                        }

                        if (x == 0 && y == 0 && z == 0)
                        {
                            //This is the current node;
                        }
                        else
                        {
                            GNode searchPos = new GNode();

                            searchPos.x = currentNode.x + x;
                            searchPos.y = currentNode.y + y;
                            searchPos.z = currentNode.z + z;

                            GNode newNode = GetNeighbourNode(searchPos, false, currentNode);
                            if (newNode != null)
                            {
                                retList.Add(newNode);
                            }
                        }
                    }
                }
            }
            return retList;
        }

        private GNode GetNeighbourNode(GNode adjPos, bool searchTopDown, GNode currentNodePos)
        {
            GNode retVal = null;

            GNode node = gridBase.GetNode(adjPos.x, adjPos.y, adjPos.z);

            if (node != null && node.walkable)
            {
                retVal = node;
            }
            else if (searchTopDown)
            {
                adjPos.y -= 1;
                GNode bottomBlock = gridBase.GetNode(adjPos.x, adjPos.y, adjPos.z);

                if (bottomBlock != null && bottomBlock.walkable)
                {
                    retVal = bottomBlock;
                }
                else
                {
                    adjPos.y += 2;
                    GNode topBlock = gridBase.GetNode(adjPos.x, adjPos.y, adjPos.z);

                    if (topBlock != null && topBlock.walkable)
                    {
                        retVal = topBlock;
                    }
                }
            }

            int origionalX = adjPos.x - currentNodePos.x;
            int origionalZ = adjPos.z - currentNodePos.z;

            if (Mathf.Abs(origionalX) == 1 && Mathf.Abs(origionalZ) == 1)
            {
                GNode neighbour1 = gridBase.GetNode(currentNodePos.x + origionalX, currentNodePos.y, currentNodePos.z);
                if (neighbour1 == null || !neighbour1.walkable)
                {
                    retVal = null;
                }

                GNode neighbour2 = gridBase.GetNode(currentNodePos.x, currentNodePos.y, currentNodePos.z + origionalZ);
                if (neighbour2 == null || !neighbour2.walkable)
                {
                    retVal = null;
                }

            }

            if (retVal != null)
            {

            }

            return retVal;
        }

        private List<GNode> RetracePath(GNode Start, GNode End)
        {
            List<GNode> path = new List<GNode>();
            GNode currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }

            path.Reverse();

            return path;

        }
    }
}
