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
        
        SceneManager.sceneLoaded += OnSceneLoaded;

        
        BindDamageable();
    }

    private void OnDisable()
    {
      
        SceneManager.sceneLoaded -= OnSceneLoaded;

        
        if (damageable != null)
        {
            damageable.healthChanged.RemoveListener(OnPlayerHealthChanged);
        }
    }
    private void Start()
    {
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BindDamageable(); 
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

    public void UpdateHealthText(int currentHealth, int maxHealth)
    {
        healthText.text = string.Format(healthFormat, currentHealth, maxHealth);
    }
}
