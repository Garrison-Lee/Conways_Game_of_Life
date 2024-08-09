using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TestTools;

public class StringListenerTests
{
    private GameObject _testGameObject;
    private StringListener _listener;
    private StringEvent _event;
    private bool _wasEventRaised;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        _testGameObject = new GameObject();
        _listener = _testGameObject.AddComponent<StringListener>();
        _event = ScriptableObject.CreateInstance<StringEvent>();

        _listener.Event = _event;
        _listener.OnEventRaised = new UnityEvent<string>();
        _listener.OnEventRaised.AddListener((value) => _wasEventRaised = true);

        _wasEventRaised = false;

        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        yield return null;

        if (_event != null)
            ScriptableObject.DestroyImmediate(_event);

        if (_testGameObject != null)
            Object.DestroyImmediate(_testGameObject);
    }

    [UnityTest]
    public IEnumerator Listener_RegistersToEventOnStart()
    {
        yield return null;

        _event.Raise("test");

        Assert.IsTrue(_wasEventRaised);
    }

    [UnityTest]
    public IEnumerator Listener_UnregistersFromEventOnDestroy()
    {
        yield return null;

        GameObject.DestroyImmediate(_listener.gameObject);

        _wasEventRaised = false;
        _event.Raise("test");

        Assert.IsFalse(_wasEventRaised);
    }
}