using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
    public class StatusEffectHandler : MonoBehaviour
    {
        [HideInInspector] public Agent agent;
        public Dictionary<StatusEffectSO, int> statusEffects;

        //========== Manage Effects =========
        public void AddEffect(StatusEffectSO effect, int stacks = 1)
        {
            if (!statusEffects.ContainsKey(effect)) 
            {
                //create new effect
                statusEffects.Add(effect, 0);
                effect.AddEffect(agent);
            }
            for (int i = 0; i < stacks; i++)
            {
                //add stacks
                effect.AddStacks(agent);
                statusEffects[effect]++;
            }
        }

        public void RemoveEffect(StatusEffectSO effect, int stacks = 1)
        {

        }

        //========== Clear ==========
        public void Clear()
        {

        }
    }
}
