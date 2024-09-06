using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PHealthBar : MonoBehaviour
{
    public Slider healthSlider;
    Damageable damageable;
    public TMP_Text healthText; 
    string healthFormat = "{0}/{1}"; 

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        damageable = player.GetComponent<Damageable>();
    }

    void Start()
    {
        healthSlider.value = CalculateSliderPercentage(damageable.Health, damageable.MaxHealth);
        UpdateHealthText(damageable.Health, damageable.MaxHealth);
    }

    private void OnEnable()
    {
        damageable.healthChanged.AddListener(OnPlayerHealthChanged);
    }

    private void OnDisable()
    {
        damageable.healthChanged.RemoveListener(OnPlayerHealthChanged);
    }

    private void OnPlayerHealthChanged(int newHealth, int maxHealth)
    {
        healthSlider.value = CalculateSliderPercentage(newHealth, maxHealth);
        UpdateHealthText(newHealth, maxHealth);
    }

    private float CalculateSliderPercentage(float currentHealth, float maxHealth)
    {
        return currentHealth / maxHealth;
    }

    private void UpdateHealthText(int currentHealth, int maxHealth)
    {
        healthText.text = string.Format(healthFormat, currentHealth, maxHealth);
    }
}
