using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
    public static class AgentRandom
    {
        //============ Try Proc =============
        public static bool TryProc(float chance, HitEvent hitEvent)
        {
            bool result = CalcTotalChance(hitEvent) <= chance;
            if (hitEvent.hasAgentSource)
            {
                int luckPoints = hitEvent.source.stats.luck;
                while (!result && luckPoints > 0)
                {
                    result = CalcTotalChance(hitEvent) <= chance;
                    luckPoints--;
                }
            }
            return result;
        }

        public static bool TryProc(float chance, Agent agent)
        {
            bool result = Random.Range(0f, 100f) > chance;
            int luckPoints = agent.stats.luck;
            while (!result && luckPoints > 0)
            {
                result = Random.Range(0f, 100f) > chance;
                luckPoints--;
            }
            return result;
        }

        //========= Chance Calc ==============
        private static float CalcTotalChance(HitEvent hitEvent)
        {
            return Random.Range(0f, 100f) * hitEvent.procCoef;
        }
    }
}
