using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
    [CreateAssetMenu(fileName = "stats", menuName = "ScriptableObjects/Agent/Stats")]
    public class AgentStatsSO : ScriptableObject
    {
        public AgentStats baseStats;
        [Space(10f)]
        public ScalingStats scalingStats;

        [System.Serializable]
        public class ScalingStats
        {
            public float baseDamageScaling = 1;
            public float moneyScaling = 1;
            public float maxHealthScaling = 1;
        }
    }
}
