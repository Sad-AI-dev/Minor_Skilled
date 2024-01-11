using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Enemy.Core;
using Game.Core;
using System.Threading.Tasks;

namespace Game.Enemy {
    public class Rock_BigSquid_SecondaryAttackVars : Ability.AbilityVars
    {
        public Animator anim;
    }

    [CreateAssetMenu(fileName = "Rock_BigSquid_Secondary", menuName = "ScriptableObjects/Enemy/Rock_BigSquid/SecondaryAttack")]
    public class Rock_BigSquid_SecondaryAttack : AbilitySO
    {
        public override void InitializeVars(Ability source)
        {
            source.vars = new Rock_BigSquid_SecondaryAttackVars()
            {
                anim = source.agent.GetComponent<Animator>()
            };
        }

        public override void Use(Ability source)
        {
            Rock_BigSquid_SecondaryAttackVars vars = source.vars as Rock_BigSquid_SecondaryAttackVars;

            vars.anim.SetTrigger("Melee");
        }
    }
}
