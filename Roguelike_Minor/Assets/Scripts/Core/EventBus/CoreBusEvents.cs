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
    public class SceneLoadedEvent : Event { }

    public class StageLoadedEvent: Event { }

    //===== Purchaseables =====
    public class PurchaseEvent : Event
    {
        public int price;
    }

    //========== Game State =============
    public class GameEndEvent : Event { }

}