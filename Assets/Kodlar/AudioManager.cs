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
    public AudioClip defaultLoopClip;  // Varsayýlan müzik
    public AudioClip doorSceneClip;    // Door sahnesinde çalacak müzik
    public AudioClip battleSceneClip;  // Savaþ sahnesinde çalacak müzik
    public AudioClip shopSceneClip;    // Shop sahnesinde çalacak müzik
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
        PlayMusicForCurrentScene();

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
    private void PlayMusicForCurrentScene()
    {
        AudioClip selectedClip = defaultLoopClip;  // Varsayýlan müzik

        // Sahneye göre müzik seçimi
        if (SceneManager.GetActiveScene().name == "Door1")
        {
            selectedClip = doorSceneClip;
        }
        else if (SceneManager.GetActiveScene().name == "Level1")
        {
            selectedClip = battleSceneClip;
        }
        else if (SceneManager.GetActiveScene().name == "Door0")
        {
            selectedClip = shopSceneClip;
        }
        // Baþka sahneler için ekle...

        PlayMusic(selectedClip);
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;  // Sahne yüklendiðinde çaðýrýlýr
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;  // Event kaldýrýlýr
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForCurrentScene();  // Yeni sahne yüklendiðinde müziði güncelle
    }
    private void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip != clip)
        {
            musicSource.Stop();  // Þu anki müziði durdur
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();  // Yeni müziði çal
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
