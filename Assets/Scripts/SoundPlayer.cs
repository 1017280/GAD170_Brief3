using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    void Awake()
    {
        _instance = this;
    }

    void Update()
    {
        
    }

    public void Play(AudioSource player, AudioClip clip, float volume, bool looping = false)
    {
        player.volume = volume;
        player.clip = clip;
        player.loop = looping;
        player.Play();
    }

    public static SoundPlayer instance { get { return _instance; } }
    private static SoundPlayer _instance;
}
