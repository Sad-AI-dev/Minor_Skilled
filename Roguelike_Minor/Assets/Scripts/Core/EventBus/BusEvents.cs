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

    public class PlayerWarpedEvent : Event
    {
        public UnityEngine.Transform player;
        public UnityEngine.Vector3 newPlayerPos;
    }

    //========= Shop Loaded ============
    public class ShopLoadedEvent : Event
    {
        public UnityEngine.MonoBehaviour shop;
    }

    //========== Game State =============
    public class GameEndEvent : Event { }
}
