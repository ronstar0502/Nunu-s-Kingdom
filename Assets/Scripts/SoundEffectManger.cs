using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManger : MonoBehaviour
{
    AudioSource myAudio;
    AudioClip currentClip;
    private void Awake()
    {
        myAudio = GetComponent<AudioSource>();
    }
    public void PlaySFX(AudioClip sfx)
    {
        myAudio.clip = sfx;
        myAudio.Play();
    }
}
