using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Core.Data;
using System;
using UnityEngine.AI;

namespace Game.Enemy {
    public class PGPrimaryAttackVars : Ability.AbilityVars
    {
        public Transform transform;
        public Transform target;
        public NavMeshAgent navAgent;
        public Core.Tree tree;
        public GameObject prefab;
        public Animator anim;
    }

    [CreateAssetMenu(fileName = "PrimaryAttack", menuName = "ScriptableObjects/Enemy/PlanetGrunt/PrimaryAttack")]
    public class PGPrimaryAttackSO : AbilitySO
    {
        public GameObject prefab;

        public override void InitializeVars(Ability source)
        {
            source.vars = new PGPrimaryAttackVars
            {
                transform = source.agent.transform,
                navAgent = source.agent.GetComponent<NavMeshAgent>(),
                tree = source.agent.GetComponent<MeleeGruntTree>(),
                prefab = prefab,
                anim = source.agent.GetComponent<Animator>()
            };
        }

        public override void Use(Ability source)
        {
            //Get and check vars
            PGPrimaryAttackVars vars = source.vars as PGPrimaryAttackVars;
            vars.navAgent.isStopped = true;
            vars.target = (Transform)vars.tree.root.GetData("Target");
            Vector3 targetPostition = new Vector3(vars.target.position.x, vars.transform.position.y, vars.target.position.z);

            vars.transform.LookAt(targetPostition);
            vars.prefab.GetComponent<PGPrimaryBehaviour>().source = source;
            vars.prefab.SetActive(true);
            vars.anim.SetTrigger("Slap");
        }
    }
}
