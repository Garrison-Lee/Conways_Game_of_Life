using UnityEngine;

[CreateAssetMenu(fileName = nameof(StringEvent), menuName = ("Custom/Scriptable Objects/Events/" + nameof(StringEvent)))]
public class StringEvent : BaseEvent<string> { }
