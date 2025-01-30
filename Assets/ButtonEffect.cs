using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Pointer eventleri için gerekli

public class ButtonEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image targetImage;
    [SerializeField] private float maxAlpha = 1f; // Butona basýlýyken alpha
    private float originalAlpha;

    private void Start()
    {
        if (targetImage != null)
        {
            originalAlpha = targetImage.color.a; // Baþlangýç alpha deðerini kaydet
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Butona basýldýðýnda alpha'yý artýr
        if (targetImage != null)
        {
            Color color = targetImage.color;
            color.a = maxAlpha;
            targetImage.color = color;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Buton býrakýldýðýnda eski alpha'ya dön
        if (targetImage != null)
        {
            Color color = targetImage.color;
            color.a = originalAlpha;
            targetImage.color = color;
        }
    }
}
