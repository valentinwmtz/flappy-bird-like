using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static  class SoundManager
{
    public enum SoundEnum
    {
        BirdDie,
        BirdJump,
        BirdScore
    }
    public static void PlaySound(SoundEnum sound)
    {
        GameObject gameObject = new GameObject("Sound", typeof(AudioSource));
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(GetAudioClip(sound));
    }

    private static AudioClip GetAudioClip(SoundEnum sound)
    {
        foreach (GameAssets.SoundAudioClip soundAudioClip in GameAssets.GetInstance().soundAudioClips)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }

        return null;
    }
}
