using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public abstract class StatusEffectBar : MonoBehaviour
    {
        [HideInInspector] public StatusEffectHandler handler;

        public abstract void HandleAddEffect(StatusEffectSO effect);
        public abstract void HandleRemoveEffect(StatusEffectSO effect);

        //stacks
        public abstract void HandleUpdateStacks(StatusEffectSO effect, int stacks);
    }
}
