namespace Game.Core {
    //========= Game Timer Events ========
    public class EnemyLevelupEvent : Event
    {
        public int level;
    }

    public class PriceIncreaseEvent : Event
    {
        public float baseMult;
    }

    //========= Damage / Heal Events ===========
    public class AgentTakeDamageEvent : Event
    {
        public HitEvent hitEvent;
    }

    public class AgentHealEvent : Event
    {
        public HealEvent healEvent;
    }

    //========= Scene Loaded =========
    public class SceneLoadedEvent : Event 
    {
        public int loadedIndex;
    }

    //===== Purchaseables =====
    public class PurchaseEvent : Event
    {
        public int price;
    }

    public class PickupItemEvent : Event
    {
        public ItemDataSO item;
    }

    //========== Game State =============
    public class GameEndEvent : Event { }

}