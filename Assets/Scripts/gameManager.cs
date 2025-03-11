using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;
    int Score;
    public TMP_Text ScoreText;
    string ScoreFormat = "{0,5:000}";
    [SerializeField] Animator animator;
    [SerializeField] ManaBar manaBar;
    [SerializeField] Button pauseButton;
    [SerializeField] Button resumeButton;
    [SerializeField] Image crownImage;
    [SerializeField] Sprite fillCrown;
    [SerializeField] Sprite emptyCrown;

    Timer timer;
    public GameObject pauseMenuUI;
    public static bool isPaused = false;
   
    public float timeElapsed = 0f;
    public bool timerRunning = true;
    public TMP_Text timerText;
    public float timerCurrency = 0f;  // vakit nakittir
    public bool crownTaken = false;

    public bool isRewarded = false;
    private void OnEnable()
    {
        ActionsListener.OnHourglassCollected += HourglassCollected;
        ActionsListener.OnCoinCollected += CoinCollected;
        ActionsListener.OnCrownCollected += CrownCollected;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        ActionsListener.OnHourglassCollected -= HourglassCollected;
        ActionsListener.OnCoinCollected -= CoinCollected;
        ActionsListener.OnCrownCollected -= CrownCollected;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Awake()
    {
       
      

        if (instance == null )
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
      
        else
        {
            Destroy(gameObject);
            return;
        }
        timer = GetComponentInChildren<Timer>();
        Score = 0;
        ScoreText.text = string.Format(ScoreFormat, Score);
        pauseButton.onClick.AddListener(OnPauseButtonPressed);
        resumeButton.onClick.AddListener(OnResumeButtonPressed);
        Time.timeScale = 1;
        pauseMenuUI.SetActive(false);
    }

    private void Update()
    {
        if (timerRunning)
        {
            timeElapsed += Time.deltaTime;
            DisplayTime(timeElapsed);
        }
    }
    public void SaveCurrentLevel(string levelName)
    {
        PlayerPrefs.SetString("LastSavedLevel", levelName);
        PlayerPrefs.Save();
    }
    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "CreditsScene")
        {
            
            Destroy(gameObject);
            return;
        }

        if (PlayerMovement.instance == null)
        {
            SpawnPlayer();
        }
        else
        {
            MovePlayerToSpawn();
        }

        if (scene.name.StartsWith("level") || scene.name.StartsWith("Door") || scene.name.StartsWith("First"))
        {
            SaveCurrentLevel(scene.name);
        }

        if (scene.name == "FirstScene")
        {
            timeElapsed = 1f;
            timerRunning = false;
            if (timer != null)
            {
                timer.StopTimer();
            }
        }

        if (scene.name.Contains("Door"))
        {
            timerRunning = false;
            timerCurrency = timeElapsed;
            if (timer != null)
            {
                timer.StopTimer();
            }
            timeElapsed = 0f;

            if (DialogueManager.Instance != null)
            {
                DialogueManager.Instance.UpdateShopVisibility(true);
            }
        }
        else
        {
            timerRunning = true;
            if (DialogueManager.Instance != null)
            {
                DialogueManager.Instance.UpdateShopVisibility(false);
            }
        }
    }
    [SerializeField] private GameObject playerPrefab;

    private void SpawnPlayer()
    {
        if (playerPrefab != null)
        {
          
            GameObject player = Instantiate(playerPrefab);
           // PlayerMovement playerMovement = PlayerMovement.instance.GetComponent<PlayerMovement>();
           // playerMovement.InitializeComponents();
            MovePlayerToSpawn(); 
        }
       
    }

    public ManaBar GetManaBar()
    {
        return manaBar;
    }

    void HourglassCollected()
    {
        Score += 200;
        ScoreText.text = string.Format(ScoreFormat, Score);
    }
    public void AdWatched()
    {
        if (isRewarded == true)
        {
            Score += 300;
            ScoreText.text = string.Format(ScoreFormat, Score);
        }
        else
        {
            Score += 300;
        }

    }
    void CoinCollected()
    {
        Score += 50;
        ScoreText.text = string.Format(ScoreFormat, Score);
    }
    void CrownCollected()
    {
        crownImage.sprite=fillCrown;
        crownTaken = true;
    }
    public void NextLevel()
    {
        StartCoroutine(LoadLevel());
    }
    
    IEnumerator LoadLevel()
    {
        animator.SetTrigger("end");
        yield return new WaitForSeconds(0);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        animator.SetTrigger("start");
    }

    public int GetScore()
    {
        return Score;
    }

    public void DeductScore(int amount)
    {
        Score -= amount;
        ScoreText.text = string.Format(ScoreFormat, Score);
    }
    public void EmptyCrown()
    {
        crownImage.sprite = emptyCrown;
        crownTaken = false;
    }

    public void OnPauseButtonPressed()
    {
        if (!isPaused)
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0;
            isPaused = true;
        }
    }

    public void OnResumeButtonPressed()
    {
        if (isPaused)
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1;
            isPaused = false;
        }
    }
    public void OnRestartButtonPressed()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    var  a=  SceneManager.GetActiveScene();
        SceneManager.LoadScene(a.buildIndex);
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }
    private void MovePlayerToSpawn()
    {

        string sceneName = SceneManager.GetActiveScene().name;

       if (sceneName == "Door1")
        {
            PlayerMovement.instance.transform.position = new Vector2(-8, 1);
        }
    
        else if (sceneName == "Door2")
        {
            PlayerMovement.instance.transform.position = new Vector2(5, 3); 
        }
        else if (sceneName == "Door3")
        {
            PlayerMovement.instance.transform.position = new Vector2(5, 3);
        }
        else if (sceneName == "level1")
        {
            PlayerMovement.instance.transform.position = new Vector2(-170, -2);
            EmptyCrown();
        }
        else if (sceneName == "level2")
        {
            PlayerMovement.instance.transform.position = new Vector2(37, 1);
            EmptyCrown();
        }
        else if (sceneName == "level3")
        {
            PlayerMovement.instance.transform.position = new Vector2(35, 5);
            EmptyCrown();
        }
       else if(sceneName == "level4")
        {
            PlayerMovement.instance.transform.position = new Vector2(240, -23);
        }
        else if (sceneName == "level5")
        {
            PlayerMovement.instance.transform.position = new Vector2(73 , 1);
        }   
    }
}
