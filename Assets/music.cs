using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class music : MonoBehaviour
{
    
    private bool isMusicMuted = false;
    public AudioMixer audioMixer;
    public Button musicToggleButton;
    private void Start()
    {
        musicToggleButton.onClick.AddListener(ToggleMusic);
    }
   
    public void ToggleMusic()
    {
        isMusicMuted = !isMusicMuted;

        if (isMusicMuted)
        {
            audioMixer.SetFloat("MyExposedParam 1", -80f);  // Müzik sesi tamamen kapatýldý
        }
        else
        {
            audioMixer.SetFloat("MyExposedParam 1", -18f);  // Müzik sesi açýldý
        }
    }
}
