using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.GameSystems
{
    [System.Serializable]
    public class SubEmitter
    {
        public ParticleSystem particles;
        [Range(0,1)] public float emitChance;
    }
}
