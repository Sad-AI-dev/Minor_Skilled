using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
    [CreateAssetMenu(fileName = "stats", menuName = "ScriptableObjects/Agent/Stats")]
    public class AgentStatsSO : ScriptableObject
    {
        public AgentStats baseStats;
    }
}
