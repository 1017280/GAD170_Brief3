////////////////////////////////////////////////////////////////////////////////////////////////////
//  Description:
//   A thin wrapper around the Unity sound playing API
//  Purpose:
//   To make it more simple to globally play an audio clip
//  Usage:
//   Call SoundPlayer.instance.Play() and provide necessary parameters
////////////////////////////////////////////////////////////////////////////////////////////////////

#region Namespace Dependencies
using UnityEngine;
#endregion

public class SoundPlayer : MonoBehaviour
{
#region Unity Callback Functions
    void Awake()
    {
        _instance = this;
    }

    void Update()
    {
        
    }

#endregion

#region Exposed API
    public void Play(AudioSource player, AudioClip clip, float volume, bool looping = false)
    {
        player.volume = volume;
        player.clip = clip;
        player.loop = looping;
        player.Play();
    }
#endregion

#region Singleton
    public static SoundPlayer instance { get { return _instance; } }
    private static SoundPlayer _instance;
#endregion
}
