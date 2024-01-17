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

    public enum InteractFailCause { money, inventory, noRerrols }
    public class InteractFailEvent : Event
    {
        public InteractFailCause failCause;
    }

    //===== Player Leave Bounds =====
    public class RespawnEvent : Event { }
 
    //========== Game State =============
    public class GamePauseEvent : Event 
    {
        public GamePauseEvent(bool state)
        {
            paused = state;
        }
        public bool paused;
    }

    public class GameEndEvent : Event { }

}