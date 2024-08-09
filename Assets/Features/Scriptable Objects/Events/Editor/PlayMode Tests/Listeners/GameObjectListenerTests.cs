using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TestTools;

public class GameObjectListenerTests
{
    private GameObject _testGameObject;
    private GameObjectListener _listener;
    private GameObjectEvent _event;
    private bool _wasEventRaised;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        _testGameObject = new GameObject();
        _listener = _testGameObject.AddComponent<GameObjectListener>();
        _event = ScriptableObject.CreateInstance<GameObjectEvent>();

        _listener.Event = _event;
        _listener.OnEventRaised = new UnityEvent<GameObject>();
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

        GameObject go = new GameObject("New GameObject");
        _event.Raise(go);
        GameObject.DestroyImmediate(go);

        Assert.IsTrue(_wasEventRaised);
    }

    [UnityTest]
    public IEnumerator Listener_UnregistersFromEventOnDestroy()
    {
        yield return null;

        GameObject.DestroyImmediate(_listener.gameObject);

        _wasEventRaised = false;
        GameObject go = new GameObject("New GameObject");
        _event.Raise(go);
        GameObject.DestroyImmediate(go);

        Assert.IsFalse(_wasEventRaised);
    }
}