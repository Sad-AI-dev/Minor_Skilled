using Game.Core;
using Game.Core.GameSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "19Radiating_Ration", menuName = "ScriptableObjects/Items/T2/19: Radiating Ration", order = 219)]
    public class Item19SO : ItemDataSO
    {
        public class RadiatingItemVars : Item.ItemVars
        {
            public float radius;
            public Coroutine damageCoroutine;
        }

        [Header("Radius")]
        [SerializeField] private float baseRadius;
        [SerializeField] private float bonusRadius;

        [Header("Damage Vars")]
        [SerializeField] private float cooldown;
        [SerializeField] private int damage;

        public override void InitializeVars(Item item)
        {
            item.vars = new RadiatingItemVars
            {
                radius = 0,
                damageCoroutine = null
            };
        }

        public override void AddStack(Item item)
        {
            RadiatingItemVars vars = item.vars as RadiatingItemVars;

            if(item.stacks == 1) 
            { 
                vars.radius += baseRadius;
                vars.damageCoroutine = item.agent.StartCoroutine(DealDamageCo(item));
            }
            else { vars.radius += bonusRadius; }
        }

        public override void RemoveStack(Item item)
        {
            RadiatingItemVars vars = item.vars as RadiatingItemVars;
            if (item.stacks == 0) 
            { 
                vars.radius -= baseRadius;

                if(vars.damageCoroutine != null)
                    item.agent.StopCoroutine(vars.damageCoroutine);
            }
            else { vars.radius -= bonusRadius; }
        }

        private IEnumerator DealDamageCo(Item item)
        {
            yield return new WaitForSeconds(cooldown);
            RadiatingItemVars vars = item.vars as RadiatingItemVars;
            List<Agent> agents = Explosion.FindAgentsInRange(item.agent.transform.position, vars.radius, item.agent);
            Explosion.DealDamage(agents, item.agent, damage, 0);
            vars.damageCoroutine = item.agent.StartCoroutine(DealDamageCo(item));
        }

        public override string GenerateLongDescription()
        {
            return $"Deal <color=#{HighlightColor}>{damage}</color> damage to enemies within a " +
                   $"<color=#{HighlightColor}>{baseRadius}m</color> <color=#{StackColor}>(+{bonusRadius}m per stack)</color> " +
                   $"radius around you.";
        }
    }
}
