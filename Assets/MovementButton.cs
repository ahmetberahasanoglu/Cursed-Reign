using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementButton : MonoBehaviour
{
   private PlayerMovement playerMovement;
    void Start()
    {
        FindPlayerMovement();

    }
    private void FindPlayerMovement()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
        }
      
    }
    public void OnJoystick()
    {
      playerMovement.SwitchControlJoystick();
    }
    public void OnButton()
    {
       playerMovement.SwitchControlButton();
    }
   
    void Update()
    {
        if (playerMovement == null)
        {
            FindPlayerMovement();
        }
    }
}
