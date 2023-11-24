using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Enemy.Core;
using Game.Core;

namespace Game.Enemy {
    [CreateAssetMenu(fileName = "Boss_BigSquid_Primary", menuName = "ScriptableObjects/Enemy/Boss/BigSquid/PrimaryAttack")]
    public class Boss_BigSquid_PrimarySO : AbilitySO
    {
        public GameObject goopPrefab;

        public override void InitializeVars(Ability source)
        {
            
        }

        public override void Use(Ability source)
        {
            GameObject goop = Instantiate(goopPrefab, source.agent.transform.position, Quaternion.identity);
            goop.GetComponent<Boss_BigSquid_PrimaryBehaviour>().source = source.agent;
        }
    }
}
