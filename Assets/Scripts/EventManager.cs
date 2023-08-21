/* Adapted from:
 * https://learn.unity.com/tutorial/create-a-simple-messaging-system-with-events#5cf5960fedbc2a281acd21fa */

using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;


public class EventData
{
    public object data;

    public EventData(object data = null)
    {
        this.data = data;
    }
}

public class EventManager : MonoBehaviour
{
    private Dictionary<string, UnityEvent<EventData>> _events;

    private static EventManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
        Init();
    }

    void Init()
    {
        if (_events == null)
        {
            _events = new Dictionary<string, UnityEvent<EventData>>();
        }
    }

    public static void AddListener(string eventName, UnityAction<EventData> listener)
    {
        UnityEvent<EventData> evt = null;
        if (instance._events.TryGetValue(eventName, out evt))
        {
            evt.AddListener(listener);
        }
        else
        {
            evt = new UnityEvent<EventData>();
            evt.AddListener(listener);
            instance._events.Add(eventName, evt);
        }
    }

    public static void RemoveListener(string eventName, UnityAction<EventData> listener)
    {
        if (instance == null)
            return;
        UnityEvent<EventData> evt = null;
        if (instance._events.TryGetValue(eventName, out evt))
            evt.RemoveListener(listener);
    }

    public static void TriggerEvent(string eventName, EventData eventData)
    {
        UnityEvent<EventData> evt = null;
        if (instance._events.TryGetValue(eventName, out evt))
        {
            evt.Invoke(eventData);
        }
    }

    public static void TriggerEvent(string eventName)
    {
        TriggerEvent(eventName, new EventData());
    }
}