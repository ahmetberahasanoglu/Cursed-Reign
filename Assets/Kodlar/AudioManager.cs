using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class audiomanager : MonoBehaviour
{
    [Header("---AUDIO SOURCE---")]
    public AudioSource SFXSource;
    [SerializeField] AudioSource musicSource;

    [Header("---AUDIO MIXER---")]
    public AudioMixer audioMixer;

    [Header("---AUDIO Clip---")]
    // public AudioClip introClip;
    public AudioClip loopClip;
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
    public AudioClip timesUP;

    public static audiomanager Instance { get; private set; }

    [Header("Pause Menu Buttonlari")]
    public Button musicToggleButton;
    public Button SFXToggleButton;

    private bool isMusicMuted = false;  // Müzik durumu
    private bool isSFXMuted = false;    // SFX durumu

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
        musicSource.clip = loopClip;
        musicSource.loop = true;
        musicSource.Play();

        // Butonlara iþlevleri atama
        musicToggleButton.onClick.AddListener(ToggleMusic);
        SFXToggleButton.onClick.AddListener(ToggleSFX);
    }

    private void Update()
    {
        // gameManager.isPaused kontrolü
        if (gameManager.isPaused)
        {
            audioMixer.SetFloat("MyExposedParam 1", -80f);
            audioMixer.SetFloat("MyExposedParam", -80f);
        }
        else
        {
            if (!isMusicMuted)
                audioMixer.SetFloat("MyExposedParam 1", -18f); // Orijinal ses seviyesi
            if (!isSFXMuted)
                audioMixer.SetFloat("MyExposedParam", 0f);     // Orijinal ses seviyesi
        }
    }

    public void ToggleMusic()
    {
        isMusicMuted = !isMusicMuted;

        if (isMusicMuted)
        {
            audioMixer.SetFloat("MyExposedParam 1", -80f);  // Müzik sesi tamamen kapatýldý
        }
        else
        {
            audioMixer.SetFloat("MyExposedParam 1", -18f);  // Müzik sesi açýldý
        }
    }

    public void ToggleSFX()
    {
        isSFXMuted = !isSFXMuted;

        if (isSFXMuted)
        {
            audioMixer.SetFloat("MyExposedParam", -80f);  // SFX sesi tamamen kapatýldý
        }
        else
        {
            audioMixer.SetFloat("MyExposedParam", 0f);  // SFX sesi açýldý
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
