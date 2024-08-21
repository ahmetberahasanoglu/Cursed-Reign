using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PHealthBar : MonoBehaviour
{
    public Slider healthSlider;
    Damageable damageable;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        damageable = player.GetComponent<Damageable>();
    }
    void Start()
    {

        healthSlider.value = CalculateSliderPercentage(damageable.Health, damageable.MaxHealth);

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
    }

    private float CalculateSliderPercentage(float currentHealth, float maxHealth)
    {
        return currentHealth / maxHealth;
    }

}
