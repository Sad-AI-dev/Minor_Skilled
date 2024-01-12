using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player.Soldier
{
    [CreateAssetMenu(fileName = "SoldierUtility", menuName = "ScriptableObjects/Agent/Ability/Soldier/Utility")]
    public class SoldierUtility : AbilitySO
    {
        [SerializeField] private float dashForce;
        [SerializeField] private float bonusDashForce;

        private PlayerController controller;

        public class UtilityVars : Ability.AbilityVars
        {
            public float currentDashForce;
        }

        public override void InitializeVars(Ability source)
        {
            controller = source.agent.GetComponent<PlayerController>();

            source.vars = new UtilityVars
            {
                currentDashForce = dashForce
            };
        }

        public override void Use(Ability source)
        {
            Vector3 velocity = controller.cam.transform.forward * (source.vars as UtilityVars).currentDashForce;
            controller.Dash(velocity);
        }

        public override void Upgrade(Ability source)
        {
            (source.vars as UtilityVars).currentDashForce += bonusDashForce;
        }

        public override void DownGrade(Ability source)
        {
            (source.vars as UtilityVars).currentDashForce -= bonusDashForce;
        }

        private IEnumerator HandDashAnim(float duration)
        {
            yield return new WaitForSeconds(duration);
        }
    }
}
