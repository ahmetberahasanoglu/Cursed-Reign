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
    [SerializeField] Button pauseButton;
    [SerializeField] Button resumeButton; // Devam butonu i�in
    public GameObject pauseMenuUI;
    public static bool isPaused = false;

    private void OnEnable()
    {
        ActionsListener.OnHourglassCollected += HourglassCollected;
    }

    private void OnDisable()
    {
        ActionsListener.OnHourglassCollected -= HourglassCollected;
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

    void HourglassCollected()
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

    // Skoru azaltmak i�in fonksiyon
    public void DeductScore(int amount)
    {
        Score -= amount;
        ScoreText.text = string.Format(ScoreFormat, Score);
    }

    // Pause butonuna bas�ld���nda
    public void OnPauseButtonPressed()
    {
        if (!isPaused)
        {
            pauseMenuUI.SetActive(true);   // Pause men�s�n� aktif et
            Time.timeScale = 0;            // Oyunu durdur
            isPaused = true;
        }
    }

    // Devam butonuna bas�ld���nda
    public void OnResumeButtonPressed()
    {
        if (isPaused)
        {
            pauseMenuUI.SetActive(false);  // Pause men�s�n� kapat
            Time.timeScale = 1;            // Oyunu yeniden ba�lat
            isPaused = false;
        }
    }
    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }
  

}
