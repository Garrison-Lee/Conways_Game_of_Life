using NUnit.Framework;
using UnityEngine;

public class Vector2EventTests
{
    private Vector2Event _event;
    private Vector2 _lastReceivedValue;
    private bool _wasListenerCalled;

    [SetUp]
    public void SetUp()
    {
        _event = ScriptableObject.CreateInstance<Vector2Event>();
        _lastReceivedValue = Vector2.one;
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

    private void TestListener(Vector2 value)
    {
        _lastReceivedValue = value;
        _wasListenerCalled = true;
    }

    [Test]
    public void RegisterListener_AddsListenerToListeners()
    {
        _event.RegisterListener(TestListener);

        // Trigger the event
        _event.Raise(Vector2.one);

        Assert.IsTrue(_wasListenerCalled);
    }

    [Test]
    public void UnregisterListener_RemovesListenerFromListeners()
    {
        _event.RegisterListener(TestListener);
        _event.UnregisterListener(TestListener);

        // Try to trigger the event
        _event.Raise(Vector2.one);

        Assert.IsFalse(_wasListenerCalled);
    }

    [Test]
    public void Raise_CallsAllRegisteredListenersWithCorrectValue()
    {
        _event.RegisterListener(TestListener);

        // Define a test value
        Vector2 testValue = Vector2.one;

        // Trigger the event
        _event.Raise(testValue);

        Assert.AreEqual(testValue, _lastReceivedValue);
    }
}