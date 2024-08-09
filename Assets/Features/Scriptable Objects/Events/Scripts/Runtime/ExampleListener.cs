using UnityEngine;

public class ExampleListener : MonoBehaviour
{
    public VoidEvent voidEvent;

    private void OnEventRaised(object _)
    {
        Debug.Log("void event raised!");
    }

    [ContextMenu("Raise Event")]
    private void RaiseEvent()
    {
        voidEvent.Raise(null);
    }

    private void OnDestroy()
    {
        voidEvent.UnregisterListener(OnEventRaised);
    }

    private void Start()
    {
        voidEvent.RegisterListener(OnEventRaised);
    }
}
