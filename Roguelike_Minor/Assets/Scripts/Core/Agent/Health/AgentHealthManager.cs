using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Core {
    public class AgentHealthManager : MonoBehaviour, IHittable
    {
        [HideInInspector] public Agent agent;

        [Header("Health")]
        public float health;
        public HealthBar healthBar;

        [Header("Events")]
        public UnityEvent onHeal;
        public UnityEvent onHurt;
        public UnityEvent onDeath;

        //============ IHittable ===============
        public void Hurt()
        {

        }

        public void Heal()
        {

        }
    }
}
