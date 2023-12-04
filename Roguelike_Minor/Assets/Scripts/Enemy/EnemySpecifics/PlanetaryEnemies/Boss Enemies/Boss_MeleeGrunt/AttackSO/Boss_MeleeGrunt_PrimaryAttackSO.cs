using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Core.Data;
using System;


namespace Game.Enemy {
    public class Boss_MeleeGrunt_PrimaryAttackVars : Ability.AbilityVars
    {
        public Animator Anim;
    }

    [CreateAssetMenu(fileName = "Boss_MeleeGrunt_PrimaryAttack", menuName = "ScriptableObjects/Enemy/Boss/MeleeGrunt/Primary")]
    public class Boss_MeleeGrunt_PrimaryAttackSO : AbilitySO
    {
        public override void InitializeVars(Ability source)
        {
            
        }

        public override void Use(Ability source)
        {
            Boss_MeleeGrunt_PrimaryAttackVars vars = source.vars as Boss_MeleeGrunt_PrimaryAttackVars;

            //Play animation
            vars.Anim.SetTrigger("Primary");
        }
    }
}
