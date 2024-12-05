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
            if (currentDashCooldown >= maxDashCooldown)
            {
                currentDashCooldown = maxDashCooldown;
                isCooldownActive = false; // Dolduktan sonra tekrar dash kullanýlabilir
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

   /* private void UpdatedashText() dash üstünde Yazý yazmasýn grk yok
    {
        dashText.text = string.Format(dashFormat, Mathf.Ceil(currentDashCooldown), maxDashCooldown);
    }*/
}
