using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Enemy.Pathfinding
{
    public class GridBuilder : MonoBehaviour
    {
        public int maxX = 10;
        public int maxY = 2;
        public int maxZ = 10;

        public float offsetX = 1;
        public float offsetY = 1;
        public float offsetZ = 1;

        public GNode[,,] grid;

        public Vector3 startNodePossition;
        public Vector3 endNodePossition;

        public List<GNode> lastPath;

        [SerializeField] bool DrawGizmos = false;

        public bool start;
        private void Update()
        {
            if (start)
            {
                start = false;
                Pathfinder path = new Pathfinder();

                GNode startNode = GetNodeFromVector3(startNodePossition);
                GNode targetNode = GetNodeFromVector3(endNodePossition);

                path.startNode = startNode;
                path.endNode = targetNode;

                List<GNode> p = path.FindPath();
                lastPath = p;

                foreach (GNode n in p)
                {
                    //Debug.Log(n.worldObject.name);
                    //n.worldObject.SetActive(false);
                }

            }
        }

        public GNode GetNode(int x, int y, int z)
        {
            GNode retVal = null;

            if (x < maxX && x >= 0
                && y < maxY && y >= 0
                && z < maxZ && z >= 0)
            {
                retVal = grid[x, y, z];
            }

            return retVal;
        }

        public GNode GetNodeFromVector3(Vector3 pos)
        {
            int x = Mathf.RoundToInt(pos.x);
            int y = Mathf.RoundToInt(pos.y);
            int z = Mathf.RoundToInt(pos.z);

            GNode retVal = GetNode(x, y, z);

            return retVal;
        }

        public static GridBuilder instance;
        public static GridBuilder GetInstance()
        {
            return instance;
        }

        public List<Vector3> GetPath(Vector3 startpos, Vector3 target, float targetOffset)
        {
            Pathfinder path = new Pathfinder();

            GNode startNode = GetNodeFromVector3(startpos);
            GNode targetNode = GetNodeFromVector3(target);

            path.startNode = startNode;
            path.endNode = targetNode;

            List<GNode> p = path.FindPath();
            List<Vector3> retPath = new List<Vector3>();

            foreach (var waypoint in p)
            {
                retPath.Add(waypoint.worldObject.transform.position += (Random.insideUnitSphere * targetOffset));
            }

            return retPath;
        }

        public float GetPathDistance(List<Vector3> path)
        {
            float totalDistance = 0;
            for (int i = 0; i < path.Count - 1; i++)
            {
                totalDistance += Vector3.Distance(path[i], path[i + 1]);
            }
            return totalDistance;
        }

        public void BuildGrid()
        {
            StartCoroutine(PlaceNode());
        }

        IEnumerator PlaceNode()
        {
            grid = new GNode[maxX, maxY, maxZ];
            for (int x = 0; x < maxX; x++)
            {
                for (int y = 0; y < maxY; y++)
                {
                    for (int z = 0; z < maxZ; z++)
                    {
                        float xpos = x * offsetX + transform.position.x;
                        float ypos = y * offsetY + transform.position.y;
                        float zpos = z * offsetZ + transform.position.z;

                        GameObject go = Instantiate(new GameObject(), new Vector3(xpos, ypos, zpos), Quaternion.identity);
                        go.transform.name = $"{x}-{y}-{z}";
                        go.transform.parent = transform;

                        GNode node = new GNode();
                        node.x = x;
                        node.y = y;
                        node.z = z;
                        node.walkable = true;
                        node.worldObject = go;

                        grid[x, y, z] = node;

                        RaycastHit hit;
                        if (Physics.Raycast(new Vector3(xpos, ypos, zpos), transform.up, out hit, 100))
                        {
                            node.walkable = false;
                        }

                    }
                    yield return new WaitForSeconds(1);
                }
            }
        }
        

        private void Awake()
        {
            instance = this;
        }

        private void OnDrawGizmos()
        {
            if (DrawGizmos)
            {
                for (int x = 0; x < maxX; x++)
                {
                    for (int y = 0; y < maxY; y++)
                    {
                        for (int z = 0; z < maxZ; z++)
                        {
                            float xpos = x * offsetX + transform.position.x;
                            float ypos = y * offsetY + transform.position.y;
                            float zpos = z * offsetZ + transform.position.z;

                            Gizmos.color = Color.green;
                            Gizmos.DrawSphere(new Vector3(xpos, ypos, zpos), 0.2f);
                        }
                    }
                }
            }

            if (lastPath != null && lastPath.Count > 0)
            {
                foreach (var item in lastPath)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawSphere(item.worldObject.transform.position, 0.22f);
                }
            }
        }
    }
}
