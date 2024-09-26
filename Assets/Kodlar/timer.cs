using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float timeElapsed = 0;
    public bool timeIsRunning = true;
    public TMP_Text timeText;
    public Color normalColor = Color.white;
    public Color warningColor = Color.red;
    public float warningThreshold = 120f;
    audiomanager manager;
    bool audioRunned = false;
    [SerializeField] float volume = 0.5f;

    void Start()
    {
        ResetTimer();
        manager = audiomanager.Instance;
        if (manager == null)
        {
            Debug.Log("Timer: AudioManager instance not found.");
        }
    }

    void Update()
    {
        if (timeIsRunning)
        {
            timeElapsed += Time.deltaTime;
            DisplayTime(timeElapsed);
            UpdateTextColor(timeElapsed);
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
            timeText.color = warningColor;
            if (!audioRunned)
            {
                manager.PlaySFX(manager.timesUP, volume);
                audioRunned = true;
            }
        }
        else
        {
            timeText.color = normalColor;
        }
    }

   
    public void StopTimer()
    {
        timeIsRunning = false;
    }


    public void ResetTimer()
    {
        timeElapsed = 0;
        timeIsRunning = true;
        audioRunned = false;
        timeText.color = normalColor;
    }


    public float GetTimeAsCurrency()
    {
        return timeElapsed;
    }
}
