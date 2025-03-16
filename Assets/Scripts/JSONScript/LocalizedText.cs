using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class LocalizedText : MonoBehaviour
{
    [SerializeField]
    public string localizationKey;//public yapabiliriz
    TextMeshProUGUI textMeshProComponent;
   IEnumerator Start()
    {
        while (!LocalizationManager.Instance.isReady)
        {
            yield return null;
        }
        AttributionText();
    }
    public void AttributionText()
    {
        if (textMeshProComponent == null) {
            textMeshProComponent = gameObject.GetComponent<TextMeshProUGUI>();
                }
        try
        {
            textMeshProComponent.text= LocalizationManager.Instance.GetTextForKey(localizationKey);
        }
        catch (Exception e) { 
        Debug.LogError(e);
}
    }

    
}
