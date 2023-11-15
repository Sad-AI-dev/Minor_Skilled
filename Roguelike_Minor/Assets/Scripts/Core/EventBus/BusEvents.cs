using Game.Core;

namespace Game
{
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

    //========= Shop Loaded ============
    public class ShopLoadedEvent : Event
    {
        public UnityEngine.MonoBehaviour shop;
    }

    //===== Purchaseables =====
    public class PurchaseEvent : Event
    {
        public int price;
    }

    //========== Game State =============
    public class GameEndEvent : Event { }
}
