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
            maxDashCooldown = playerMovement.dashCooldown; // Dash cooldown süresi
            slider.maxValue = maxDashCooldown;
            ResetCooldown(); // Barý baþlat
        }
        else
        {
            Debug.LogError("PlayerMovement bulunamadý!");
        }
    }

    void Update()
    {
        if (isCooldownActive)
        {
            // Dash cooldown barýný doldur
            currentDashCooldown += Time.deltaTime;
            slider.value = currentDashCooldown;

            if (currentDashCooldown >= maxDashCooldown)
            {
                isCooldownActive = false; // Dolduktan sonra tekrar dash kullanýlabilir
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
