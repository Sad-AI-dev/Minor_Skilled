using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Core.Data;
using System;

namespace Game.Enemy {
    public class Boss_MeleeGrunt_SecondaryAttackVars : Ability.AbilityVars
    {
        public GameObject ClapHitbox;
        public Animator Anim;
    }

    [CreateAssetMenu(fileName = "Boss_MeleeGrunt_SecondaryAttack", menuName = "ScriptableObjects/Enemy/Boss/MeleeGrunt/Secondary")]
    public class Boss_MeleeGrunt_SecondaryAttackSO : AbilitySO
    {
        public float ClapChargeTime = 2;

        public override void InitializeVars(Ability source)
        {
            
        }

        public override void Use(Ability source)
        {
            Boss_MeleeGrunt_SecondaryAttackVars vars  = source.vars as Boss_MeleeGrunt_SecondaryAttackVars;
            Debug.Log("Using secondary");
            source.agent.StartCoroutine(activate(vars));
        }

        IEnumerator activate(Boss_MeleeGrunt_SecondaryAttackVars vars)
        {
            vars.Anim.SetTrigger("Secondary");
            yield return new WaitForSeconds(ClapChargeTime);
            vars.ClapHitbox.SetActive(true);
            yield return null;
            vars.ClapHitbox.SetActive(false);
        }
    }
}
