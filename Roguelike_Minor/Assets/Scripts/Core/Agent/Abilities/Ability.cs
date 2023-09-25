using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
    [System.Serializable]
    public class Ability
    {
        [HideInInspector] public Agent agent;

        public int uses;
        public int maxUses = 1;

        [Header("Timings")]
        public float coolDown;

        //vars
        private float coolDownTimer;

        //============ Use Ability ================
        public void TryUse()
        {

        }

        private void Use()
        {

        }

        //============= Reset Ability ===============
        public void Reset()
        {
            uses = maxUses;
        }
    }
}
