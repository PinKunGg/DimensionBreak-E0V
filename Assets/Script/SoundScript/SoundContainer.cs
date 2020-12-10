using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundContainer
{
    public AudioClip Fx;

    [Range(0,1)]
    public float volume;
}
