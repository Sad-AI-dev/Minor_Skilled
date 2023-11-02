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

        public override void Use(Ability source)
        {
            if(!source.vars.ContainsKey("PlayerController"))
            {
                if(source.agent.TryGetComponent(out PlayerController playerController))
                    source.vars.Add("PlayerController", playerController);
            }

            PlayerController controller = (PlayerController)source.vars["PlayerController"];

            Vector3 velocity = controller.cam.transform.forward * dashForce;
            controller.ReceiveKnockback(velocity);
        }
    }
}
