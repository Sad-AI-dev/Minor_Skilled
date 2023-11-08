using UnityEngine;

namespace Game.Core {
    public static class AgentRandom
    {
        //============ Try Proc =============
        private static bool TryProc(float chance, HitEvent hitEvent)
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

        private static bool TryProc(float chance, Agent agent)
        {
            bool result = Random.Range(0f, 100f) <= chance;
            int luckPoints = agent.stats.luck;
            while (!result && luckPoints > 0)
            {
                result = Random.Range(0f, 100f) <= chance;
                luckPoints--;
            }
            return result;
        }

        //========= Multi Proc ============
        public static void TryProc<T>(float chance, Agent agent, System.Action<T> onProc, T vars = default)
        {
            //guarenteed procs
            for (int i = 0; i < Mathf.FloorToInt(chance / 100f); i++) { onProc?.Invoke(vars); }
            //try proc for left chance
            if (TryProc(chance % 100, agent)) { onProc?.Invoke(vars); }
        }
        public static void TryProc(float chance, Agent agent, System.Action onProc)
        {
            TryProc(chance, agent, (int i) => onProc?.Invoke());
        }

        public static void TryProc<T>(float chance, HitEvent hitEvent, System.Action<T> onProc, T vars = default)
        {
            //guarenteed procs
            float guarenteedChance = 100f * hitEvent.procCoef; //100% is not always guarenteed due to proccoef
            for (int i = 0; i < Mathf.FloorToInt(chance / guarenteedChance); i++) { onProc?.Invoke(vars); }
            //try proc for left chance
            if (TryProc(chance % guarenteedChance, hitEvent)) { onProc?.Invoke(vars); }
        }
        public static void TryProc(float chance, HitEvent hitEvent, System.Action onProc)
        {
            TryProc(chance, hitEvent, (int i) => onProc?.Invoke());
        }


        //========= Chance Calc ==============
        private static float CalcTotalChance(HitEvent hitEvent)
        {
            return Random.Range(0f, 100f) * hitEvent.procCoef;
        }
    }
}
