using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Core.Data;
using System;

namespace Game.Enemy {
    public class PGPrimaryAttackVars : Ability.AbilityVars
    {
        //public BehaviourPool<PGPrimaryBehaviour> behaviourPool;
        public GameObject prefab;
        public Animator anim;

    }

    [CreateAssetMenu(fileName = "PrimaryAttack", menuName = "ScriptableObjects/Enemy/PlanetGrunt/PrimaryAttack")]
    public class PGPrimaryAttackSO : AbilitySO
    {
        public GameObject prefab;

        public override void InitializeVars(Ability source)
        {
            
        }

        public override void Use(Ability source)
        {  
            PGPrimaryAttackVars vars = source.vars as PGPrimaryAttackVars;
            vars.prefab.GetComponent<PGPrimaryBehaviour>().source = source;
            vars.prefab.SetActive(true);
            vars.anim.SetTrigger("Slap");
        }
    }
}
