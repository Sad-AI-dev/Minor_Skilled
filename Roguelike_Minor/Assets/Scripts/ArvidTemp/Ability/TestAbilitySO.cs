using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    [CreateAssetMenu(fileName = "TestAbility", menuName = "ScriptableObjects/Agent/Ability/TestAbility")]
    public class TestAbilitySO : AbilitySO
    {
        public override void Use(Ability source)
        {
            Debug.Log($"used {title}, it dealt {source.agent.stats.baseDamage * damageMultiplier} base damage!");
        }
    }
}
