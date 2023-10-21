using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlaySoundWhenHitTag : MonoBehaviour
{
    [SerializeField] string objectToHitTag;
    [SerializeField] string soundToPlayTag;
    Sound soundToPlay;
    // Start is called before the first frame update

    private void Start()
    {
        soundToPlay = Array.Find(AudioManager.instance.sounds, item => item.name == soundToPlayTag);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(objectToHitTag))
        {
            if (!soundToPlay.source.isPlaying)
            {
                AudioManager.instance.Play(soundToPlayTag);
            }
        }
    }
}
