using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private HashSet<GameObject> portalObjects = new HashSet<GameObject>();
    [SerializeField] private Transform destination;
    audiomanager manager;
    [SerializeField] float volume = 0.5f; // SFX volume

    private void Start()
    {
        // AudioManager instance kontrol�
        manager = audiomanager.Instance;
        if (manager == null)
        {
            Debug.LogError("AudioManager instance bulunamad�!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ayn� nesne tekrar portaldan ge�memesi i�in kontrol
        if (portalObjects.Contains(collision.gameObject))
        {
            return;
        }

        // Mermiyi portaldan ge�irme
        if (collision.CompareTag("Projectile"))
        {
            TeleportProjectile(collision);
            return;
        }

        // Di�er nesneler i�in portal i�lemi
        if (destination.TryGetComponent(out Portal destinationPortal))
        {
            destinationPortal.portalObjects.Add(collision.gameObject);
        }

        // Nesnenin pozisyonunu hedef portala ta��ma
        collision.transform.position = destination.position;

        // AudioManager �zerinden portal sesini oynatma
        if (manager != null)
        {
            manager.PlaySFX(manager.portal, volume);
        }
        else
        {
            Debug.LogWarning("AudioManager instance eksik, ses �al�namad�.");
        }
    }

    private void TeleportProjectile(Collider2D collision)
    {
        // Mermiyi portaldan ge�iriyoruz
        Projectile projectile = collision.GetComponent<Projectile>();
        if (projectile != null)
        {
            Vector2 entryVelocity = projectile.GetComponent<Rigidbody2D>().velocity;

            // Merminin pozisyonunu hedef portala ta��ma
            collision.transform.position = destination.position;

            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();

            // Merminin h�z�n� ve y�n�n� koruyarak yeniden h�zland�rma
            rb.velocity = entryVelocity;

            // �ift portaldan ge�mesini �nlemek i�in hedef portala ekleme
            if (destination.TryGetComponent(out Portal destinationPortal))
            {
                destinationPortal.portalObjects.Add(collision.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        portalObjects.Remove(collision.gameObject);
    }
}
