using NUnit.Framework;
using UnityEngine;

public class StringValueTests
{
    private StringValue stringValue;

    [SetUp]
    public void SetUp()
    {
        stringValue = ScriptableObject.CreateInstance<StringValue>();
    }

    [TearDown]
    public void TearDown()
    {
        if (stringValue != null)
            ScriptableObject.DestroyImmediate(stringValue);
    }

    [Test]
    public void Value_AssignmentAndRetrieval_ReturnsCorrectValue()
    {
        stringValue.Value = "test";
        Assert.AreEqual("test", stringValue.Value);
    }

    [Test]
    public void OnValueChanged_TriggeredOnValueChange()
    {
        bool eventFired = false;
        stringValue.OnValueChanged += (value) => eventFired = true;

        stringValue.Value = "test";

        Assert.IsTrue(eventFired);
    }

    [Test]
    public void OnValueChanged_NotTriggeredForSameValue()
    {
        stringValue.Value = "test";

        bool eventFired = false;
        stringValue.OnValueChanged += (value) => eventFired = true;

        stringValue.Value = "test";

        Assert.IsFalse(eventFired);
    }

    [Test]
    public void SetValueWithoutNotify_DoesNotTriggerEvent()
    {
        bool eventFired = false;
        stringValue.OnValueChanged += (value) => eventFired = true;

        stringValue.SetValueWithoutNotify("test");

        Assert.IsFalse(eventFired);
    }

    [Test]
    public void GetValue_GetsCorrectValue()
    {
        string value = "test";
        stringValue.Value = value;

        Assert.AreEqual(value, stringValue.GetValue());
    }
}