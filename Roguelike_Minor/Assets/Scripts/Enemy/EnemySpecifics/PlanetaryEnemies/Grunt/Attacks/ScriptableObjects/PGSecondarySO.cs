using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Core.Data;
using System;

namespace Game.Enemy
{
    [CreateAssetMenu(fileName = "SecondaryAttack", menuName = "ScriptableObjects/Enemy/PlanetGrunt/SecondaryAttack")]
    public class PGSecondaryAttackSO : AbilitySO
    {
        public GameObject prefab;
        public float bulletSpeed;
        public class PGSecondaryAttackVars : Ability.AbilityVars
        {
            public BehaviourPool<PGSecondaryBehaviour> behaviourPool;
        }

        public override void InitializeVars(Ability source)
        {
            BehaviourPool<PGSecondaryBehaviour> pool = new BehaviourPool<PGSecondaryBehaviour>();
            pool.behaviourTemplate = prefab;
            source.vars = new PGSecondaryAttackVars { behaviourPool = pool };
        }

        public override void Use(Ability source)
        {
            PGSecondaryBehaviour secondary = (source.vars as PGSecondaryAttackVars).behaviourPool.GetBehaviour();
            secondary.bulletSpeed = bulletSpeed;
            secondary.target = GameStateManager.instance.player.transform;
            secondary.gameObject.transform.position = source.agent.transform.position + source.agent.transform.forward;
            secondary.Initialize(source);
        }
    }
}
