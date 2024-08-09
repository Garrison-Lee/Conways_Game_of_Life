using NUnit.Framework;
using UnityEngine;

public class Vector3ValueTests
{
    private Vector3Value vector3Value;

    [SetUp]
    public void SetUp()
    {
        vector3Value = ScriptableObject.CreateInstance<Vector3Value>();
    }

    [TearDown]
    public void TearDown()
    {
        if (vector3Value != null)
            ScriptableObject.DestroyImmediate(vector3Value);
    }

    [Test]
    public void Value_AssignmentAndRetrieval_ReturnsCorrectValue()
    {
        vector3Value.Value = Vector3.one;
        Assert.AreEqual(Vector3.one, vector3Value.Value);
    }

    [Test]
    public void OnValueChanged_TriggeredOnValueChange()
    {
        bool eventFired = false;
        vector3Value.OnValueChanged += (value) => eventFired = true;

        vector3Value.Value = Vector3.one;

        Assert.IsTrue(eventFired);
    }

    [Test]
    public void OnValueChanged_NotTriggeredForSameValue()
    {
        vector3Value.Value = Vector3.one;

        bool eventFired = false;
        vector3Value.OnValueChanged += (value) => eventFired = true;

        vector3Value.Value = Vector3.one;

        Assert.IsFalse(eventFired);
    }

    [Test]
    public void SetValueWithoutNotify_DoesNotTriggerEvent()
    {
        bool eventFired = false;
        vector3Value.OnValueChanged += (value) => eventFired = true;

        vector3Value.SetValueWithoutNotify(Vector3.one);

        Assert.IsFalse(eventFired);
    }

    [Test]
    public void GetValue_GetsCorrectValue()
    {
        Vector3 value = new Vector3(Random.Range(0f, 100f), Random.Range(0f, 100f), Random.Range(0f, 100f));
        vector3Value.Value = value;

        Assert.AreEqual(value, vector3Value.GetValue());
    }
}