using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Util
{
    public class NavMeshUtil : MonoBehaviour
    {
        public static bool RandomNavmeshLocation(out Vector3 position, Vector3 center, float radius)
        {
            Vector2 randDirection = Random.insideUnitCircle * radius;
            Vector3 randomPoint = new Vector3(randDirection.x, 0, randDirection.y) + center;
            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, radius, 1))
            {
                position = hit.position;
                return true;
            }
            //fail fallback
            else 
            {
                position = Vector3.zero;
                return false;
            }
        }

        public static bool RandomNavmeshLocationAtDistance(out Vector3 position, Vector3 center, float radius, float allowedRadius = 1f)
        {
            position = new Vector3();
            float bestOffset = 100f;

            for (int i = 0; i < 100; i++)
            {
                Vector2 randDirection = Random.insideUnitCircle.normalized * radius;
                Vector3 randomPoint = new Vector3(randDirection.x, 0, randDirection.y) + center;
                if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, radius, 1))
                {
                    float offset = GetOffset(randomPoint, hit.position);
                    if (offset < allowedRadius)
                    { //found allowed position
                        position = hit.position;
                        return true;
                    }
                    else if (offset < bestOffset)
                    {
                        bestOffset = offset;
                        position = hit.position;
                    }
                }
            }
            return false;
        }

        private static float GetOffset(Vector3 searchPos, Vector3 foundPos)
        {
            return Vector2.Distance(new Vector2(searchPos.x, searchPos.y), new Vector2(foundPos.x, foundPos.y));
        }
    }
}
