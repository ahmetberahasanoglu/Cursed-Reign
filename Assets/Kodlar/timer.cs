using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float timeElapsed = 0; // Ge�en zaman� takip eder
    public bool timeIsRunning = true;
    public TMP_Text timeText;
    public Color normalColor = Color.white; // Normal renk (Beyaz)
    public Color warningColor = Color.red; // Uyar� rengi (K�rm�z�)
    public float warningThreshold = 120f; // Zaman�n k�rm�z�ya d�nmeye ba�lad��� e�ik (2 dakika)
    audiomanager manager;
    bool audioRunned = false;
    [SerializeField] float volume = 0.5f;

    void Start()
    {
        timeIsRunning = true;
        timeText.color = normalColor; // �lk ba�ta beyaz rengiyle ba�la
        manager = audiomanager.Instance;
        if(manager==null)
        {
            Debug.Log("timer hata instance");
        }
    }

    void Update()
    {
        if (timeIsRunning)
        {
            timeElapsed += Time.deltaTime; // Ge�en zaman� art�r
            DisplayTime(timeElapsed);
            UpdateTextColor(timeElapsed); // Zaman� g�sterirken rengi g�ncelle
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
            timeText.color = warningColor; // K�rm�z� renk
            if (!audioRunned)
            {
                manager.PlaySFX(manager.timesUP, volume);
                audioRunned = true;
            }
        }
        else
        {
            timeText.color = normalColor; // Normal renk (Beyaz)
        }
    }
}
