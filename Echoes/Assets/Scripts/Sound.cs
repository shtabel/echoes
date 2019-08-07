using UnityEngine.Audio;
using UnityEngine;

[System.Serializable ]
public class Sound
{
    // PUBLIC INIT
    public string name;         // name of the clip
    public AudioClip clip;      // reference to audio clip
    [Range(0f, 1f)] 
    public float volume;        // volume of the clip
    [Range(0.1f, 3f)]
    public float pitch;         // pitch of the clip
    public bool loop;           // is clip looping


    [HideInInspector]
    public AudioSource source;  // audio source - чтобы настравить клип?

}
