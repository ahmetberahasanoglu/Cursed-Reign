using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PHealthBar : MonoBehaviour
{
    public Slider healthSlider;
    private Damageable damageable;
    public TMP_Text healthText;
    string healthFormat = "{0}/{1}";

    private void OnEnable()
    {
        // Rebind player reference after scene changes
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Find and initialize the player's health component
        BindDamageable();
    }

    private void OnDisable()
    {
        // Unbind from the sceneLoaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;

        // Unsubscribe from healthChanged event
        if (damageable != null)
        {
            damageable.healthChanged.RemoveListener(OnPlayerHealthChanged);
        }
    }

    // Called when the scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BindDamageable(); // Ensure the health bar is bound to the new scene's player
    }

    private void BindDamageable()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            damageable = player.GetComponent<Damageable>();

            if (damageable != null)
            {
                // Subscribe to healthChanged event
                damageable.healthChanged.AddListener(OnPlayerHealthChanged);

                // Initialize health slider and text with current player health
                healthSlider.value = CalculateSliderPercentage(damageable.Health, damageable.MaxHealth);
                UpdateHealthText(damageable.Health, damageable.MaxHealth);
            }
            else
            {
                Debug.LogError("Player is missing a Damageable component!");
            }
        }
        else
        {
            Debug.LogError("Player not found in scene!");
        }
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
