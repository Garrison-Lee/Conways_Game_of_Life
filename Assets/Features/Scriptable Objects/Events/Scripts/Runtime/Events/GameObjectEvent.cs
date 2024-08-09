using UnityEngine;

[CreateAssetMenu(fileName = nameof(GameObjectEvent), menuName = ("Custom/Scriptable Objects/Events/" + nameof(GameObjectEvent)))]
public class GameObjectEvent : BaseEvent<GameObject> { }
