using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour {

    public static AudioManagerScript instance = null;

    public AudioSource sfxSource;
    public AudioSource musicSource;

    public float lowestPitch = 0.95f;
    public float highestPitch = 1.05f;

    private float pitchAlt = 0.02f;
    public int numConsec = 0;

    public AudioClip[] fxClips;

    public AudioClip explosionClip;
    public AudioClip gemTwinkleClip;

    public AudioClip mainTheme;
    public AudioClip victoryTheme;

    private bool soundOn = true;


    //Refactor this to make it so that the audio plays sequentially instead of interrupting each other.

    //Potentially:
    //On each request for a clip, add it to a queue
    //while the queue has clips, play and then remove each clip.

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }    
        else if(instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

    }

    void Start()
    {
        
    }

    public void PlaySingle(AudioClip clip)
    {
            sfxSource.pitch = 1.0f;
            sfxSource.clip = clip;
            sfxSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
            //float randomPitch = Random.Range(lowestPitch, highestPitch);
            sfxSource.pitch = lowestPitch + (numConsec * pitchAlt);
            sfxSource.clip = clip;
            sfxSource.Play();
    }

    public void PlayExplosionSound()
    {
        numConsec = 0;
        PlaySFX(explosionClip);
    }

    public void PlayGemSound()
    {
        numConsec++;
        PlaySFX(gemTwinkleClip);
    }

    public void toggleSound()
    {
        Debug.Log("toggleSound");

        if (soundOn)
        {
            soundOn = false;
            sfxSource.mute = true;
            musicSource.mute = true;
        }

        else if(!soundOn)
        {
            soundOn = true;
            sfxSource.mute = false;
            musicSource.mute = false;
        }

        Debug.Log(soundOn);
    }

    public void playVictory()
    {
        musicSource.clip = victoryTheme;
        musicSource.Play();
    }

    public void playMain()
    {
        musicSource.clip = mainTheme;
        musicSource.Play();
    }
}
