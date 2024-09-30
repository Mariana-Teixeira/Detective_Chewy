using System;

public class EventBind<T> where T : IEvent
{
    private Action<T> m_argEvent = delegate (T e) { };
    public Action<T> ArgEvent { get => m_argEvent; }

    public EventBind(Action<T> argEvent) => m_argEvent = argEvent;
}