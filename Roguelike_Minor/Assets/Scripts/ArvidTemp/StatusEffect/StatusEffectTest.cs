using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    public class StatusEffectTest : MonoBehaviour
    {
        public Agent target;
        public StatusEffectSO statusEffect;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                target.effectHandler.AddEffect(statusEffect);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                target.effectHandler.RemoveEffect(statusEffect);
            }
        }
    }
}
