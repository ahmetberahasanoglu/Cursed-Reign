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
    Timer timer;
    public GameObject pauseMenuUI;
    public static bool isPaused = false;
   
    public float timeElapsed = 0f;
    public bool timerRunning = true;
    public TMP_Text timerText;
    public float timerCurrency = 0f;  // Holds time-based currency

    private void OnEnable()
    {
        ActionsListener.OnHourglassCollected += HourglassCollected;
        ActionsListener.OnCoinCollected += CoinCollected;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        ActionsListener.OnHourglassCollected -= HourglassCollected;
        ActionsListener.OnCoinCollected -= CoinCollected;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Awake()
    {
       
        timer =GetComponentInChildren<Timer>();
        Score = 0;
        ScoreText.text = string.Format(ScoreFormat, Score);

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

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

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Called when a new scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        MovePlayerToSpawn();
        if (scene.name.Contains("Door"))  
        {
            timerRunning = false;
            timerCurrency = timeElapsed;  // zaman� para olarak ald�k
            timer.StopTimer();
            timeElapsed = 0f;  // doora girince timer'I s�f�rla
                               

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

    public ManaBar GetManaBar()
    {
        return manaBar;
    }

    void HourglassCollected()
    {
        Score += 200;
        ScoreText.text = string.Format(ScoreFormat, Score);
    }

    void CoinCollected()
    {
        Score += 50;
        ScoreText.text = string.Format(ScoreFormat, Score);
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
        }
        else if (sceneName == "level2")
        {
            PlayerMovement.instance.transform.position = new Vector2(37, 1);
        }
        else if (sceneName == "level3")
        {
            PlayerMovement.instance.transform.position = new Vector2(35, 5);
        }
    }
}
