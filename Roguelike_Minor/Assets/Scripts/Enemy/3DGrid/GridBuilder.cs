using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace Game.Enemy.Pathfinding {
    public class GridBuilder : MonoBehaviour
    {
        public int maxX = 10;
        public int maxY = 2;
        public int maxZ = 10;

        public float offsetX = 1;
        public float offsetY = 1;
        public float offsetZ = 1;

        [HideInInspector][SerializeField]GNode[] grid;

        public Vector3 startNodePossition;
        public Vector3 endNodePossition;

        [HideInInspector] public List<GNode> lastPath;

        [SerializeField] bool DrawGizmos = false;

        [Header("Builder Variables")]
        [SerializeField] int RaycastHeight = 60;
        [SerializeField] Vector3 SpecificNode;

        [Header("Pathfinding Specific")]
        public bool start = false;
        
        /// <summary>
        /// INSTANCE
        /// </summary>
        public static GridBuilder instance;
        public static GridBuilder GetInstance()
        {
            return instance;
        }

        /// <summary>
        /// PATHFINDING
        /// </summary>
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
                retPath.Add(waypoint.worldPosition += (Random.insideUnitSphere * targetOffset));
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

        /// <summary>
        /// BUTTONS
        /// </summary>
        //In between for generating grid;
        public void BuildGrid()
        {
            CreateNodes();
        }
        
        //Get length of grid
        public void GetGrid()
        {
            if (grid != null) Debug.Log(grid.Length);
            else Debug.Log("No grid");
        }
        
        //Clear the entire grid
        public void ClearGrid()
        {
            grid = null;
            Debug.Log("Cleared grid");
        }

        //Get Specific node info.
        public void GetSpecificNode()
        {
            int x = Mathf.RoundToInt(SpecificNode.x);
            int y = Mathf.RoundToInt(SpecificNode.y);
            int z = Mathf.RoundToInt(SpecificNode.z);
            if (grid[GetNodeArray(x, y, z)] == null)
            {
                Debug.Log("there is no grid");
            }
            else if(grid[GetNodeArray(x, y, z)] == null)
            {
                Debug.Log("This node does not exist");
            }
            else
            {
                Debug.Log($"Node: {x}-{y}-{z}\n" +
                    $"Walkable: {grid[GetNodeArray(x, y, z)].walkable}\n" +
                    $"World Position: {grid[GetNodeArray(x, y, z)].worldPosition}\n" +
                    $"Node Type: {grid[GetNodeArray(x, y, z)].nodeType}\n"
                    );
            }
        }


        /// <summary>
        /// ARRAY HANDLING
        /// </summary>
        //Generate nodes
        private async void CreateNodes()
        {
            Debug.Log("Creating Grid");
            grid = new GNode[maxX * maxY * maxZ];

            for (int x = 0; x < maxX; x++)
            {
                for (int y = 0; y < maxY; y++)
                { 
                    for (int z = 0; z < maxZ; z++)
                    {
                        float xpos = x * offsetX + transform.position.x;
                        float ypos = y * offsetY + transform.position.y;
                        float zpos = z * offsetZ + transform.position.z;

                        GNode node = new GNode();
                        node.x = x;
                        node.y = y;
                        node.z = z;
                        node.walkable = true;
                        node.worldPosition = new Vector3(xpos, ypos, zpos);
                        node.nodeType = NodeType.air;

                        grid[GetNodeArray(x,y,z)] = node;

                        RaycastHit hit;
                        if (Physics.Raycast(new Vector3(xpos, 60, zpos), -transform.up, out hit, Mathf.Infinity))
                        {
                            if(hit.point.y > ypos)
                            {
                                node.walkable = false;
                            }
                        }
                    }
                }
                await Task.Delay(1);
            }
            Debug.Log("grid Creation Complete");
        }
        //get node at position x, y, z
        private int GetNodeArray(int x, int y, int z)
        {
            return x + (y * maxX) + (z * maxX * maxY);
        }
        //get vector3 from index
        private Vector3 GetNodePos(int index)
        {
            return new Vector3(
              Mathf.RoundToInt(index % maxX), //loops from 0 to maxX
              Mathf.RoundToInt(Mathf.Floor((float)(index % (maxX * maxY)) / (float)maxX)), //divide by maxX, but resets every maxX * maxY
              Mathf.RoundToInt(Mathf.Floor((float)index / (float)(maxX * maxY))) //devide by maxX * maxY
            );
        }
        public GNode GetNode(int x, int y, int z)
        {
            GNode retVal = null;

            if (x < maxX && x >= 0
                && y < maxY && y >= 0
                && z < maxZ && z >= 0)
            {
                retVal = grid[GetNodeArray(x, y, z)];
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

        /// <summary>
        /// UNITY METHODS
        /// </summary>
        private void Awake()
        {
            instance = this;
        }
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
            }
        }
        private void OnDrawGizmos()
        {
            if (DrawGizmos)
            {
                if (lastPath != null && lastPath.Count > 0)
                {
                    foreach (var item in lastPath)
                    {
                        Gizmos.color = Color.blue;
                        Gizmos.DrawSphere(item.worldPosition, 0.22f);
                    }
                }
            }
        }
    }
}
