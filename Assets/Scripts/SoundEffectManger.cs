using UnityEngine;

public class SoundEffectManger : MonoBehaviour
{
    private AudioSource _myAudio;
    private AudioClip _currentClip;
    private void Awake()
    {
        _myAudio = GetComponent<AudioSource>();
    }
    public void PlaySFX(AudioClip sfx)
    {
        _myAudio.clip = sfx;
        _myAudio.Play();
    }
}
