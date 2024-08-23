using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 0;
    public bool timeIsRunning = true;
    public TMP_Text timeText;
    public AudioClip failureSound; // Ba�ar�s�zl�k sesi
    public float volume = 0.5F;
    public Color normalColor = Color.white; // Normal renk (Beyaz)
    public Color warningColor = Color.red; // Uyar� rengi (K�rm�z�)
    public float warningThreshold = 10f; // Zaman�n k�rm�z�ya d�nmeye ba�lad��� e�ik (10 saniye)

    void Start()
    {
        timeIsRunning = true;
        timeText.color = normalColor; // �lk ba�ta beyaz rengiyle ba�la
    }

    void Update()
    {
        if (timeIsRunning)
        {
            if (timeRemaining >= 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
                UpdateTextColor(timeRemaining); // Zaman� g�sterirken rengi g�ncelle
            }
            else
            {
                timeIsRunning = false;
                timeRemaining = 0.1f; // Zaman eksiye d��mesin diye
                DisplayTime(timeRemaining);
                PlayFailureSound(); // Zaman bitince ses �al
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
            timeText.color = warningColor; // K�rm�z� renk
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
            AudioSource.PlayClipAtPoint(failureSound, Camera.main.transform.position, volume); // Sesin kameran�n oldu�u noktada �almas�n� sa�la
        }
    }
}
