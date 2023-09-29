using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
    public abstract class AbilitySO : ScriptableObject
    {
        public string title;
        //procCoef = Proc Coefficient, this is a multiplier applied to the likelyhood of a hitEvent proccing items.
        public float procCoef = 1f;
        public float damageMultiplier;

        public abstract void Use(Ability source);
    }
}
