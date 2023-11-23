using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Core.Data;
using System;

namespace Game.Enemy
{
    [CreateAssetMenu(fileName = "PrimaryAttack", menuName = "ScriptableObjects/Enemy/PlanetGrunt/PrimaryAttack")]
    public class PGPrimaryAttackSO : AbilitySO
    {
        public GameObject prefab;
        PGPrimaryBehaviour primary;

        public class PGPrimaryAttackVars : Ability.AbilityVars
        {
            public BehaviourPool<PGPrimaryBehaviour> behaviourPool;
        }

        public override void InitializeVars(Ability source)
        {
            BehaviourPool<PGPrimaryBehaviour> pool = new BehaviourPool<PGPrimaryBehaviour>();
            pool.behaviourTemplate = prefab;
            source.vars = new PGPrimaryAttackVars { behaviourPool = pool };
        }

        public override void Use(Ability source)
        {
            primary = (source.vars as PGPrimaryAttackVars).behaviourPool.GetBehaviour();
            primary.source = source;
            primary.gameObject.transform.parent = source.agent.gameObject.transform;
            primary.gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
            primary.gameObject.transform.position = source.originPoint.position;
        }
    }
}
