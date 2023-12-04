using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
    [CreateAssetMenu(fileName = "stats", menuName = "ScriptableObjects/Agent/Stats")]
    public class AgentStatsSO : ScriptableObject
    {
        public AgentStats baseStats;
        public ScalingStats scalingStats;

        public class ScalingStats
        {
            public float baseDamageScaling = 1;
            public float moneyScaling = 1;
            public float maxHealthScaling = 1;
        }
    }
}
