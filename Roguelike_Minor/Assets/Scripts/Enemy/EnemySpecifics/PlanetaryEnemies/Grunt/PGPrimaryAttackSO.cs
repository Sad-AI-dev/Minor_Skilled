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

        public override void Use(Ability source)
        {
            if (!source.vars.ContainsKey("poolPrimary")) InitializeVars(source);
            primary = ((BehaviourPool<PGPrimaryBehaviour>)source.vars["poolPrimary"]).GetBehaviour();
            primary.source = source;
            primary.gameObject.transform.parent = source.agent.gameObject.transform;
            primary.gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
            primary.gameObject.transform.position = source.agent.gameObject.transform.position + source.agent.gameObject.transform.forward;
        }

        void InitializeVars(Ability source)
        {
            BehaviourPool<PGPrimaryBehaviour> behaviourPool = new BehaviourPool<PGPrimaryBehaviour>();
            behaviourPool.behaviourTemplate = prefab;
            source.vars.Add("poolPrimary", behaviourPool);
        }
    }
}
