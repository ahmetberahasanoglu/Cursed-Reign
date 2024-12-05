using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DashBar : MonoBehaviour
{
    private PlayerMovement playerMovement;
    [SerializeField] private Slider slider;
   // [SerializeField] private TMP_Text dashText;
   // private string dashFormat = "{0}/{1}";

    private float maxDashCooldown;
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
            if (currentDashCooldown >= maxDashCooldown)
            {
                currentDashCooldown = maxDashCooldown;
                isCooldownActive = false; // Dolduktan sonra tekrar dash kullan�labilir
            }
            slider.value = currentDashCooldown;
           // UpdatedashText();
        }
    }

    public void TriggerDashCooldown()
    {
    
        currentDashCooldown = 0;
        isCooldownActive = true; 
        slider.value = currentDashCooldown;
     //   UpdatedashText();
    }

    private void ResetCooldown()
    {
        currentDashCooldown = maxDashCooldown;
        slider.value = maxDashCooldown;
        //UpdatedashText();
    }

   /* private void UpdatedashText() dash �st�nde Yaz� yazmas�n grk yok
    {
        dashText.text = string.Format(dashFormat, Mathf.Ceil(currentDashCooldown), maxDashCooldown);
    }*/
}
