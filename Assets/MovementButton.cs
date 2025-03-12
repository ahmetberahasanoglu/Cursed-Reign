using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementButton : MonoBehaviour
{
   private PlayerMovement gameObject;
    void Start()
    {
        gameObject=GameObject.Find("Player").GetComponent<PlayerMovement>();   
    }

    public void OnJoystick()
    {
        gameObject.SwitchControlJoystick();
    }
    public void OnButton()
    {
        gameObject.SwitchControlButton();
    }
    public void OnBack()
    {
        gameObject.OnCloseButtonPressed();
    }
   
}
