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
        PGSecondaryBehaviour secondary;
        public float bulletSpeed;

        public override void Use(Ability source)
        {
            if (!source.vars.ContainsKey("poolSecondary")) InitializeVars(source);
            secondary = ((BehaviourPool<PGSecondaryBehaviour>)source.vars["poolSecondary"]).GetBehaviour();
            secondary.source = source;
            secondary.bulletSpeed = bulletSpeed;
            secondary.gameObject.transform.localRotation = source.agent.transform.rotation;
            secondary.gameObject.transform.position = source.agent.gameObject.transform.position + source.agent.gameObject.transform.forward;
        }

        void InitializeVars(Ability source)
        {
            BehaviourPool<PGSecondaryBehaviour> behaviourPool = new BehaviourPool<PGSecondaryBehaviour>();
            behaviourPool.behaviourTemplate = prefab;
            source.vars.Add("poolSecondary", behaviourPool);
        }
    }
}
