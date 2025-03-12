using UnityEngine;

public static class LocaleHelper 
{
    public static string GetSupportedLanguageCode()
    {
        SystemLanguage lang = Application.systemLanguage;
        switch (lang)
        {
            case SystemLanguage.English:
                return LocaleApplication.EN;
            case SystemLanguage.Turkish:
                return LocaleApplication.TR;
            default:
                return GetDefaultSupportedLanguageCode();
        }
    }
    static string GetDefaultSupportedLanguageCode() {
        return LocaleApplication.EN;
    }
}
