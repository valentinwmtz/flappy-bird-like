using System;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets instance;

    public static GameAssets GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }

    public Sprite pipeSprite;
    public Sprite[] birdSprites;
    public Transform pipePrefab;
    public SoundAudioClip[] soundAudioClips;


    [Serializable]
    public class SoundAudioClip
    {
        public SoundManager.SoundEnum sound;
        public AudioClip audioClip;
    }

    [Serializable]
    class SpriteArrays : ScriptableObject
    {
        public Sprite[] Sprites;
    }
}
