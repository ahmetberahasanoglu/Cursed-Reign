using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingAnim : MonoBehaviour
{
    public Volume global;
    private WhiteBalance whiteBalance; // WhiteBalance bileþenini sýnýf seviyesinde tanýmla
    private float val = -100f; // float olarak tanýmlandý
    private float changeRate = 20f; 

    private void Start()
    {
        // WhiteBalance bileþenine eriþimi burada ayarla
        global.profile.TryGet(out whiteBalance);
    }

    void FixedUpdate()
    {
        // Eðer WhiteBalance bileþenine ulaþýlamadýysa, burada iþlem yapma
        if (whiteBalance == null) return;

        // Sýcaklýk deðerini sürekli artýr veya azalt
        if (val >= 100f)
        {
            changeRate = -Mathf.Abs(changeRate); // 100'e ulaþtýysa azaltmaya baþla
        }
        else if (val <= -100f)
        {
            changeRate = Mathf.Abs(changeRate); // -100'e ulaþtýysa artýrmaya baþla
        }

        // Deðeri `Time.fixedDeltaTime` ile zamana baðlý olarak deðiþtir
        val += changeRate * Time.fixedDeltaTime;
        whiteBalance.temperature.value = val;
    }
}
