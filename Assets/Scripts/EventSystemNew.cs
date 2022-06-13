using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public enum Sound_Type
{
    WebImpact,
    WebTrail,

    SpiderStep
}

public enum Event_Type
{
    SPIDER_DESTROY_CAMERA,
    SPIDER_DIED,
    SPIDER_RESPAWNED,

    IS_SWINGING,
    COLLIDED,

    CAN_SWING,

    GAME_STARTED,
    GAME_WON,
    GAME_ENDED,

    UPDATE_SCORE,

    CHANGE_SPECTATOR,

    TRIGGER_SOUND
}

public static class EventSystemNew
{
    private static Dictionary<Event_Type, System.Action> eventRegister = new Dictionary<Event_Type, System.Action>();

    public static void Subscribe(Event_Type evt, System.Action func)
    {
        if (!eventRegister.ContainsKey(evt))
        {
            eventRegister.Add(evt, null);
        }

        eventRegister[evt] += func;
    }

    public static void Unsubscribe(Event_Type evt, System.Action func)
    {
        if (eventRegister.ContainsKey(evt))
        {
            eventRegister[evt] -= func;
        }
    }

    public static void RaiseEvent(Event_Type evt)
    {
        if (eventRegister.ContainsKey(evt))
        {
            eventRegister[evt]?.Invoke();
        }
        else
            Debug.Log("Event: " + evt + " doesn't Subscribe.");
    }
}

public static class EventSystemNew<A>
{
    private static Dictionary<Event_Type, System.Action<A>> eventRegister = new Dictionary<Event_Type, System.Action<A>>();

    public static void Subscribe(Event_Type evt, System.Action<A> func)
    {
        if (!eventRegister.ContainsKey(evt))
        {
            eventRegister.Add(evt, null);
        }

        eventRegister[evt] += func;
    }

    public static void Unsubscribe(Event_Type evt, System.Action<A> func)
    {
        if (eventRegister.ContainsKey(evt))
        {
            eventRegister[evt] -= func;
        }
    }

    public static void RaiseEvent(Event_Type evt, A arg)
    {
        if (eventRegister.ContainsKey(evt))
        {
            eventRegister[evt]?.Invoke(arg);
        }
        else
            Debug.Log("Event: " + evt + " doesn't Subscribe.");
    }
}

public static class EventSystemNew<A, B>
{
    private static Dictionary<Event_Type, System.Action<A, B>> eventRegister = new Dictionary<Event_Type, System.Action<A, B>>();

    public static void Subscribe(Event_Type _evt, System.Action<A, B> _func)
    {
        if (!eventRegister.ContainsKey(_evt))
        {
            eventRegister.Add(_evt, null);
        }

        eventRegister[_evt] += _func;
    }

    public static void Unsubscribe(Event_Type _evt, System.Action<A, B> _func)
    {
        if (eventRegister.ContainsKey(_evt))
        {
            eventRegister[_evt] -= _func;
        }
    }

    public static void RaiseEvent(Event_Type _evt, A _arg,  B _arg2)
    {
        if (eventRegister.ContainsKey(_evt))
        {
            eventRegister[_evt]?.Invoke(_arg, _arg2);
        }
        else
            Debug.Log("Event: " + _evt + " doesn't Subscribe.");
    }
}

public static class EventSystemNew<A, B, C>
{
    private static Dictionary<Event_Type, System.Action<A, B, C>> eventRegister = new Dictionary<Event_Type, System.Action<A, B, C>>();

    public static void Subscribe(Event_Type _evt, System.Action<A, B, C> _func)
    {
        if (!eventRegister.ContainsKey(_evt))
        {
            eventRegister.Add(_evt, null);
        }

        eventRegister[_evt] += _func;
    }

    public static void Unsubscribe(Event_Type _evt, System.Action<A, B, C> _func)
    {
        if (eventRegister.ContainsKey(_evt))
        {
            eventRegister[_evt] -= _func;
        }
    }

    public static void RaiseEvent(Event_Type _evt, A _arg, B _arg2, C _arg3)
    {
        if (eventRegister.ContainsKey(_evt))
        {
            eventRegister[_evt]?.Invoke(_arg, _arg2, _arg3);
        }
        else
            Debug.Log("Event: " + _evt + " doesn't Subscribe.");
    }
}