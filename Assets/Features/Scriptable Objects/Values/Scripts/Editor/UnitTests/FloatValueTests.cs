using NUnit.Framework;
using UnityEngine;

public class FloatValueTests
{
    private FloatValue floatValue;

    [SetUp]
    public void SetUp()
    {
        floatValue = ScriptableObject.CreateInstance<FloatValue>();
    }

    [TearDown]
    public void TearDown()
    {
        if (floatValue != null)
            ScriptableObject.DestroyImmediate(floatValue);
    }

    [Test]
    public void Value_AssignmentAndRetrieval_ReturnsCorrectValue()
    {
        floatValue.Value = 10.5f;
        Assert.AreEqual(10.5f, floatValue.Value);
    }

    [Test]
    public void OnValueChanged_TriggeredOnValueChange()
    {
        bool eventFired = false;
        floatValue.OnValueChanged += (value) => eventFired = true;

        floatValue.Value = 20.0f;

        Assert.IsTrue(eventFired);
    }

    [Test]
    public void OnValueChanged_NotTriggeredForSameValue()
    {
        floatValue.Value = 30.0f;

        bool eventFired = false;
        floatValue.OnValueChanged += (value) => eventFired = true;

        floatValue.Value = 30.0f;

        Assert.IsFalse(eventFired);
    }

    [Test]
    public void SetValueWithoutNotify_DoesNotTriggerEvent()
    {
        bool eventFired = false;
        floatValue.OnValueChanged += (value) => eventFired = true;

        floatValue.SetValueWithoutNotify(40.0f);

        Assert.IsFalse(eventFired);
    }

    [Test]
    public void GetValue_GetsCorrectValue()
    {
        float value = Random.Range(0f, 100f);
        floatValue.Value = value;

        Assert.AreEqual(value, floatValue.GetValue());
    }
}