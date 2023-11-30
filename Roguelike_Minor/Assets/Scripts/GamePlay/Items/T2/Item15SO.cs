using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "15Airflow_Regulator3000", menuName = "ScriptableObjects/Items/T2/15: Airflow Regulator3000", order = 215)]
    public class Item15SO : ItemDataSO
    {
        public class AirflowItemVars : Item.ItemVars
        {
            public float duration;
            
        }

        [Header("Duration")]
        [SerializeField] private float baseDuration;
        [SerializeField] private float bonusDuration;

        [Header("gravity vars")]
        [SerializeField] private float gravMultiplier;

        public override void InitializeVars(Item item)
        {
            item.vars = new AirflowItemVars { duration = 0 };
        }

        public override void AddStack(Item item)
        {
            AirflowItemVars vars = item.vars as AirflowItemVars;
            if(item.stacks == 1) { vars.duration += baseDuration; }
            else { vars.duration += bonusDuration; }
        }

        public override void RemoveStack(Item item)
        {
            AirflowItemVars vars = item.vars as AirflowItemVars;
            if (item.stacks == 0) { vars.duration -= baseDuration; }
            else { vars.duration -= bonusDuration; }
        }

        public override string GenerateLongDescription()
        {
            return "The missile knows where it is at all times. It knows this because it knows where it isn't, by subtracting where it is, from where it isn't, or where it isn't, from where it is, whichever is greater, it obtains a difference, or deviation. The guidance sub-system uses deviations to generate corrective commands to drive the missile from a position where it is, to a position where it isn't, and arriving at a position where it wasn't, it now is. Consequently, the position where it is, is now the position that it wasn't, and it follows that the position where it was, is now the position that it isn't. In the event of the position that it is in is not the position that it wasn't, the system has required a variation. The variation being the difference between where the missile is, and where it wasn't. If variation is considered to be a significant factor, it too, may be corrected by the GEA. However, the missile must also know where it was. The missile guidance computance scenario works as follows: Because a variation has modified some of the information the missile has obtained, it is not sure just where it is, however it is sure where it isn't, within reason, and it knows where it was. It now subracts where it should be, from where it wasn't, or vice versa. By differentiating this from the algebraic sum og where it shouldn't be, and where it was. It is able to obtain a deviation, and a variation, which is called air";
        }
    }
}
