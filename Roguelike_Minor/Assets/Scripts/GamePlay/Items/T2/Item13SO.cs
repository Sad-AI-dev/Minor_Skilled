using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "13Mirror_Of_Reflection", menuName = "ScriptableObjects/Items/T2/13: Mirror of Reflection", order = 213)]
    public class Item13SO : ItemDataSO
    {
        public class MirrorReflectionItems : Item.ItemVars
        {
            public float reflectionDamage;
        }

        [Header("Damage")]
        [SerializeField] private float damage;
        [SerializeField] private float bonusDamage;

        //Initialize vars

        public override void InitializeVars(Item item)
        {
            item.vars = new MirrorReflectionItems { reflectionDamage = damage};
        }

        //Manage stacks

        public override void AddStack(Item item)
        {
            (item.vars as MirrorReflectionItems).reflectionDamage += bonusDamage;
        }

        public override void RemoveStack(Item item)
        {
            MirrorReflectionItems vars = item.vars as MirrorReflectionItems;
            if(item.stacks == 0) { vars.reflectionDamage -= damage; }
            else { vars.reflectionDamage -= bonusDamage; }
        }

        //Process HitEvents

        public override void ProcessTakeDamage(ref HitEvent hitEvent, Item sourceItem)
        {
            DamageSource(hitEvent, sourceItem);
        }

        private void DamageSource(HitEvent hitEvent, Item item)
        {
            MirrorReflectionItems vars = item.vars as MirrorReflectionItems;

            HitEvent hitSource = new HitEvent();
            hitSource.baseDamage = vars.reflectionDamage;
            hitEvent.source.health.Hurt(hitSource);
        }

        public override string GenerateLongDescription()
        {
            return "fuck off";
        }
    }
}
