using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Simple component for setting text fields with public methods for use with UnityEvents configured in inspector
/// </summary>
public class TextFieldSetter : MonoBehaviour
{
    [Tooltip("The TextField to set")]
    public TextMeshProUGUI TextField = null;

    [Tooltip("This string will always be prepended to whatever you call Set() with")]
    public string Prefix = "";
    [Tooltip("This string will always be appended to whatever you call Set() with")]
    public string Suffix = "";

    [Tooltip("(OPTIONAL) IntVariable from which the body will be pulled whenever the value changes")]
    public IntValue IntVariable = null;

    /// <summary>
    /// Sets the body of the text with a float value
    /// </summary>
    /// <param name="value"></param>
    public void SetTextBody(float value)
    {
        if (TextField)
            TextField.text = Prefix + value.ToString() + Suffix;
    }

    private void OnIntVariableChanged(dynamic value)
    {
        if (value is int) // it is
        {
            SetTextBody(value);
        }
    }

    private void OnEnable()
    {
        if (IntVariable)
        {
            IntVariable.OnValueChanged += OnIntVariableChanged;
            SetTextBody(IntVariable.Value);
        }
    }

    private void OnDisable()
    {
        if (IntVariable)
        {
            IntVariable.OnValueChanged -= OnIntVariableChanged;
        }
    }
}
