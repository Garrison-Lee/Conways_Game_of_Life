using UnityEngine;

[CreateAssetMenu(fileName = nameof(StringValue), menuName = ("Custom/Scriptable Objects/Values/" + nameof(StringValue)))]
public class StringValue : BaseValue<string> { }
