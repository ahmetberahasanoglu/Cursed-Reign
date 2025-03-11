using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class TimelineManager : MonoBehaviour
{
    public PlayableDirector timeline;
    public Button nextButton;
    private double[] timeStamps = { 6.01, 12.01, 18.01, 24.01, 30.01 };
    private int currentPauseIndex = 0;

    void Start()
    {
        nextButton.onClick.AddListener(SkipTimeline);
        timeline.Play();
    }

   

    void SkipTimeline()
    {
        if (currentPauseIndex < timeStamps.Length)
        {
            timeline.time = timeStamps[currentPauseIndex]; 
            currentPauseIndex++;
        }
    }
}
