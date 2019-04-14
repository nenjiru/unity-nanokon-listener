using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// nanoKONTROL2 key mapping.
/// </summary>
public enum NanoKey
{
    // control
    Slider1 = 0,
    Slider2 = 1,
    Slider3 = 2,
    Slider4 = 3,
    Slider5 = 4,
    Slider6 = 5,
    Slider7 = 6,
    Slider8 = 7,
    Knob1 = 16,
    Knob2 = 17,
    Knob3 = 18,
    Knob4 = 19,
    Knob5 = 20,
    Knob6 = 21,
    Knob7 = 22,
    Knob8 = 23,
    // button
    Solo1 = 32,
    Solo2 = 33,
    Solo3 = 34,
    Solo4 = 35,
    Solo5 = 36,
    Solo6 = 37,
    Solo7 = 38,
    Solo8 = 39,
    Mute1 = 48,
    Mute2 = 49,
    Mute3 = 50,
    Mute4 = 51,
    Mute5 = 52,
    Mute6 = 53,
    Mute7 = 54,
    Mute8 = 55,
    Rec1 = 64,
    Rec2 = 65,
    Rec3 = 66,
    Rec4 = 67,
    Rec5 = 68,
    Rec6 = 69,
    Rec7 = 70,
    Rec8 = 71,
    // function
    TrackPrev = 58,
    TrackNext = 59,
    Cycle = 46,
    MarkerSet = 60,
    MarkerPrev = 61,
    MarkerNext = 62,
    // transport
    Prev = 43,
    Next = 44,
    Stop = 42,
    Play = 41,
    Rec = 45
}

/// <summary>
/// nanoKONTROL2 listener.
/// </summary>
public class NanoKontrol2 : MonoBehaviour
{
    #region VARIABLE
    List<Change> _change = new List<Change>();
    List<Trigger> _low = new List<Trigger>();
    List<Trigger> _high = new List<Trigger>();
    List<Toggle> _toggle = new List<Toggle>();

    class Change
    {
        public Change(NanoKey key, Action<float> callback)
        {
            _key = key;
            _callback = callback;
        }
        NanoKey _key;
        Action<float> _callback;
        public NanoKey Key { get { return _key; } }
        public Action<float> Callback { get { return _callback; } }
    }

    class Trigger
    {
        public Trigger(NanoKey key, Action callback)
        {
            _key = key;
            _callback = callback;
        }
        NanoKey _key;
        Action _callback;
        public NanoKey Key { get { return _key; } }
        public Action Callback { get { return _callback; } }
    }

    class Toggle
    {
        public Toggle(NanoKey key, Action<bool> callback)
        {
            _key = key;
            _callback = callback;
            State = false;
        }
        NanoKey _key;
        Action<bool> _callback;
        public bool State;
        public NanoKey Key { get { return _key; } }
        public Action<bool> Callback { get { return _callback; } }
    }
    #endregion

    #region UNITY_EVENT
    void Update()
    {
        while (true)
        {
            var data = UnityMidiReceiver.DequeueIncomingData();
            if (data == 0)
            {
                break;
            }

            var message = new MidiMessage(data);
            _dispatcher((int)message.data1, (float)message.data2 / 127f);
        }
    }
    #endregion

    #region PUBLIC_METHODS
    public void On(NanoKey key, Action<float> callback)
    {
        _change.Add(new Change(key, callback));
    }

    public void OnLow(NanoKey key, Action callback)
    {
        _low.Add(new Trigger(key, callback));
    }

    public void OnHigh(NanoKey key, Action callback)
    {
        _high.Add(new Trigger(key, callback));
    }

    public void OnTrigger(NanoKey key, Action callback)
    {
        _high.Add(new Trigger(key, callback));
    }

    public void OnToggle(NanoKey key, Action<bool> callback)
    {
        _toggle.Add(new Toggle(key, callback));
    }
    #endregion

    #region PRIVATE_METHODS
    void _dispatcher(int id, float value)
    {
        for (int i = 0; i < _change.Count; i++)
        {
            if (id == (int)_change[i].Key)
            {
                _change[i].Callback(value);
            }
        }

        for (int i = 0; i < _low.Count; i++)
        {
            if (id == (int)_low[i].Key && value == 0f)
            {
                _low[i].Callback();
            }
        }

        for (int i = 0; i < _high.Count; i++)
        {
            if (id == (int)_high[i].Key && value == 1f)
            {
                _high[i].Callback();
            }
        }

        for (int i = 0; i < _toggle.Count; i++)
        {
            if (id == (int)_toggle[i].Key)
            {
                if (value == 1f)
                {
                    _toggle[i].Callback(true);
                }
                if (value == 0f)
                {
                    _toggle[i].Callback(false);
                }
            }
        }
    }
    #endregion
}