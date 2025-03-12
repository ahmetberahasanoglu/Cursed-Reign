using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class LocalizationManager : MonoBehaviour
{
    [Header("Important String")]
    private const string FILENAME_PREFIX = "text_";
    private const string FILE_EXTENSION = ".json";
    private string FULL_NAME_TEXT_FILE;
    private string URL = "";
    private string FULL_PATH_TEXT_FILE;
    private string LANGUAGE_CHOOSE = "EN";
    private string LOADED_JSON_TEXT = "";

    [Header("Important bool")]
    private bool isReady = false;
    private bool isFileFound = false;
    private bool isTryChangeLangRunTime = false;

    [Header("JSON Variable")]
    private Dictionary<string, string> localizedDictionary;
    private LocalizationData loadedData;


    #region InstanceFunction
    private static LocalizationManager instance;
    public static LocalizationManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance=FindObjectOfType(typeof(LocalizationManager))as LocalizationManager;

            }
            return instance;

        }
    }
    #endregion InstanceFunction


    // Start is called before the first frame update
    IEnumerator Start()
    {
        LANGUAGE_CHOOSE =LocaleHelper.GetSupportedLanguageCode();
        FULL_NAME_TEXT_FILE = FILENAME_PREFIX + LANGUAGE_CHOOSE.ToLower()+FILE_EXTENSION;
        FULL_PATH_TEXT_FILE = Path.Combine(Application.streamingAssetsPath, FULL_NAME_TEXT_FILE);
        yield return StartCoroutine(LoadJsonLanguageData());
    }
    IEnumerator LoadJsonLanguageData()
    {
        yield return null;

    }



    
}
