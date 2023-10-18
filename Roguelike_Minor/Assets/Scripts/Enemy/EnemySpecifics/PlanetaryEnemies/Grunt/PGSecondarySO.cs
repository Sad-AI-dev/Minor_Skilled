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

        public override void Use(Ability source)
        {
            if (!source.vars.ContainsKey("poolSecondary")) InitializeVars(source);
            PGSecondaryBehaviour secondary = ((BehaviourPool<PGSecondaryBehaviour>)source.vars["poolSecondary"]).GetBehaviour();
            secondary.bulletSpeed = bulletSpeed;
            secondary.target = GameStateManager.instance.player.transform;
            secondary.gameObject.transform.position = source.agent.transform.position + source.agent.transform.forward;
            secondary.Initialize(source);
        }

        void InitializeVars(Ability source)
        {
            BehaviourPool<PGSecondaryBehaviour> behaviourPool = new BehaviourPool<PGSecondaryBehaviour>();
            behaviourPool.behaviourTemplate = prefab;
            source.vars.Add("poolSecondary", behaviourPool);
        }
    }
}
