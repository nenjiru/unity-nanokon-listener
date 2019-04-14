using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Demo : MonoBehaviour
{
    [SerializeField]
    NanoKontrol2 nano;
    [SerializeField]
    KeyListener key;
    [SerializeField]
    Slider slider;
    [SerializeField]
    Toggle solo;
    [SerializeField]
    Toggle mute;
    [SerializeField]
    Toggle rec;

    void Start()
    {
        // NanoKontrol2
        nano.On(NanoKey.Slider1, (v) => slider.value = v);
        nano.OnToggle(NanoKey.Solo1, (v) => solo.isOn = v);
        nano.OnTrigger(NanoKey.Mute1, () => mute.isOn = !mute.isOn);
        nano.OnHigh(NanoKey.Rec1, () => rec.isOn = true);

        // key event
        key.On(KeyCode.Alpha1, (v) => Debug.LogFormat("Numkey 1 value {0}", v));
        key.OnTrigger(KeyCode.Alpha2, () => Debug.Log("Numkey 2 Pushed"));
        key.OnDown(KeyCode.Alpha3, () => Debug.Log("Numkey 3 Pushed"));
        key.OnUp(KeyCode.Alpha3, () => Debug.Log("Numkey 3 Released"));
        key.OnToggle(KeyCode.Alpha4, (v) => Debug.LogFormat("Numkey 4 Toggle {0}", v));
    }
}