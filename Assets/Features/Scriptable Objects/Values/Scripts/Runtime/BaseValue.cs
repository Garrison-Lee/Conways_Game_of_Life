using UnityEngine;

public abstract class BaseValue : ScriptableObject
{
    public delegate void ValueEvent(dynamic value);

    public abstract event ValueEvent OnValueChanged;

    public abstract dynamic GetValue();

    public abstract void ResetValue();
}

public abstract class BaseValue<T> : BaseValue
{
    public override event ValueEvent OnValueChanged = null;

    [SerializeField, TextArea(3, 10)]
    protected string _notes = "";
    [SerializeField]
    protected T _value;
    [SerializeField]
    protected T _defaultValue;

    public virtual T Value
    {
        get { return _value; }
        set
        {
            if(_value == null && value != null)
            {
                _value = value;
                OnValueChanged?.Invoke(_value);
                return;
            }

            if (_value.Equals(value))
                return;

            _value = value;
            OnValueChanged?.Invoke(_value);
        }
    }

    public virtual T DefaultValue
    {
        get { return _defaultValue; }
        set { _defaultValue = value; }
    }

    /// <summary>
    /// Sets the value of the variable without raising the OnValueChanged event.
    /// </summary>
    /// <param name="value">The value to set the scriptable variable to</param>
    public virtual void SetValueWithoutNotify(T value)
    {
        _value = value;
    }

    /// <summary>
    /// Returns the value held by this scriptable value
    /// </summary>
    /// <returns>The scriptable value</returns>
    public override dynamic GetValue()
    {
        return Value;
    }

    /// <summary>
    /// Resets the value to the configured default value
    /// </summary>
    public override void ResetValue()
    {
        SetValueWithoutNotify(DefaultValue);
    }
}
