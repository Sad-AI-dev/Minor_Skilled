using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using Game.Core.GameSystems;

namespace Game
{
    public class Healer : MonoBehaviour
    {
        [Tooltip("percent of total health to be healt")]
        public float healPercent;

        public void Heal(Interactor interactor)
        {
            interactor.agent.health.HealSubtle(interactor.agent.stats.GetMaxHealth() * (healPercent / 100f));
        }
    }
}
