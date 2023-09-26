using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
    public abstract class AbilitySO : ScriptableObject
    {
        public string title;
        public float damageMultiplier;

        public abstract void Use(Ability source);
    }
}
