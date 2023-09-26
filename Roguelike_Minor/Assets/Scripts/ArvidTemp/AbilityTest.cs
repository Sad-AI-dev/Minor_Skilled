using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;

namespace Game {
    public class AbilityTest : MonoBehaviour
    {
        private Agent agent;

        private void Start()
        {
            agent = GetComponent<Agent>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) 
            {
                agent.abilities.primary.TryUse();
            } 

        }
    }
}
