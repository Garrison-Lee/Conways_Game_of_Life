using NUnit.Framework;
using UnityEngine;

public class FloatEventTests
{
    private FloatEvent _event;
    private float _lastReceivedValue;
    private bool _wasListenerCalled;

    [SetUp]
    public void SetUp()
    {
        _event = ScriptableObject.CreateInstance<FloatEvent>();
        _lastReceivedValue = 0f;
        _wasListenerCalled = false;
    }

    [TearDown]
    public void TearDown()
    {
        if (_event != null)
        {
            ScriptableObject.DestroyImmediate(_event);
            _event = null;
        }
    }

    private void TestListener(float value)
    {
        _lastReceivedValue = value;
        _wasListenerCalled = true;
    }

    [Test]
    public void RegisterListener_AddsListenerToListeners()
    {
        _event.RegisterListener(TestListener);

        // Trigger the event
        _event.Raise(10f);

        Assert.IsTrue(_wasListenerCalled);
    }

    [Test]
    public void UnregisterListener_RemovesListenerFromListeners()
    {
        _event.RegisterListener(TestListener);
        _event.UnregisterListener(TestListener);

        // Try to trigger the event
        _event.Raise(10f);

        Assert.IsFalse(_wasListenerCalled);
    }

    [Test]
    public void Raise_CallsAllRegisteredListenersWithCorrectValue()
    {
        _event.RegisterListener(TestListener);

        // Define a test value
        float testValue = 5.5f;

        // Trigger the event
        _event.Raise(testValue);

        Assert.AreEqual(testValue, _lastReceivedValue);
    }
}