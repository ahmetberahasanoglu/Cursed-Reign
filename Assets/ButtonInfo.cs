using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ButtonInfo : MonoBehaviour
{
    public TMP_Text priceText;
    public TMP_Text descriptionText;
    public TMP_Text durumText;
    public Button purchaseButton;
    public int itemPrice;

    void Start()
    {
        // Fiyat� priceText'e yazd�r
        priceText.text = itemPrice.ToString();

        // Sat�n alma butonuna t�klama i�lemi ekle
        purchaseButton.onClick.AddListener(OnPurchaseButtonClicked);
    }

   public void OnPurchaseButtonClicked()
    {
        // Yeterli skora sahipsen �r�n� sat�n al, de�ilse uyar� ver
        if (gameManager.instance.GetScore() >= itemPrice)
        {
            gameManager.instance.DeductScore(itemPrice);
           
            durumText.text=("�r�n al�nd�: " + descriptionText.text);
            purchaseButton.interactable = false; // �r�n al�nd���nda butonu devre d��� b�rak
        }
        else
        {
            durumText.text = "Yeterli skor yok!";
        }
    }


}
