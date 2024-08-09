using NUnit.Framework;
using UnityEngine;

public class GameObjectEventTests
{
    private GameObjectEvent _event;
    private GameObject _lastReceivedValue;
    private bool _wasListenerCalled;

    [SetUp]
    public void SetUp()
    {
        _event = ScriptableObject.CreateInstance<GameObjectEvent>();
        _lastReceivedValue = new GameObject("New GameObject");
        _wasListenerCalled = false;
    }

    [TearDown]
    public void TearDown()
    {
        if (_event != null)
        {
            ScriptableObject.DestroyImmediate(_event);
            GameObject.DestroyImmediate(_lastReceivedValue);
            _event = null;
        }
    }

    private void TestListener(GameObject value)
    {
        _lastReceivedValue = value;
        _wasListenerCalled = true;
    }

    [Test]
    public void RegisterListener_AddsListenerToListeners()
    {
        _event.RegisterListener(TestListener);

        // Trigger the event
        _event.Raise(new GameObject("New GameObject"));

        Assert.IsTrue(_wasListenerCalled);
    }

    [Test]
    public void UnregisterListener_RemovesListenerFromListeners()
    {
        _event.RegisterListener(TestListener);
        _event.UnregisterListener(TestListener);

        // Try to trigger the event
        _event.Raise(new GameObject("New GameObject"));

        Assert.IsFalse(_wasListenerCalled);
    }

    [Test]
    public void Raise_CallsAllRegisteredListenersWithCorrectValue()
    {
        _event.RegisterListener(TestListener);

        // Define a test value
        GameObject testValue = new GameObject("New GameObject");

        // Trigger the event
        _event.Raise(testValue);

        Assert.AreEqual(testValue, _lastReceivedValue);
    }
}