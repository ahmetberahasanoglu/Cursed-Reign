using UnityEngine;

public class ChangeLanguage : MonoBehaviour
{
    public void OnClickChangeLangRunTime(string lang)
    {
            LocalizationManager.Instance.ChangeLanguage(lang);
    }
}
