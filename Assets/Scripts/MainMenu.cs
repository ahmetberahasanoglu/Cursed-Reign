using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("IntroScene");
    }
    public void QuitGame()
    {
       Application.Quit();
    }
    public void Continue()
    {

    }
    public void GoToSettingsMenu()
    {
        SceneManager.LoadScene("SettingsMenu");
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void GoToMainCredits()
    {
        SceneManager.LoadScene("CreditsScene");
    }
}
