using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    int Score;
    public Text ScoreText;
    string ScoreFormat = "{0,5:0000}";
    private void Awake()
    {
        Score = 0;
        ScoreText.text=string.Format(ScoreFormat, Score);
    }

    void HourglassCollected()
    {
        Score += 50;
    }
}
