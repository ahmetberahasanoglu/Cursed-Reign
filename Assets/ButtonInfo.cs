using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ButtonInfo : MonoBehaviour
{
    public TMP_Text priceText;
    public TMP_Text descriptionText;
    public Button purchaseButton;
    public int itemPrice;

    void Start()
    {
        // Fiyatý priceText'e yazdýr
        priceText.text = itemPrice.ToString();

        // Satýn alma butonuna týklama iþlemi ekle
        purchaseButton.onClick.AddListener(OnPurchaseButtonClicked);
    }

   public void OnPurchaseButtonClicked()
    {
        // Yeterli skora sahipsen ürünü satýn al, deðilse uyarý ver
        if (gameManager.instance.GetScore() >= itemPrice)
        {
            gameManager.instance.DeductScore(itemPrice);
            Debug.Log("Ürün alýndý: " + descriptionText.text);
            purchaseButton.interactable = false; // Ürün alýndýðýnda butonu devre dýþý býrak
        }
        else
        {
            Debug.Log("Yeterli skor yok!");
        }
    }


}
