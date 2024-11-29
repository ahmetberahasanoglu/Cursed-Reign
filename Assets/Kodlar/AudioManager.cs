using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class audiomanager : MonoBehaviour
{
    [Header("---AUDIO SOURCE---")]
    public AudioSource SFXSource;
    [SerializeField] AudioSource musicSource;

    [Header("---AUDIO MIXER---")]
    public AudioMixer audioMixer;

    [Header("---AUDIO Clip---")]
    // public AudioClip introClip;
    public AudioClip defaultLoopClip;  
    public AudioClip doorSceneClip;    
    public AudioClip firstSceneClip;    
    public AudioClip battleSceneClip;  
    public AudioClip level2SceneClip;  
    public AudioClip level3SceneClip;  
    public AudioClip level4SceneClip;  
    public AudioClip bossSceneClip;  
    public AudioClip shopSceneClip;    
    public AudioClip menuSceneClip;
    public AudioClip death;
    public AudioClip attack;
    public AudioClip checkPoint;
    public AudioClip groundTouch;
    public AudioClip portal;
    public AudioClip healthPickup;
    public AudioClip manaPickup;
    public AudioClip laser;
    public AudioClip samdanCrack;
    public AudioClip sandikCrack;
    public AudioClip barrelCrack;
    public AudioClip skeletonDeath;
    public AudioClip skeletonWalk;
    public AudioClip bounceSound;
    public AudioClip pjump;
    public AudioClip coinPickup;
    public AudioClip pTakeHit;
    public AudioClip enemyTakeHit;
    public AudioClip bite;
    public AudioClip flee;
    public AudioClip Confirm;
    public AudioClip BuySell;
    public AudioClip Pause;
    public AudioClip unPause;
    public AudioClip denied;
    public AudioClip shopOpen;
    public AudioClip doorOpen;
    public AudioClip timesUP;

    public static audiomanager Instance { get; private set; }

    [Header("Pause Menu Buttonlari")]
    public Button musicToggleButton;
    public Button SFXToggleButton;

    private bool isMusicMuted = false;  
    private bool isSFXMuted = false;    

    private void Awake()
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

    private void Start()
    {
        PlayMusicForCurrentScene();

        // Butonlara i�levleri atama
        if (musicToggleButton != null)
        {
            musicToggleButton.onClick.AddListener(ToggleMusic);
            SFXToggleButton.onClick.AddListener(ToggleSFX);
        }
        else
        {
            Debug.Log("sfx ve music baglanmadi");
        }

    }

    private void Update()
    {
        // gameManager.isPaused kontrol�
        if (gameManager.isPaused)
        {
            audioMixer.SetFloat("MyExposedParam 1", -80f);
            audioMixer.SetFloat("MyExposedParam", -80f);
        }
        else
        {
            if (!isMusicMuted)
                audioMixer.SetFloat("MyExposedParam 1", -18f); 
            if (!isSFXMuted)
                audioMixer.SetFloat("MyExposedParam", 0f);     
        }
    }
    private void PlayMusicForCurrentScene()
    {
        AudioClip selectedClip = defaultLoopClip;  // Varsay�lan m�zik

        // Sahneye g�re m�zik se�imi
        if (SceneManager.GetActiveScene().name == "Door1")
        {
            selectedClip = doorSceneClip;
        }
        else if (SceneManager.GetActiveScene().name == "FirstScene")
        {
           selectedClip = firstSceneClip;
        }
        else if (SceneManager.GetActiveScene().name == "level1")
        {
            selectedClip = battleSceneClip;
        }
        else if (SceneManager.GetActiveScene().name == "level2")
        {
            selectedClip = level2SceneClip;
        }
        else if (SceneManager.GetActiveScene().name == "level3")
        {
            selectedClip = level3SceneClip;
        }
        else if (SceneManager.GetActiveScene().name == "level4")
        {
            selectedClip = level4SceneClip;
        }
        else if (SceneManager.GetActiveScene().name == "level5")
        {
            selectedClip = bossSceneClip;
        }
        else if (SceneManager.GetActiveScene().name == "Door0")
        {
            selectedClip = shopSceneClip;
        }
        else if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            selectedClip = menuSceneClip;
        }
        // Ba�ka sahneler i�in ekle...

        PlayMusic(selectedClip);
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;  // Sahne y�klendi�inde �a��r�l�r
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;  // Event kald�r�l�r
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForCurrentScene();  // Yeni sahne y�klendi�inde m�zi�i g�ncelle
    }
    private void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip != clip)
        {
            musicSource.Stop();  
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();  
        }
    }
    public void ToggleMusic()
    {
        isMusicMuted = !isMusicMuted;

        if (isMusicMuted)
        {
            audioMixer.SetFloat("MyExposedParam 1", -80f);  // M�zik sesi tamamen kapat�ld�
        }
        else
        {
            audioMixer.SetFloat("MyExposedParam 1", -18f);  // M�zik sesi a��ld�
        }
    }

    public void ToggleSFX()
    {
        isSFXMuted = !isSFXMuted;

        if (isSFXMuted)
        {
            audioMixer.SetFloat("MyExposedParam", -80f);  // SFX sesi tamamen kapat�ld�
        }
        else
        {
            audioMixer.SetFloat("MyExposedParam", 0f);  // SFX sesi a��ld�
        }
    }

    public void PlaySFX(AudioClip clip, float volume)
    {
        if (!isSFXMuted)
        {
            SFXSource.PlayOneShot(clip, volume);
        }
    }
}
