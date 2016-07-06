using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System;

[Serializable]
public class Audio
{
    #region Fields

    public AudioClip Clip;
    public AudioMixerGroup Output;
    [Range(0, 1)]
    public float MinVolume = 1.0f;
    [Range(0, 1)]
    public float MaxVolume = 1.0f;
    [Range(0, 3)]
    public float MinPitch = 1.0f;
    [Range(0, 3)]
    public float MaxPitch = 1.0f;
    public bool Loop;
    public bool Mute;

    #endregion

    #region Properties	
    #endregion

    #region Methods

    public bool IsNull()
    {
        return Clip == null;
    }

    #endregion
}