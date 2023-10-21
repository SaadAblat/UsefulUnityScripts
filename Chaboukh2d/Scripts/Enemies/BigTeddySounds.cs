using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigTeddySounds : MonoBehaviour
{
    float timeBeforePlayingAudio;
    float timer;
    [SerializeField] BigTeddy bt;

    // Start is called before the first frame update
    void Start()
    {
        timeBeforePlayingAudio = Random.Range(1, 10);
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!bt.bigTeddyIsDead)
        {
            timer += Time.deltaTime;
            if (timer >= timeBeforePlayingAudio)
            {
                AudioManager.instance.Play("Breathing1");
                timer = 0;
                timeBeforePlayingAudio = Random.Range(1, 30);
            } 
        }
    }


    public void PlayBGWalkSound()
    {
        AudioManager.instance.Play("BigTeddyWalk");
    }
    public void PlayKillingSounds()
    {
        AudioManager.instance.Play("KillingPlayer");
    }

    public void PlayCatchedThePlayerSounds()
    {
        AudioManager.instance.Play("CatchedThePlayer");
    }
    public void PlayBigTeddyDeath()
    {
        AudioManager.instance.Play("BigTeddyDeath");
    }

    private void OnDestroy()
    {
        if (gameObject != null && AudioManager.instance != null)
        {
            AudioManager.instance.Stop("KillingPlayer");
            AudioManager.instance.Stop("Breathing1");
        }

    }
}
