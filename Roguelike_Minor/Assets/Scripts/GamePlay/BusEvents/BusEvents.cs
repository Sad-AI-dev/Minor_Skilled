using Game.Core;

namespace Game
{
    //========= Scene Loaded =========
    public class SceneLoadedEvent : Event { }

    //========= Shop Loaded ============
    public class ShopLoadedEvent : Event
    {
        public Shop shop;
    }

    //========== Game State =============
    public class GameEndEvent : Event { }
}
