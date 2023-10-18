using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
    public abstract class ItemDataSO : ScriptableObject
    {
        public SlotSizeSO size;
        public bool canProcSelf;

        [Header("Visuals")]
        public GameObject pickupPrefab;
        public Sprite UISprite;

        [Header("Description")]
        public string shortDescription;

        //============ Manage Stacks ===============
        public abstract void AddStack(Item item);
        public abstract void RemoveStack(Item item);

        //============ Process hit / heal events ==============
        public abstract void ProcessDealDamage(ref HitEvent hitEvent);
        public abstract void ProcessTakeDamage(ref HitEvent hitEvent);
        public abstract void ProcessHealEvent(ref HealEvent healEvent);

        //============ Description ===========
        public abstract string GenerateLongDescription();
    }
}
