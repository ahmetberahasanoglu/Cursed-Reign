using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Color low;
    public Color high;
    public Vector3 offset;
  
    public void SetHealth(float health,float maxHealth)
    {
        slider.gameObject.SetActive(health<maxHealth);//hasar almadýgýmýz sürece slider aktif olmayacak
        slider.value = health;
        slider.maxValue = maxHealth;
        slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, slider.normalizedValue);//normalizedvalue 1-0 arasý bir deger
    }
    void Update()
    {
        slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offset)
;    }
}
