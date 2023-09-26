using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
    public interface IHittable
    {
        public void Hurt();
        public void Heal();
    }
}
