using NUnit.Framework;
using UnityEngine;

public class Vector2ValueTests
{
    private Vector2Value vector2Value;

    [SetUp]
    public void SetUp()
    {
        vector2Value = ScriptableObject.CreateInstance<Vector2Value>();
    }

    [TearDown]
    public void TearDown()
    {
        if (vector2Value != null)
            ScriptableObject.DestroyImmediate(vector2Value);
    }

    [Test]
    public void Value_AssignmentAndRetrieval_ReturnsCorrectValue()
    {
        vector2Value.Value = Vector2.one;
        Assert.AreEqual(Vector2.one, vector2Value.Value);
    }

    [Test]
    public void OnValueChanged_TriggeredOnValueChange()
    {
        bool eventFired = false;
        vector2Value.OnValueChanged += (value) => eventFired = true;

        vector2Value.Value = Vector2.one;

        Assert.IsTrue(eventFired);
    }

    [Test]
    public void OnValueChanged_NotTriggeredForSameValue()
    {
        vector2Value.Value = Vector2.one;

        bool eventFired = false;
        vector2Value.OnValueChanged += (value) => eventFired = true;

        vector2Value.Value = Vector2.one;

        Assert.IsFalse(eventFired);
    }

    [Test]
    public void SetValueWithoutNotify_DoesNotTriggerEvent()
    {
        bool eventFired = false;
        vector2Value.OnValueChanged += (value) => eventFired = true;

        vector2Value.SetValueWithoutNotify(Vector2.one);

        Assert.IsFalse(eventFired);
    }

    [Test]
    public void GetValue_GetsCorrectValue()
    {
        Vector2 value = new Vector2(Random.Range(0f, 100f), Random.Range(0f, 100f));
        vector2Value.Value = value;

        Assert.AreEqual(value, vector2Value.GetValue());
    }
}