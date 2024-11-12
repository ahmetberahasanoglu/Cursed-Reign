using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroLoader : MonoBehaviour
{

    private void OnEnable()
    {

        SceneManager.LoadScene("FirstScene", LoadSceneMode.Single);
    }
}
 