using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOneWayPlatform : MonoBehaviour
{
    private GameObject currentOneWayPlatform;
    [SerializeField] private CapsuleCollider2D playerCollider;
    PlayerMovement playerMovement;
    private Joystick Joystick;
    private Button downButton;

    private void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
        Joystick = playerMovement.fixedJoystick;
        downButton = playerMovement.downB;
        downButton.onClick.AddListener(onDownButtonPressed);
    }

    private void Update()
    {
       
            if (Joystick != null && Joystick.Vertical < -0.5f)
            {
                onDownButtonPressed();
            }
        
      
    }

    public void onDownButtonPressed()
    {
        if (currentOneWayPlatform != null)
        {
            StartCoroutine(DisableCollision());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = null;
        }
    }

    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(0.4f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
        yield return new WaitForSeconds(0.25f);
    }

  
}
