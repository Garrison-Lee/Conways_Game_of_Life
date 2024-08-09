using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseListener<T> : MonoBehaviour
{
    /// <summary>
    /// Unity Event raised when the associated scriptable event is raised
    /// </summary>
    public UnityEvent<T> OnEventRaised = new UnityEvent<T>();

    /// <summary>
    /// Event subscribed to by this listener
    /// </summary>
    [field: SerializeField]
    public BaseEvent<T> Event { get; set; } = null;

    protected virtual void OnDestroy()
    {
        Event?.UnregisterListener(OnEventRaised.Invoke);
    }

    protected virtual void Start()
    {
        Event?.RegisterListener(OnEventRaised.Invoke);
    }
}
