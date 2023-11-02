using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core {
    public abstract class ItemDataSO : ScriptableObject
    {
        [Header("General")]
        public SlotSizeSO size;
        public ItemTierSO tier;

        [Header("Technical")]
        public bool canProcSelf;

        [Header("Visuals")]
        public GameObject pickupPrefab;
        public Sprite UISprite;

        [Header("Description")]
        public string title;
        public string shortDescription;

        [Header("Colors")]
        [SerializeField] private Color highlightColor = new Color(1f, 0.9764706f, 0.8039216f, 1);
        [SerializeField] private Color stackColor = new Color(0.8196079f, 0.8196079f, 0.8196079f, 1);
        //color refs
        protected string HighlightColor { get { return ColorUtility.ToHtmlStringRGB(highlightColor); } }
        protected string StackColor { get { return ColorUtility.ToHtmlStringRGB(stackColor); } }

        //============ Initialize Vars =============
        public abstract void InitializeVars(Item item);

        //============ Manage Stacks ===============
        public abstract void AddStack(Item item);
        public abstract void RemoveStack(Item item);

        //============ Process hit / heal events ==============
        public abstract void ProcessDealDamage(ref HitEvent hitEvent, Item sourceItem);
        public abstract void ProcessTakeDamage(ref HitEvent hitEvent, Item sourceItem);
        public abstract void ProcessHealEvent(ref HealEvent healEvent, Item sourceItem);

        //============ Description ===========
        public abstract string GenerateLongDescription();
    }
}
