using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEvent<T> : ScriptableObject
{
    /// <summary>
    /// A delegate type for event handling
    /// </summary>
    /// <param name="value">The value handled by the event</param>
    public delegate void EventListener(T value);

    /// <summary>
    /// Notes developers can use to summarize the purpose of the event
    /// </summary>
    [SerializeField, TextArea(3, 10)]
    protected string _notes = "";

    /// <summary>
    /// A list to hold all registered listeners
    /// </summary>
    protected List<EventListener> listeners = new List<EventListener>();

    /// <summary>
    /// Register a listener to the event
    /// </summary>
    /// <param name="listener">The listener to be registered</param>
    public virtual void RegisterListener(EventListener listener)
    {
        // Ensure the listener is not already registered
        if (!listeners.Contains(listener))
            listeners.Add(listener);
    }

    /// <summary>
    /// Unregister a listener from the event
    /// </summary>
    /// <param name="listener">The listener to be unregistered</param>
    public virtual void UnregisterListener(EventListener listener)
    {
        // Remove the listener if it is registered
        if (listeners.Contains(listener))
            listeners.Remove(listener);
    }

    /// <summary>
    /// Raise the event, notifying all listeners
    /// </summary>
    /// <param name="value">The value to raise the event with</param>
    public virtual void Raise(T value)
    {
        // Iterate through the listeners and invoke their methods
        foreach (var listener in listeners)
        {
            listener?.Invoke(value);
        }
    }
}