using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] Button resumeButton; // Devam butonu i�in
    public GameObject pauseMenuUI;
    public static bool isPaused = false;

    private void OnEnable()
    {
        ActionsListener.OnHourglassCollected += HourglassCollected;
        ActionsListener.OnCoinCollected += CoinCollected;
        SceneManager.sceneLoaded += OnSceneLoaded;  // Sahne y�klendi�inde �a��r�l�r
    }

    private void OnDisable()
    {
        ActionsListener.OnHourglassCollected -= HourglassCollected;
        ActionsListener.OnCoinCollected -= CoinCollected;
        SceneManager.sceneLoaded -= OnSceneLoaded;  // Event'� kald�r
    }

    private void Awake()
    {
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

    // Sahne y�klendi�inde shop'un g�sterilip g�sterilmeyece�ini kontrol eder
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (DialogueManager.Instance != null)
        {
            // Sahne "ShopScene" ise shop g�sterimi aktif, de�ilse pasif
            if (scene.name == "Door1")
            {
                DialogueManager.Instance.UpdateShopVisibility(true);  // Shop a��l�r
            }
            else
            {
                DialogueManager.Instance.UpdateShopVisibility(false); // Shop a��lmaz
            }
        }
        else
        {
            Debug.LogError("DialogueManager instance bulunamad�.");
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
}
