using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingAnim : MonoBehaviour
{
    public Volume global;
    private WhiteBalance whiteBalance; // WhiteBalance bile�enini s�n�f seviyesinde tan�mla
    private float val = -100f; // float olarak tan�mland�
    private float changeRate = 20f; 

    private void Start()
    {
        // WhiteBalance bile�enine eri�imi burada ayarla
        global.profile.TryGet(out whiteBalance);
    }

    void FixedUpdate()
    {
        // E�er WhiteBalance bile�enine ula��lamad�ysa, burada i�lem yapma
        if (whiteBalance == null) return;

        // S�cakl�k de�erini s�rekli art�r veya azalt
        if (val >= 100f)
        {
            changeRate = -Mathf.Abs(changeRate); // 100'e ula�t�ysa azaltmaya ba�la
        }
        else if (val <= -100f)
        {
            changeRate = Mathf.Abs(changeRate); // -100'e ula�t�ysa art�rmaya ba�la
        }

        // De�eri `Time.fixedDeltaTime` ile zamana ba�l� olarak de�i�tir
        val += changeRate * Time.fixedDeltaTime;
        whiteBalance.temperature.value = val;
    }
}
