using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float timeElapsed = 0; // Geçen zamaný takip eder
    public bool timeIsRunning = true;
    public TMP_Text timeText;
    public Color normalColor = Color.white; // Normal renk (Beyaz)
    public Color warningColor = Color.red; // Uyarý rengi (Kýrmýzý)
    public float warningThreshold = 120f; // Zamanýn kýrmýzýya dönmeye baþladýðý eþik (2 dakika)

    void Start()
    {
        timeIsRunning = true;
        timeText.color = normalColor; // Ýlk baþta beyaz rengiyle baþla
    }

    void Update()
    {
        if (timeIsRunning)
        {
            timeElapsed += Time.deltaTime; // Geçen zamaný artýr
            DisplayTime(timeElapsed);
            UpdateTextColor(timeElapsed); // Zamaný gösterirken rengi güncelle
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void UpdateTextColor(float timeToDisplay)
    {
        if (timeToDisplay >= warningThreshold)
        {
            timeText.color = warningColor; // Kýrmýzý renk
        }
        else
        {
            timeText.color = normalColor; // Normal renk (Beyaz)
        }
    }
}
