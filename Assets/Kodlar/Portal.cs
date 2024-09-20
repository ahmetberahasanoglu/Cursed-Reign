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
        // AudioManager instance kontrolü
        manager = audiomanager.Instance;
        if (manager == null)
        {
            Debug.LogError("AudioManager instance bulunamadý!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ayný nesne tekrar portaldan geçmemesi için kontrol
        if (portalObjects.Contains(collision.gameObject))
        {
            return;
        }

        // Mermiyi portaldan geçirme
        if (collision.CompareTag("Projectile"))
        {
            TeleportProjectile(collision);
            return;
        }

        // Diðer nesneler için portal iþlemi
        if (destination.TryGetComponent(out Portal destinationPortal))
        {
            destinationPortal.portalObjects.Add(collision.gameObject);
        }

        // Nesnenin pozisyonunu hedef portala taþýma
        collision.transform.position = destination.position;

        // AudioManager üzerinden portal sesini oynatma
        if (manager != null)
        {
            manager.PlaySFX(manager.portal, volume);
        }
        else
        {
            Debug.LogWarning("AudioManager instance eksik, ses çalýnamadý.");
        }
    }

    private void TeleportProjectile(Collider2D collision)
    {
        // Mermiyi portaldan geçiriyoruz
        Projectile projectile = collision.GetComponent<Projectile>();
        if (projectile != null)
        {
            Vector2 entryVelocity = projectile.GetComponent<Rigidbody2D>().velocity;

            // Merminin pozisyonunu hedef portala taþýma
            collision.transform.position = destination.position;

            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();

            // Merminin hýzýný ve yönünü koruyarak yeniden hýzlandýrma
            rb.velocity = entryVelocity;

            // Çift portaldan geçmesini önlemek için hedef portala ekleme
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
