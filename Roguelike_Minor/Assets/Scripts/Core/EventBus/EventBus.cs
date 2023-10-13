using System;

namespace Game.Core {
    public class EventBus<T> where T : Event
    {
        public static event Action<T> onInvoke;

        public static void AddListener(Action<T> listener)
        {
            onInvoke += listener;
        }

        public static void RemoveListener(Action<T> listener)
        {
            onInvoke -= listener;
        }

        public static void Invoke(T pEvent)
        {
            onInvoke?.Invoke(pEvent);
        }
    }

    //base event class
    public abstract class Event { }
}
