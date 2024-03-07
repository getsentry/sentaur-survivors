/* Adapted from:
 * https://learn.unity.com/tutorial/create-a-simple-messaging-system-with-events#5cf5960fedbc2a281acd21fa */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventData
{
    public object Data;

    public EventData(object data = null)
    {
        this.Data = data;
    }
}

public class EventManager : MonoBehaviour
{
    private Dictionary<string, UnityEvent<EventData>> _events;

    private static EventManager _instance;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

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
        if (_instance._events.TryGetValue(eventName, out evt))
        {
            evt.AddListener(listener);
        }
        else
        {
            evt = new UnityEvent<EventData>();
            evt.AddListener(listener);
            _instance._events.Add(eventName, evt);
        }
    }

    public static void RemoveListener(string eventName, UnityAction<EventData> listener)
    {
        if (_instance == null)
            return;
        UnityEvent<EventData> evt = null;
        if (_instance._events.TryGetValue(eventName, out evt))
            evt.RemoveListener(listener);
    }

    public static void TriggerEvent(string eventName, EventData eventData)
    {
        UnityEvent<EventData> evt = null;
        if (_instance._events.TryGetValue(eventName, out evt))
        {
            evt.Invoke(eventData);
        }
    }

    public static void TriggerEvent(string eventName)
    {
        TriggerEvent(eventName, new EventData());
    }
}
