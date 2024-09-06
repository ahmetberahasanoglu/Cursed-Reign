using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManaBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text manaText; 
    private string manaFormat = "{0}/{1}"; 

    
    public void SetMaxMana(int maxMana)
    {
        slider.maxValue = maxMana;
        slider.value = maxMana;
        UpdateManaText(maxMana, maxMana);
    }

    
    public void SetMana(int currentMana)
    {
        slider.value = currentMana;
        UpdateManaText(currentMana, (int)slider.maxValue);
    }

    
    private void UpdateManaText(int currentMana, int maxMana)
    {
        manaText.text = string.Format(manaFormat, currentMana, maxMana);
    }
}
