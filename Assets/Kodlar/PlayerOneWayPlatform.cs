using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class PlayerOneWayPlatform : MonoBehaviour
{
    // Oyuncunun �zerinde durdu�u mevcut tek y�nl� platformu tutar
    private GameObject currentOneWayPlatform;
    public Button downButton;
    // Oyuncunun CapsuleCollider2D bile�eni
    [SerializeField] private CapsuleCollider2D playerCollider;

    private void Start()
    {
        downButton.onClick.AddListener(onDownButtonPressed);
    }
    public void onDownButtonPressed()
    {
        if (currentOneWayPlatform != null)
        {
            // �arp��may� ge�ici olarak devre d��� b�rakmak i�in Coroutine ba�lat�l�r
            StartCoroutine(DisableCollisiion());
        }
    }
   /* private void Update()
    {
        // E�er oyuncu a�a�� ok tu�una basarsa
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // Ve e�er oyuncu bir tek y�nl� platformun �zerindeyse
            if (currentOneWayPlatform != null)
            {
                // �arp��may� ge�ici olarak devre d��� b�rakmak i�in Coroutine ba�lat�l�r
                StartCoroutine(DisableCollisiion());
            }
        }
    }
   */
    // Oyuncu bir nesneyle �arp��t���nda �al���r
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // E�er �arp���lan nesne "OneWayPlatform" tag'ine sahipse
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            // Bu nesne currentOneWayPlatform olarak atan�r
            currentOneWayPlatform = collision.gameObject;
        }
    }

    // Oyuncu bir nesneyle �arp��may� b�rakt���nda �al���r
    private void OnCollisionExit2D(Collision2D collision)
    {
        // E�er ayr�l�nan nesne "OneWayPlatform" tag'ine sahipse
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            // currentOneWayPlatform null yap�l�r (yani art�k oyuncu platformun �zerinde de�il)
            currentOneWayPlatform = null;
        }
    }

    // �arp��may� ge�ici olarak devre d��� b�rakmak i�in Coroutine
    private IEnumerator DisableCollisiion()
    {
        // Mevcut tek y�nl� platformun BoxCollider2D bile�enini al�r
        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();

        // Oyuncu ile platform aras�ndaki �arp��may� devre d��� b�rak�r
        Physics2D.IgnoreCollision(playerCollider, platformCollider);

        // 0.4 saniye bekler (bu s�re boyunca oyuncu platformdan ge�ebilir)
        yield return new WaitForSeconds(0.4f);

        // �arp��may� yeniden aktif eder
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);

        // 0.25 saniye daha bekler (bu s�re boyunca �arp��ma etkin kal�r)
        yield return new WaitForSeconds(0.25f);
    }
}
