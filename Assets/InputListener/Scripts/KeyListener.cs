using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Key listener.
/// </summary>
public class KeyListener : MonoBehaviour
{
    #region VARIABLE
    List<Change> _change = new List<Change>();
    List<Trigger> _down = new List<Trigger>();
    List<Trigger> _up = new List<Trigger>();
    List<Toggle> _toggle = new List<Toggle>();

    class Change
    {
        public Change(KeyCode key, Action<bool> callback)
        {
            _key = key;
            _callback = callback;
        }
        KeyCode _key;
        Action<bool> _callback;
        public KeyCode Key { get { return _key; } }
        public Action<bool> Callback { get { return _callback; } }
    }

    class Trigger
    {
        public Trigger(KeyCode key, Action callback)
        {
            _key = key;
            _callback = callback;
        }
        KeyCode _key;
        Action _callback;
        public KeyCode Key { get { return _key; } }
        public Action Callback { get { return _callback; } }
    }

    class Toggle
    {
        public Toggle(KeyCode key, Action<bool> callback)
        {
            _key = key;
            _callback = callback;
            State = false;
        }
        KeyCode _key;
        Action<bool> _callback;
        public bool State;
        public KeyCode Key { get { return _key; } }
        public Action<bool> Callback { get { return _callback; } }
    }
    #endregion

    #region UNITY_EVENT
    void Update()
    {
        for (int i = 0; i < _change.Count; i++)
        {
            var listener = _change[i];
            if (Input.GetKeyDown(listener.Key))
            {
                listener.Callback(true);
            }
            if (Input.GetKeyUp(listener.Key))
            {
                listener.Callback(false);
            }
        }

        for (int i = 0; i < _down.Count; i++)
        {
            if (Input.GetKeyDown(_down[i].Key))
            {
                _down[i].Callback();
            }
        }

        for (int i = 0; i < _up.Count; i++)
        {
            if (Input.GetKeyUp(_up[i].Key))
            {
                _up[i].Callback();
            }
        }

        for (int i = 0; i < _toggle.Count; i++)
        {
            var toggle = _toggle[i];
            if (Input.GetKeyDown(toggle.Key))
            {
                toggle.Callback(toggle.State = !toggle.State);
            }
        }
    }
    #endregion

    #region PUBLIC_METHODS
    public void On(KeyCode key, Action<bool> callback)
    {
        _change.Add(new Change(key, callback));
    }

    public void OnDown(KeyCode key, Action callback)
    {
        _down.Add(new Trigger(key, callback));
    }

    public void OnUp(KeyCode key, Action callback)
    {
        _up.Add(new Trigger(key, callback));
    }

    public void OnTrigger(KeyCode key, Action callback)
    {
        _down.Add(new Trigger(key, callback));
    }

    public void OnToggle(KeyCode key, Action<bool> callback)
    {
        _toggle.Add(new Toggle(key, callback));
    }
    #endregion

    #region PRIVATE_METHODS
    #endregion
}
