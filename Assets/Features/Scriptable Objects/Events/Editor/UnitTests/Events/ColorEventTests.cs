using NUnit.Framework;
using UnityEngine;

public class ColorEventTests
{
    private ColorEvent _event;
    private Color _lastReceivedValue;
    private bool _wasListenerCalled;

    [SetUp]
    public void SetUp()
    {
        _event = ScriptableObject.CreateInstance<ColorEvent>();
        _lastReceivedValue = Color.red;
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

    private void TestListener(Color value)
    {
        _lastReceivedValue = value;
        _wasListenerCalled = true;
    }

    [Test]
    public void RegisterListener_AddsListenerToListeners()
    {
        _event.RegisterListener(TestListener);

        // Trigger the event
        _event.Raise(Color.white);

        Assert.IsTrue(_wasListenerCalled);
    }

    [Test]
    public void UnregisterListener_RemovesListenerFromListeners()
    {
        _event.RegisterListener(TestListener);
        _event.UnregisterListener(TestListener);

        // Try to trigger the event
        _event.Raise(Color.white);

        Assert.IsFalse(_wasListenerCalled);
    }

    [Test]
    public void Raise_CallsAllRegisteredListenersWithCorrectValue()
    {
        _event.RegisterListener(TestListener);

        // Define a test value
        Color testValue = Color.black;

        // Trigger the event
        _event.Raise(testValue);

        Assert.AreEqual(testValue, _lastReceivedValue);
    }
}