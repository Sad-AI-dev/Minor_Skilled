using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Util
{
    public class NavMeshUtil : MonoBehaviour
    {
        public static Vector3 RandomNavmeshLocation(Vector3 center, float radius)
        {
            Vector2 randDirection = Random.insideUnitCircle * radius;
            Vector3 randomPoint = new Vector3(randDirection.x, 0, randDirection.y) + center;
            NavMeshHit hit;
            Vector3 finalPosition = Vector3.zero;
            if (NavMesh.SamplePosition(randomPoint, out hit, radius, 1))
            {
                finalPosition = hit.position;
            }
            return finalPosition;
        }

        public static Vector3 RandomNavmeshLocationAtDistance(Vector3 center, float radius)
        {
            Vector2 randDirection = Random.insideUnitCircle.normalized * radius;
            Vector3 randomPoint = new Vector3(randDirection.x, 0, randDirection.y) + center;
            NavMeshHit hit;
            Vector3 finalPosition = Vector3.zero;
            if (NavMesh.SamplePosition(randomPoint, out hit, radius, 1))
            {
                finalPosition = hit.position;
            }
            return finalPosition;
        }
    }
}
