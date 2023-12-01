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

        [Header("Visuals Settings")]
        [SerializeField] private List<Material> mats;

        //holder vars
        [HideInInspector] public Agent holder;
        [HideInInspector] public int ringIndex;

        //movement settings
        [HideInInspector] public float targetRadius;

        //damage settings
        [HideInInspector] public float damageMult;

        //================ Setup ===================
        private void Start()
        {
            SetupVisuals();
        }

        private void SetupVisuals()
        {
            GetComponent<MeshRenderer>().material = mats[Random.Range(0, mats.Count)];
        }
    }
}
