using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ContinueButton : MonoBehaviour
{
    Button button;
    TextMeshProUGUI text;

    void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();
    }

    private void Start()
    {
        // En son kaydedilen bölüm varsa butonu aktif hale getir
        if (PlayerPrefs.HasKey("LastSavedLevel"))
        {
            button.interactable = true;
            Color color = text.color;
            color.a = 1f;
            text.color = color;
        }
        else
        {
            button.interactable = false;
            Color color = text.color;
            color.a = 0.5f;
            text.color = color;
        }
    }

    public void Continue()
    {
        if (PlayerPrefs.HasKey("LastSavedLevel"))
        {
            string savedLevel = PlayerPrefs.GetString("LastSavedLevel");
            SceneManager.LoadScene(savedLevel);
        }
    }
  
}





