using NUnit.Framework;
using UnityEngine;

public class ColorValueTests
{
    private ColorValue colorValue;

    [SetUp]
    public void SetUp()
    {
        colorValue = ScriptableObject.CreateInstance<ColorValue>();
    }

    [TearDown]
    public void TearDown()
    {
        if (colorValue != null)
            ScriptableObject.DestroyImmediate(colorValue);
    }

    [Test]
    public void Value_AssignmentAndRetrieval_ReturnsCorrectValue()
    {
        colorValue.Value = Color.red;
        Assert.AreEqual(Color.red, colorValue.Value);
    }

    [Test]
    public void OnValueChanged_TriggeredOnValueChange()
    {
        bool eventFired = false;
        colorValue.OnValueChanged += (value) => eventFired = true;

        colorValue.Value = Color.red;

        Assert.IsTrue(eventFired);
    }

    [Test]
    public void OnValueChanged_NotTriggeredForSameValue()
    {
        colorValue.Value = Color.red;

        bool eventFired = false;
        colorValue.OnValueChanged += (value) => eventFired = true;

        colorValue.Value = Color.red;

        Assert.IsFalse(eventFired);
    }

    [Test]
    public void SetValueWithoutNotify_DoesNotTriggerEvent()
    {
        bool eventFired = false;
        colorValue.OnValueChanged += (value) => eventFired = true;

        colorValue.SetValueWithoutNotify(Color.red);

        Assert.IsFalse(eventFired);
    }

    [Test]
    public void GetValue_GetsCorrectValue()
    {
        Color value = Color.blue;
        colorValue.Value = value;

        Assert.AreEqual(value, colorValue.GetValue());
    }
}