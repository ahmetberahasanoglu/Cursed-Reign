using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Pointer eventleri i�in gerekli

public class ButtonEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image targetImage;
    [SerializeField] private float maxAlpha = 1f; // Butona bas�l�yken alpha
    private float originalAlpha;

    private void Start()
    {
        if (targetImage != null)
        {
            originalAlpha = targetImage.color.a; // Ba�lang�� alpha de�erini kaydet
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Butona bas�ld���nda alpha'y� art�r
        if (targetImage != null)
        {
            Color color = targetImage.color;
            color.a = maxAlpha;
            targetImage.color = color;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Buton b�rak�ld���nda eski alpha'ya d�n
        if (targetImage != null)
        {
            Color color = targetImage.color;
            color.a = originalAlpha;
            targetImage.color = color;
        }
    }
}
