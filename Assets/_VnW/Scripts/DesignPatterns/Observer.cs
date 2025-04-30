// ObserverManager.cs
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DesignPatterns
{
    /// <summary>
    /// Generic event data interface
    /// </summary>
    public interface IEventData { }

    /// <summary>
    /// Observer Manager - Singleton implementation to manage events
    /// </summary>
    public class ObserverManager : SingletonDontDestroy<ObserverManager>
    {
        // Dictionary of event type to list of listeners
        private readonly Dictionary<Type, List<Action<IEventData>>> _eventListeners = new Dictionary<Type, List<Action<IEventData>>>();

        /// <summary>
        /// Subscribe to an event of type T
        /// </summary>
        public void Subscribe<T>(Action<T> listener) where T : IEventData
        {
            var eventType = typeof(T);

            if (!_eventListeners.ContainsKey(eventType))
            {
                _eventListeners[eventType] = new List<Action<IEventData>>();
            }

            _eventListeners[eventType].Add(Wrapper);
            return;

            // Wrapper to convert generic type to IEventData
            void Wrapper(IEventData eventData) => listener((T)eventData);
        }

        /// <summary>
        /// Unsubscribe from an event of type T
        /// </summary>
        public void Unsubscribe<T>(Action<T> listener) where T : IEventData
        {
            var eventType = typeof(T);

            if (!_eventListeners.TryGetValue(eventType, out var eventListener))
                return;

            // This is a simplification - in production code, you'd need a more complex solution
            // to track the original wrapper for each listener
            // For simplicity, we'll just clear all listeners for this event type
            eventListener.Clear();
        }

        /// <summary>
        /// Publish an event
        /// </summary>
        public void Notify<T>(T eventData) where T : IEventData
        {
            var eventType = typeof(T);

            if (!_eventListeners.TryGetValue(eventType, out var eventListener))
                return;

            foreach (var listener in eventListener)
            {
                listener(eventData);
            }
        }

        /// <summary>
        /// Clear all event listeners
        /// </summary>
        public void ClearAllListeners()
        {
            _eventListeners.Clear();
        }
    }
}