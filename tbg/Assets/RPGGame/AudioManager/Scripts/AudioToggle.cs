using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class AudioToggle : AudioComponent
{
    public Toggle toggle { get; private set; }
    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.RemoveListener(OnValueChanged);
        toggle.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(bool isOn)
    {
        RPGAudioManager.Singleton.SetVolumeIsOn(SettingId, isOn);
    }

    private void OnEnable()
    {
        toggle.isOn = RPGAudioManager.Singleton.GetVolumeIsOn(SettingId);
    }
}
