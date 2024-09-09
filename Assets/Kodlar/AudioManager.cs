using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio; 

public class audiomanager : MonoBehaviour
{
    [Header("---AUDIO SOURCE---")]
    public AudioSource SFXSource;
    [SerializeField] AudioSource musicSource;

    [Header("---AUDIO MIXER---")]
    public AudioMixer audioMixer;

    [Header("---AUDIO Clip---")]
    public AudioClip introClip, loopClip;
    public AudioClip death;
    public AudioClip attack;
    public AudioClip checkPoint;
    public AudioClip groundTouch;
    public AudioClip portal;
    public AudioClip healthPickup;
    public AudioClip laser;
    public AudioClip samdanCrack;
    public AudioClip sandikCrack;
    public AudioClip barrelCrack;
    public AudioClip skeletonDeath;
    public AudioClip pjump;
    public AudioClip coinPickup;
    public AudioClip pTakeHit;
    public AudioClip enemyTakeHit;
    public AudioClip bite;
    public AudioClip flee;
    public AudioClip Hover;
    public AudioClip Confirm;
    public AudioClip BuySell;
    public AudioClip Pause;
    public AudioClip unPause;
    public AudioClip denied;
    public AudioClip doorOpen;
 

    public static audiomanager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        musicSource.clip = introClip;
        musicSource.Play();
        musicSource.PlayScheduled(AudioSettings.dspTime + introClip.length);
        Invoke("PlayLoopMusic", introClip.length);
    }

    void Update()
    {
        // gameManager.isPaused kontrolü
        if (gameManager.isPaused)
        {
     
            audioMixer.SetFloat("MyExposedParam 1", -80f);
            audioMixer.SetFloat("MyExposedParam", -80f);
        }
        else
        {
       
            audioMixer.SetFloat("MyExposedParam 1", 0f);
            audioMixer.SetFloat("MyExposedParam", 0f);
        }
    }

    void PlayLoopMusic()
    {
        musicSource.clip = loopClip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip, float volume)
    {
        SFXSource.PlayOneShot(clip, volume);
    }
}
