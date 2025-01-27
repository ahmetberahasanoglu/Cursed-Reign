using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [Header("Stamina Settings")]
    public float maxStamina = 3f; 
    public float regenRate = 0.5f;
    public float staminaUsage = 1f; 

    [Header("UI Components")]
    public Slider staminaSlider;
    [SerializeField] private TMP_Text staminaText;
    public Animator staminaBarAnimator;
    private float currentStamina;
    private bool canUseStamina = true;

    private void Start()
    {
        currentStamina = maxStamina; 
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = currentStamina;
    }

    private void Update()
    {
      
        if (currentStamina < maxStamina && canUseStamina)
        {
            RegenerateStamina();
        }

      
        staminaSlider.value = currentStamina;
        UpdateStaminaText();
    }
    private void UpdateStaminaText()
    {
        staminaText.text = $"{currentStamina:F1} / {maxStamina}";
    }
    public bool UseStamina()
    {
        if (currentStamina >= staminaUsage)
        {
            staminaBarAnimator.SetTrigger("UseStamina");
            return true; 
        }
        else
        {
            return false;
        }
    }
    public void use()
    {
        currentStamina -= staminaUsage;
        staminaSlider.value = currentStamina;
    }

    private void RegenerateStamina()
    {
        currentStamina += regenRate * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina); 
    }

   /* public void StopRegen() // Stamina kullanýmýný durdurma (gerekirse)
    {
        canUseStamina = false;
    }

    public void ResumeRegen() //gerekirse tekrar acmaca
    {
        canUseStamina = true;
    }*/
}