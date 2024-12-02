using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonInfo : MonoBehaviour
{
    public TMP_Text priceText;
    public TMP_Text descriptionText;


    public TMP_Text durumText;
    public Button purchaseButton;
    [SerializeField] GameObject skill;
    ProjectileLauncher projectileLauncher;
    audiomanager manager;

    public int itemPrice;
    [SerializeField] private float volume = 0.5f;


    public PHealthBar healthBar;
    public ManaBar manaBar;


    public int healthIncreaseAmount = 20;
    public int manaIncreaseAmount = 1;


    private Damageable damageable;

    void Start()
    {

        if ((SceneManager.GetActiveScene().name == "Door2") && itemPrice == 300)
        {
            descriptionText.text = "Yeni bir kýlýc teknigi kitabý";
        }

        else if ((SceneManager.GetActiveScene().name == "Door2") && itemPrice == 1)
        {
            descriptionText.text = "Kýlýc týlsýmý";
        }
        else if ((SceneManager.GetActiveScene().name == "Door2") && itemPrice == 180)
        {
            descriptionText.text = "Combo gorunumu";
        }
        else if ((SceneManager.GetActiveScene().name == "Door3") && itemPrice == 300)
        {
            descriptionText.text = "Yeni bir kýlýc teknigi kitabý";
        }

        else if ((SceneManager.GetActiveScene().name == "Door3") && itemPrice == 1)
        {
            descriptionText.text = "Kýlýc týlsýmý";
        }
        else if ((SceneManager.GetActiveScene().name == "Door3") && itemPrice == 180)
        {
            descriptionText.text = "Tembel geliþtiricci baska özellik bulamadý";
        }
        priceText.text = itemPrice.ToString();
        purchaseButton.onClick.AddListener(OnPurchaseButtonClicked);
        manager = audiomanager.Instance;


        projectileLauncher = FindObjectOfType<ProjectileLauncher>();


        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            damageable = player.GetComponent<Damageable>();
        }

        healthBar = FindObjectOfType<PHealthBar>();
        manaBar = FindObjectOfType<ManaBar>();
    }

    public void OnPurchaseButtonClicked()
    {

        if (descriptionText.text.Contains("kitap"))
        {
            if (gameManager.instance.timerCurrency <= itemPrice)
            {
                durumText.text = ("Ürün alýndý: " + descriptionText.text);
                purchaseButton.interactable = false;
                manager.PlaySFX(manager.BuySell, volume);

                if (descriptionText.text.Contains("kitap"))
                {
                    if (projectileLauncher != null)
                    {
                        projectileLauncher.projectilePrefab = skill;
                    }
                    else
                    {
                        Debug.LogError("ProjectileLauncher bulunamadý.");
                    }
                }
            }
            else
            {
                durumText.text = "Yeterince hýzlý olamadýn!";
                manager.PlaySFX(manager.denied, volume);
            }

        }
        else if (itemPrice == 1)
        {
            if (gameManager.instance.crownTaken == true)
            {
                durumText.text = ("Ürün alýndý: " + descriptionText.text);
                purchaseButton.interactable = false;
                manager.PlaySFX(manager.BuySell, volume);
                gameManager.instance.EmptyCrown();
                if (descriptionText.text.Contains("Mana"))
                {
                    IncreaseMaxMana();
                }
            }
            else
            {
                durumText.text = "Tacý bulamadýn!";
                manager.PlaySFX(manager.denied, volume);
            }
           
        }
        else
        {
            if (gameManager.instance.GetScore() >= itemPrice)
            {
                gameManager.instance.DeductScore(itemPrice);


                durumText.text = ("Ürün alýndý: " + descriptionText.text);
                purchaseButton.interactable = false;
                manager.PlaySFX(manager.BuySell, volume);


                if (descriptionText.text.Contains("can"))
                {
                    IncreaseMaxHealth();
                }

                else if (descriptionText.text.Contains("Yeni"))
                {
                    Debug.Log("Kýlýc tekngi degis");

                }
            }

            else
            {
                durumText.text = "Yeterli altýnýn yok!";
                manager.PlaySFX(manager.denied, volume);
            }
        }

    }


    private void IncreaseMaxHealth()
    {
        if (damageable != null)
        {

            damageable.MaxHealth += healthIncreaseAmount;


            damageable.Health = Mathf.Min(damageable.Health, damageable.MaxHealth);

            healthBar.healthSlider.value = damageable.Health / (float)damageable.MaxHealth;
            healthBar.UpdateHealthText(damageable.Health, damageable.MaxHealth);
        }
        else
        {
            Debug.LogError("Damageable component is missing on the player.");
        }
    }


    private void IncreaseMaxMana()
    {
        int maxMana = manaBar.GetMaxMana();

        maxMana += manaIncreaseAmount;
        manaBar.SetMaxMana(maxMana);
    }
}
