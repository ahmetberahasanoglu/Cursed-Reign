using System.Collections;
using TMPro;
using UnityEngine;

public class IntroTexts : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public float typingSpeed = 0.02f;
    private string fullText;

    private void Awake()
    {
        fullText = textMeshPro.text;
    }
    private void OnEnable()
    {
            
            textMeshPro.text = "";         
            StartCoroutine(TypeText());    
    }

    private IEnumerator TypeText()
    {
        foreach (char letter in fullText)
        {
            textMeshPro.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
