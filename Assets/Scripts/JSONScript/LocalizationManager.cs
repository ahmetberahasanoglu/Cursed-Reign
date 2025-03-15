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
    [HideInInspector]
    public bool isReady = false;
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
                instance = FindObjectOfType(typeof(LocalizationManager)) as LocalizationManager;

            }
            return instance;

        }
    }
    #endregion InstanceFunction

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        LANGUAGE_CHOOSE = LocaleHelper.GetSupportedLanguageCode();
        FULL_NAME_TEXT_FILE = FILENAME_PREFIX + LANGUAGE_CHOOSE.ToLower() + FILE_EXTENSION;

#if UNITY_ANDROID
        FULL_PATH_TEXT_FILE = Path.Combine(Application.persistentDataPath, FULL_NAME_TEXT_FILE);
#else
        FULL_PATH_TEXT_FILE = Path.Combine(Application.streamingAssetsPath, FULL_NAME_TEXT_FILE);
#endif
        yield return StartCoroutine(LoadJsonLanguageData());
        isReady = true;
    }
    IEnumerator LoadJsonLanguageData()
    {
        CheckFileExist();
        yield return new WaitUntil(() => isFileFound);
        loadedData = JsonUtility.FromJson<LocalizationData>(LOADED_JSON_TEXT);
        localizedDictionary = new Dictionary<string, string>(loadedData.items.Count);
        loadedData.items.ForEach(item =>
        {
            try
            {
                localizedDictionary.Add(item.key, item.value);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        });


    }
    private void CheckFileExist()
    {
        if (!File.Exists(FULL_PATH_TEXT_FILE))
        {
            GetURLFileText();
            StartCoroutine(CopyFileFromWeb(URL));
        }
        else
        {
            LoadFileContents();
        }
    }
    private void GetURLFileText()
    {
        switch (LANGUAGE_CHOOSE)
        {
            case LocaleApplication.EN:
                URL = "https://raw.githubusercontent.com/ahmetberahasanoglu/Cursed-Reign/refs/heads/main/Assets/Resources/text_en.json";
                break;
            case LocaleApplication.TR:
                URL = "https://raw.githubusercontent.com/ahmetberahasanoglu/Cursed-Reign/refs/heads/main/Assets/Resources/text_tr.json";
                break;
            default:
                URL = "https://raw.githubusercontent.com/ahmetberahasanoglu/Cursed-Reign/refs/heads/main/Assets/Resources/text_en.json";//raw dosyalarý almamýz gerekiyormus txt dosyasý olmalarýna gerek yok
                break;
        }
    }
    IEnumerator CopyFileFromWeb(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError("Internet baglantinizi ve .txt dosyanýzý kontrol edin");
            Debug.LogWarning("Resources kýsmýndan dosya kopyalamak için 2. denememizi yaptýk");
            CopyFileFromResources();
            yield break;
        }

        LOADED_JSON_TEXT = www.downloadHandler.text;
        Debug.Log("WE copy file on a string");
        File.WriteAllText(FULL_PATH_TEXT_FILE, LOADED_JSON_TEXT);
        Debug.Log("WE writting our file on streaming asset");
        StartCoroutine(WaitCreationFile());

    }
    private void LoadFileContents()
    {
        LOADED_JSON_TEXT = File.ReadAllText(FULL_PATH_TEXT_FILE);
        isFileFound = true;
        Debug.Log("Loaded JSON: " + LOADED_JSON_TEXT);
        Debug.Log("Dosya yolu: " + FULL_PATH_TEXT_FILE);
    }
    private void CopyFileFromResources()
    {
        TextAsset myFile = Resources.Load(FILENAME_PREFIX + LANGUAGE_CHOOSE) as TextAsset;
        if (myFile == null)
        {
            Debug.LogError("Make sure the file" + FILENAME_PREFIX + LANGUAGE_CHOOSE + "is in resources folder");
            return;
        }
        LOADED_JSON_TEXT = myFile.ToString();
        File.WriteAllText(FULL_PATH_TEXT_FILE, LOADED_JSON_TEXT);
        StartCoroutine(WaitCreationFile());
    }
    IEnumerator WaitCreationFile()
    {
        FileInfo myFile = new FileInfo(FULL_PATH_TEXT_FILE);
        float timeOut = 0.0f;

        while (timeOut < 5.0f && !IsFileFinishCreate(myFile))
        {
            timeOut += Time.deltaTime;
            yield return null;

        }
        Debug.Log("The creation file is succeed");
    }
    private bool IsFileFinishCreate(FileInfo file)
    {
        FileStream stream = null;
        try
        {
            stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        }

        catch (IOException)
        {
            isFileFound = true;
            Debug.Log("dosyayý bulduk");
            return true;
        }
        finally
        {
            if (stream != null)
            {
                stream.Close();
            }

        }
        //dosya bulunamadý
        isFileFound = false;
        return false;
    }
    public string GetTextForKey(string localizationKey)
    {
        if (localizedDictionary.ContainsKey(localizationKey))
        {
            return localizedDictionary[localizationKey];
        }
        else
        {
            return "Error keyler " + localizationKey + " ile uyusmuyor";
        }
    }
    IEnumerator SwitchLanguageRunTime(string language)
    {
        if (!isTryChangeLangRunTime)
        {
            isTryChangeLangRunTime = true;
            isFileFound = false;
            isReady = false;
            LANGUAGE_CHOOSE = language;

            FULL_NAME_TEXT_FILE = FILENAME_PREFIX + LANGUAGE_CHOOSE.ToLower() + FILE_EXTENSION;

            //#if UNITY_ANDROID
            //           FULL_PATH_TEXT_FILE = Path.Combine(Application.persistentDataPath, FULL_NAME_TEXT_FILE);
            //#else
            //     FULL_PATH_TEXT_FILE = Path.Combine(Application.streamingAssetsPath, FULL_NAME_TEXT_FILE);
            //#endif
            FULL_PATH_TEXT_FILE = Path.Combine(Application.streamingAssetsPath, FULL_NAME_TEXT_FILE);
            yield return StartCoroutine(LoadJsonLanguageData());
            isReady = true;

            LocalizedText[] arrayText = FindObjectsOfType<LocalizedText>();
            for (int i = 0; i < arrayText.Length; i++)
            {
                arrayText[i].AttributionText();
            }
            isTryChangeLangRunTime = false;
        }
    }
    public void ChangeLanguage(string lang)
    {
        StartCoroutine(SwitchLanguageRunTime(lang));
    }

}
