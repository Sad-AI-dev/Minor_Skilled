using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Core.Data;
using System;

namespace Game.Enemy
{


    [CreateAssetMenu(fileName = "Boss_MeleeGrunt_SecondaryAttack", menuName = "ScriptableObjects/Enemy/Boss/MeleeGrunt/Secondary")]
    public class Boss_MeleeGrunt_SpecialAttackSO : AbilitySO
    {
        public GameObject spikePrefab;

        public override void InitializeVars(Ability source)
        {
            
        }

        public override void Use(Ability source)
        {
            
        }
    }
}
