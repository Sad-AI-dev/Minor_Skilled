using Game.Core;

namespace Game
{
    //========= Shop Loaded ============
    public class ShopLoadedEvent : Event
    {
        public Shop shop;
    }

    //========= Spawn Objectives ============
    public class ObjectiveSpawned : Event
    {
        public UnityEngine.GameObject objective;
    }
}
