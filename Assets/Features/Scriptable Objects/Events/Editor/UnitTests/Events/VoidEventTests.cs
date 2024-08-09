using NUnit.Framework;
using UnityEngine;

public class VoidEventTests
{
    private VoidEvent _event;
    private bool _wasListenerCalled;

    [SetUp]
    public void SetUp()
    {
        _event = ScriptableObject.CreateInstance<VoidEvent>();
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

    private void TestListener(object _)
    {
        _wasListenerCalled = true;
    }

    [Test]
    public void RegisterListener_AddsListenerToListeners()
    {
        _event.RegisterListener(TestListener);

        // Trigger the event
        _event.Raise(null);

        Assert.IsTrue(_wasListenerCalled);
    }

    [Test]
    public void UnregisterListener_RemovesListenerFromListeners()
    {
        _event.RegisterListener(TestListener);
        _event.UnregisterListener(TestListener);

        // Try to trigger the event
        _event.Raise(null);

        Assert.IsFalse(_wasListenerCalled);
    }
}