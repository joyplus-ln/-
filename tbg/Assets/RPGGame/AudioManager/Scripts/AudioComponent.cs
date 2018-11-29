using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioComponentSettingType
{
    MASTER,
    BGM,
    SFX,
    AMBIENT,
    OTHER
}

public class AudioComponent : MonoBehaviour
{
    public AudioComponentSettingType type;
    public string otherSettingId;

    public string SettingId
    {
        get
        {
            switch (type)
            {
                case AudioComponentSettingType.MASTER:
                    return RPGAudioManager.Singleton.masterVolumeSetting.id;
                case AudioComponentSettingType.BGM:
                    return RPGAudioManager.Singleton.bgmVolumeSetting.id;
                case AudioComponentSettingType.SFX:
                    return RPGAudioManager.Singleton.sfxVolumeSetting.id;
                case AudioComponentSettingType.AMBIENT:
                    return RPGAudioManager.Singleton.ambientVolumeSetting.id;
            }
            return otherSettingId;
        }
    }
}
