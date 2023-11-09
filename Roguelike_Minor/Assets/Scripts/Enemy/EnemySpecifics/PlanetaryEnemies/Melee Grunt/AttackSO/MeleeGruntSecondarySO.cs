using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Core.Data;
using System;

namespace Game.Enemy {
    [CreateAssetMenu(fileName = "MeleeGruntSecondary", menuName = "ScriptableObjects/Enemy/MeleeGrunt/Secondary")]
    public class MeleeGruntSecondarySO : AbilitySO
    {
        public GameObject prefab;
        public float bulletSpeed;
        public class MeleeGruntSecondaryVars : Ability.AbilityVars
        {
            public BehaviourPool<MeleeGruntSecondaryBehaviour> behaviourPool;
        }

        public override void InitializeVars(Ability source)
        {
            BehaviourPool<MeleeGruntSecondaryBehaviour> pool = new BehaviourPool<MeleeGruntSecondaryBehaviour>();
            pool.behaviourTemplate = prefab;
            source.vars = new MeleeGruntSecondaryVars { behaviourPool = pool };
        }

        public override void Use(Ability source)
        {
            MeleeGruntSecondaryBehaviour secondary = (source.vars as MeleeGruntSecondaryVars).behaviourPool.GetBehaviour();
            secondary.gameObject.transform.position = source.originPoint.position;
            secondary.target = GameStateManager.instance.player.transform.position;
            secondary.sourceTransform = source.originPoint.position;
            secondary.bulletSpeed = bulletSpeed;
            secondary.sampleTime = 0;
            secondary.Initialize(source);
        }
    }
}
