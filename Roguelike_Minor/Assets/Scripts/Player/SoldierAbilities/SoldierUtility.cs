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

        private PlayerController controller;

        public override void InitializeVars(Ability source)
        {
            controller = source.agent.GetComponent<PlayerController>();
        }

        public override void Use(Ability source)
        {
            Vector3 velocity = controller.cam.transform.forward * dashForce;
            controller.Dash(velocity);
        }

        private IEnumerator HandDashAnim(float duration)
        {
            yield return new WaitForSeconds(duration);
        }
    }
}
