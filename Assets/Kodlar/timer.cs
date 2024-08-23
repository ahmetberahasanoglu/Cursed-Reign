using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 0;
    public bool timeIsRunning = true;
    public TMP_Text timeText;
    public AudioClip failureSound; // Baþarýsýzlýk sesi
    public float volume = 0.5F;
    public Color normalColor = Color.white; // Normal renk (Beyaz)
    public Color warningColor = Color.red; // Uyarý rengi (Kýrmýzý)
    public float warningThreshold = 10f; // Zamanýn kýrmýzýya dönmeye baþladýðý eþik (10 saniye)

    void Start()
    {
        timeIsRunning = true;
        timeText.color = normalColor; // Ýlk baþta beyaz rengiyle baþla
    }

    void Update()
    {
        if (timeIsRunning)
        {
            if (timeRemaining >= 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
                UpdateTextColor(timeRemaining); // Zamaný gösterirken rengi güncelle
            }
            else
            {
                timeIsRunning = false;
                timeRemaining = 0.1f; // Zaman eksiye düþmesin diye
                DisplayTime(timeRemaining);
                PlayFailureSound(); // Zaman bitince ses çal
            }
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
        if (timeToDisplay <= warningThreshold)
        {
            timeText.color = warningColor; // Kýrmýzý renk
        }
        else
        {
            timeText.color = normalColor; // Normal renk (Beyaz)
        }
    }

    void PlayFailureSound()
    {
        if (failureSound != null)
        {
            AudioSource.PlayClipAtPoint(failureSound, Camera.main.transform.position, volume); // Sesin kameranýn olduðu noktada çalmasýný saðla
        }
    }
}
