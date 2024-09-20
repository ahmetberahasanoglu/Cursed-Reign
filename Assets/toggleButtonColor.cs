using UnityEngine;
using UnityEngine.UI;

public class ToggleButtonColor : MonoBehaviour
{
    public Image buttonImage; 
    public Color color1 = Color.green; 
    public Color color2 = Color.red; 

    private bool isColor1 = false; 

    private void Start()
    {
       
        buttonImage.color = color1;
    }

    public void ToggleColor()
    {
      
        if (isColor1)
        {
            buttonImage.color = color1; 
        }
        else
        {
            buttonImage.color = color2; 
        }

       
        isColor1 = !isColor1;
    }
}
