using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player.Soldier
{
    public class SoldierUtility : AbilitySO
    {
        public override void Use(Ability source)
        {
            if(source.agent.TryGetComponent(out PlayerController playerController))
                source.vars.Add("PlayerController", playerController);
        }
    }
}
