using NUnit.Framework;
using UnityEngine;

public class IntValueTests
{
    private IntValue intValue;

    [SetUp]
    public void SetUp()
    {
        intValue = ScriptableObject.CreateInstance<IntValue>();
    }

    [TearDown]
    public void TearDown()
    {
        if (intValue != null)
            ScriptableObject.DestroyImmediate(intValue);
    }

    [Test]
    public void Value_AssignmentAndRetrieval_ReturnsCorrectValue()
    {
        intValue.Value = 5;
        Assert.AreEqual(5, intValue.Value);
    }

    [Test]
    public void OnValueChanged_TriggeredOnValueChange()
    {
        bool eventFired = false;
        intValue.OnValueChanged += (value) => eventFired = true;

        intValue.Value = 5;

        Assert.IsTrue(eventFired);
    }

    [Test]
    public void OnValueChanged_NotTriggeredForSameValue()
    {
        intValue.Value = 5;

        bool eventFired = false;
        intValue.OnValueChanged += (value) => eventFired = true;

        intValue.Value = 5;

        Assert.IsFalse(eventFired);
    }

    [Test]
    public void SetValueWithoutNotify_DoesNotTriggerEvent()
    {
        bool eventFired = false;
        intValue.OnValueChanged += (value) => eventFired = true;

        intValue.SetValueWithoutNotify(5);

        Assert.IsFalse(eventFired);
    }

    [Test]
    public void GetValue_GetsCorrectValue()
    {
        int value = Random.Range(0, 100);
        intValue.Value = value;

        Assert.AreEqual(value, intValue.GetValue());
    }
}