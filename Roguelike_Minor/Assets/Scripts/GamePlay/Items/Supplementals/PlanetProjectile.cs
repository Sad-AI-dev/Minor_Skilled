using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    [RequireComponent(typeof(MeshRenderer))]
    public class PlanetProjectile : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float baseMoveSpeed;
        [SerializeField] private float bonusMoveSpeed;
        [SerializeField] private float yOffset = 1f;

        [Header("Visuals Settings")]
        [SerializeField] private List<Material> mats;

        //holder vars
        [HideInInspector] public Item18SO.Item18Vars holderVars;
        [HideInInspector] public int ringCapacity;
        [HideInInspector] public int numInRing;

        //movement settings
        [HideInInspector] public float targetRadius;

        //vars
        private float moveSpeed;

        //================ Setup ===================
        private void Start()
        {
            SetupVisuals();
            InitializeVars();
        }

        private void SetupVisuals()
        {
            GetComponent<MeshRenderer>().material = mats[Random.Range(0, mats.Count)];
        }

        private void InitializeVars()
        {
            moveSpeed = baseMoveSpeed + (ringCapacity * bonusMoveSpeed);
        }

        //============== Update Loop =================
        private void Update()
        {
            Move();
        }

        //======= Move ======
        private void Move()
        {
            float fract = (((float)numInRing * (Mathf.PI * 2)) / (float)ringCapacity);
            float xPos = Mathf.Cos((Time.time * moveSpeed) + fract);
            float zPos = Mathf.Sin((Time.time * moveSpeed) + fract);
            transform.localPosition = new Vector3(xPos, 0, zPos) * targetRadius + Vector3.up * yOffset;
        }
    }
}
