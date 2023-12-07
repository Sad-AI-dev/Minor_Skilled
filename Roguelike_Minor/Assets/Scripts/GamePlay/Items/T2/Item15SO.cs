using Game.Core;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "15Airflow_Regulator3000", menuName = "ScriptableObjects/Items/T2/15: Airflow Regulator3000", order = 215)]
    public class Item15SO : ItemDataSO
    {
        public override void AddStack(Item item)
        {
            item.agent.stats.totalJumps++;
            item.agent.stats.currentJumps = item.agent.stats.totalJumps;
        }

        public override void RemoveStack(Item item)
        {
            if(item.agent.stats.totalJumps > 1)
                item.agent.stats.totalJumps--;
            item.agent.stats.currentJumps = item.agent.stats.totalJumps;
        }

        public override string GenerateLongDescription()
        {
            return $"Gain <color=#{HighlightColor}>+1</color> <color=#{StackColor}>(+1 per stack)</color> extra jumps.";
        }
    }
}
