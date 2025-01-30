using UnityEngine;
using UnityEngine.UI;

public class DashBar : MonoBehaviour
{
    private PlayerMovement playerMovement;
    [SerializeField] private Slider slider;

    public float maxDashCooldown;
    private float currentDashCooldown;
    private bool isCooldownActive = false;

    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement != null)
        {
            maxDashCooldown = playerMovement.dashCooldown; // Dash cooldown s�resi
            slider.maxValue = maxDashCooldown;
            ResetCooldown(); // Bar� ba�lat
        }
        else
        {
            Debug.LogError("PlayerMovement bulunamad�!");
        }
    }

    void Update()
    {
        if (isCooldownActive)
        {
            // Dash cooldown bar�n� doldur
            currentDashCooldown += Time.deltaTime;
            slider.value = currentDashCooldown;

            if (currentDashCooldown >= maxDashCooldown)
            {
                isCooldownActive = false; // Dolduktan sonra tekrar dash kullan�labilir
            }
        }
    }

    public bool IsCooldownActive()
    {
        return isCooldownActive;
    }

    public void TriggerDashCooldown()
    {
        currentDashCooldown = 0;
        isCooldownActive = true;
        slider.value = currentDashCooldown;
    }

    private void ResetCooldown()
    {
        currentDashCooldown = maxDashCooldown;
        slider.value = maxDashCooldown;
    }

    public void UpdateDashCooldown(float newMaxCooldown)
    {
        maxDashCooldown = newMaxCooldown;
        slider.maxValue = maxDashCooldown;
        if (!isCooldownActive)
        {
            ResetCooldown();
        }
    }
}
