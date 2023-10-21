using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayMusic : MonoBehaviour
{
    [SerializeField] private string MusicTag;
    Sound sound;
    // Start is called before the first frame update
    void Start()
    {
        sound = Array.Find(AudioManager.instance.sounds, item => item.name == MusicTag);
        if (!sound.source.isPlaying)
        {
            AudioManager.instance.Play(MusicTag);
        }
    }
}
