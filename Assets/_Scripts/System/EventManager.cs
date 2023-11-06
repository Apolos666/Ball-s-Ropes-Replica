using System;
using System.Collections.Generic;

namespace Apolos.System.EventManager
{
    public static class EventManager
    {
        private static readonly Dictionary<string, Action> events = new();

        public static void RaiseEvent(string eventName)
        {
            if (events.TryGetValue(eventName, out Action action))
            {
                action?.Invoke();
            }
        }

        public static void AddListener(string eventName, Action action)
        {
            if (events.ContainsKey(eventName))
            {
                events[eventName] += action;
            }
            else
            {
                events.Add(eventName, action);
            }
        }

        public static void RemoveListener(string eventName, Action action)
        {
            if (events.ContainsKey(eventName))
            {
                events[eventName] -= action;
                if (events[eventName] == null)
                {
                    events.Remove(eventName);
                }
            }
        }
    }

    public static class EventManagerGeneric<TKey, TValue>
    {
        private static readonly Dictionary<string, Action<Dictionary<TKey, TValue>>> events = new();
        
        public static void RaiseEvent(string eventName, Dictionary<TKey, TValue> data)
        {
            if (events.TryGetValue(eventName, out var action))
            {
                action?.Invoke(data);
            }
        }

        public static void AddListener(string eventName, Action<Dictionary<TKey, TValue>> action)
        {
            if (events.ContainsKey(eventName))
            {
                events[eventName] += action;
            }
            else
            {
                events.Add(eventName, action);
            }
        }

        public static void RemoveListener(string eventName, Action<Dictionary<TKey, TValue>> action)
        {
            if (events.ContainsKey(eventName))
            {
                events[eventName] -= action;
                if (events[eventName] == null)
                {
                    events.Remove(eventName);
                }
            }
        }
    }
}


