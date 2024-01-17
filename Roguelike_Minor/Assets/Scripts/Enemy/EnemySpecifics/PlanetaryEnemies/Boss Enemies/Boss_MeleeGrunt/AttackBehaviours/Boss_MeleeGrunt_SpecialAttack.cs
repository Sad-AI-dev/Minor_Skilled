using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Core;

namespace Game.Enemy
{
    public class Boss_MeleeGrunt_SpecialAttack : MonoBehaviour
    {
        public List<Transform> rocksInRange = new List<Transform>();
        public GameObject rockPrefab;

        public List<Vector3> rockPos = new List<Vector3>();
        public Vector3 baseRotation;
        List<float> rotationsY = new List<float>() { 
        45, 180, -90};

        public void OnSpecialUse()
        {
            if(rocksInRange.Count > 0)
            {
                ThrowRocks();
            }
            else
            {
                PlaceRocks();
            }
        }
        Vector3 knockbackDirection;
        void ThrowRocks()
        {
            Debug.Log("Throwing rocks");

            for (int i = 0; i < rocksInRange.Count; i++)
            {
                Rigidbody rb = rocksInRange[i].GetComponent<Rigidbody>(); 
                knockbackDirection = (rocksInRange[i].position - transform.position).normalized;
                rb.isKinematic = false;
                rb.useGravity = true;
                rocksInRange[i].GetComponent<MeshCollider>().isTrigger = true;
                StartCoroutine(rocksInRange[i].GetComponent<SpikeRise>().launched());
                rb.velocity = (knockbackDirection + (Vector3.up * 5)) * 3;
                rocksInRange[i].gameObject.layer = 0;
                //rocksInRange.Remove(rocksInRange[i])
            }
        }

        void PlaceRocks()
        {
            Debug.Log("Placing rocks");

            for (int i = 0; i < rockPos.Count; i++)
            {
                Vector3 pos = GetNavPos(transform.position +  rockPos[i]);
                if(pos != Vector3.zero)
                {
                    pos.y = pos.y = -7;
                    GameObject rock = Instantiate(rockPrefab, pos, Quaternion.identity);
                    Vector3 alter = baseRotation;
                    alter.y = rotationsY[i];
                    rock.transform.rotation = Quaternion.Euler(alter);
                }
            }
        }

        Vector3 GetNavPos(Vector3 posToSample)
        {
            NavMeshHit closestHit;
            if (NavMesh.SamplePosition(posToSample, out closestHit, 500f, NavMesh.AllAreas))
            {
                return closestHit.position;
            }
            else
                return Vector3.zero;
        }


        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Special_Rock")
            {
                if (!rocksInRange.Contains(other.transform))
                {
                    rocksInRange.Add(other.transform);
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Special_Rock")
            {
                if (rocksInRange.Contains(other.transform))
                {
                    rocksInRange.Remove(other.transform);
                }
            }
        }


    }
}
