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
    public AudioSource SFXCollectable;
    public AudioClip hourglassSound;
    [SerializeField] Animator animator;

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
    }

    
    void HourglassCollected()
    {
        Score += 50;
        ScoreText.text = string.Format(ScoreFormat, Score);
        SFXCollectable.PlayOneShot(hourglassSound, 0.2f);
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

    // Skoru azaltmak için fonksiyon
    public void DeductScore(int amount)
    {
        Score -= amount;
        ScoreText.text = string.Format(ScoreFormat, Score);
    }
}
