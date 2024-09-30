using System.Collections.Generic;

public class EventBus<T> where T : IEvent
{
    private static HashSet<EventBind<T>> m_bindings = new HashSet<EventBind<T>>();

    public static void Register(EventBind<T> bind) => m_bindings.Add(bind);
    public static void Unregister(EventBind<T> bind) => m_bindings.Remove(bind);
}
