using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class PlayerOneWayPlatform : MonoBehaviour
{
    // Oyuncunun üzerinde durduðu mevcut tek yönlü platformu tutar
    private GameObject currentOneWayPlatform;
    public Button downButton;
    // Oyuncunun CapsuleCollider2D bileþeni
    [SerializeField] private CapsuleCollider2D playerCollider;

    private void Start()
    {
        downButton.onClick.AddListener(onDownButtonPressed);
    }
    public void onDownButtonPressed()
    {
        if (currentOneWayPlatform != null)
        {
            // Çarpýþmayý geçici olarak devre dýþý býrakmak için Coroutine baþlatýlýr
            StartCoroutine(DisableCollisiion());
        }
    }
   /* private void Update()
    {
        // Eðer oyuncu aþaðý ok tuþuna basarsa
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // Ve eðer oyuncu bir tek yönlü platformun üzerindeyse
            if (currentOneWayPlatform != null)
            {
                // Çarpýþmayý geçici olarak devre dýþý býrakmak için Coroutine baþlatýlýr
                StartCoroutine(DisableCollisiion());
            }
        }
    }
   */
    // Oyuncu bir nesneyle çarpýþtýðýnda çalýþýr
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Eðer çarpýþýlan nesne "OneWayPlatform" tag'ine sahipse
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            // Bu nesne currentOneWayPlatform olarak atanýr
            currentOneWayPlatform = collision.gameObject;
        }
    }

    // Oyuncu bir nesneyle çarpýþmayý býraktýðýnda çalýþýr
    private void OnCollisionExit2D(Collision2D collision)
    {
        // Eðer ayrýlýnan nesne "OneWayPlatform" tag'ine sahipse
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            // currentOneWayPlatform null yapýlýr (yani artýk oyuncu platformun üzerinde deðil)
            currentOneWayPlatform = null;
        }
    }

    // Çarpýþmayý geçici olarak devre dýþý býrakmak için Coroutine
    private IEnumerator DisableCollisiion()
    {
        // Mevcut tek yönlü platformun BoxCollider2D bileþenini alýr
        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();

        // Oyuncu ile platform arasýndaki çarpýþmayý devre dýþý býrakýr
        Physics2D.IgnoreCollision(playerCollider, platformCollider);

        // 0.4 saniye bekler (bu süre boyunca oyuncu platformdan geçebilir)
        yield return new WaitForSeconds(0.4f);

        // Çarpýþmayý yeniden aktif eder
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);

        // 0.25 saniye daha bekler (bu süre boyunca çarpýþma etkin kalýr)
        yield return new WaitForSeconds(0.25f);
    }
}
