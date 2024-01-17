using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Core.Data;
using System;

namespace Game.Enemy
{
    public class Boss_MeleeGrunt_SpecialAttackVars : Ability.AbilityVars
    {
        public Animator Anim;
    }

    [CreateAssetMenu(fileName = "Boss_MeleeGrunt_SecondaryAttack", menuName = "ScriptableObjects/Enemy/Boss/MeleeGrunt/Secondary")]
    public class Boss_MeleeGrunt_SpecialAttackSO : AbilitySO
    {
        public GameObject spikePrefab;

        public override void InitializeVars(Ability source)
        {
            source.vars = new Boss_MeleeGrunt_SpecialAttackVars()
            {
                Anim = source.agent.transform.GetComponent<Animator>()
            };
        }

        public override void Use(Ability source)
        {
            Boss_MeleeGrunt_SpecialAttackVars vars = source.vars as Boss_MeleeGrunt_SpecialAttackVars;

            vars.Anim.SetTrigger("Special");
        }
    }
}
