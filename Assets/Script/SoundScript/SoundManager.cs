using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public SoundContainer[] SoundCon;
    Transform Player;
    public static SoundManager instance;
    AudioSource source, BGM;
    float BGMpitch = 1f;
    public List<AudioClip> sound;
    public List<float> soundVoulme;

    private void Awake()
    {
        if(instance != null)
        {

        }   
        else
        {
            instance = this;
        }

        BGM = Camera.main.GetComponent<AudioSource>();
        source = GameObject.Find("SoundFxPlay").GetComponent<AudioSource>();
        Player = GameObject.Find("Player").transform;

        foreach(var s in SoundCon)
        {
            sound.Add(s.Fx);
            soundVoulme.Add(s.volume);
        }
    }
    public void PlaySelectAudio(int value)
    {
        source.volume = soundVoulme[value];
        AudioSource.PlayClipAtPoint(sound[value],Player.position);
    }
    public void PlaySFX(int value)
    {
        source.volume = soundVoulme[value];
        source.clip = sound[value];
        source.Play();
    }
}
