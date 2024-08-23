using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    int Score;
    public TMP_Text ScoreText;
    string ScoreFormat = "{0,5:0000}";
    public AudioSource hourglassSound;

    private void OnEnable()
    {
        ActionsListener.OnHourglassCollected +=HourglassCollected;
    }
    private void OnDisable()
    {
        ActionsListener.OnHourglassCollected -= HourglassCollected;
    }
    private void Awake()
    {
        Score = 0;
        ScoreText.text=string.Format(ScoreFormat, Score);
    }

    void HourglassCollected()
    {
        Score += 50;
        ScoreText.text = string.Format(ScoreFormat, Score);
        hourglassSound.PlayOneShot(hourglassSound.clip,0.4f);
    }
}
