using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TestTools;

public class Vector3ListenerTests
{
    private GameObject _testGameObject;
    private Vector3Listener _listener;
    private Vector3Event _event;
    private bool _wasEventRaised;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        _testGameObject = new GameObject();
        _listener = _testGameObject.AddComponent<Vector3Listener>();
        _event = ScriptableObject.CreateInstance<Vector3Event>();

        _listener.Event = _event;
        _listener.OnEventRaised = new UnityEvent<Vector3>();
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

        _event.Raise(Vector3.one);

        Assert.IsTrue(_wasEventRaised);
    }

    [UnityTest]
    public IEnumerator Listener_UnregistersFromEventOnDestroy()
    {
        yield return null;

        GameObject.DestroyImmediate(_listener.gameObject);

        _wasEventRaised = false;
        _event.Raise(Vector3.one);

        Assert.IsFalse(_wasEventRaised);
    }
}