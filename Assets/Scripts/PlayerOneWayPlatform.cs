using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class PlayerOneWayPlatform : MonoBehaviour
{
    
    private GameObject currentOneWayPlatform;
   // public Button downButton;
    
   
    [SerializeField] private CapsuleCollider2D playerCollider;
    private FixedJoystick fixedJoystick;

    private void Start()
    {
      //  downButton= GameObject.Find("DownButton").GetComponent<Button>();
        fixedJoystick = GameObject.Find("Fixed Joystick").GetComponent<FixedJoystick>();
      //  downButton.onClick.AddListener(onDownButtonPressed);

    }
    public void onDownButtonPressed()
    {
        if (currentOneWayPlatform != null)
        {
           
            StartCoroutine(DisableCollisiion());
        }
    }
    private void Update()
    {
       
        if (fixedJoystick != null && fixedJoystick.Vertical < -0.5f) 
        {
            onDownButtonPressed();
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
            // currentOneWayPlatform null yapýlýr (yani artýk oyuncu platformun üzerinde deðil)
            currentOneWayPlatform = null;
        }
    }

  
    private IEnumerator DisableCollisiion()
    {

        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(playerCollider, platformCollider);

        yield return new WaitForSeconds(0.4f);

        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);

        yield return new WaitForSeconds(0.25f);
    }
}
