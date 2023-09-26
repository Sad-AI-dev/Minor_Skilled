using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
    public abstract class HealthBar : MonoBehaviour
    {
        public AgentHealthManager owner;

        public abstract void UpdateHealthBar(float percentage);
    }
}
