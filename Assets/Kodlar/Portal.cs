using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private HashSet<GameObject> portalObjects = new HashSet<GameObject>();
    [SerializeField] private Transform destination;
    audiomanager manager;
    [SerializeField] float volume = 0.5f;
    [SerializeField] private AudioSource portalAudioSource;
    private void Start()
    {
        manager = audiomanager.Instance;
        if (manager == null)
        {
            Debug.LogError("AudioManager instance bulunamadý!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (portalObjects.Contains(collision.gameObject))
        {
            return;
        }

       
        if (collision.CompareTag("Projectile"))
        {
            TeleportProjectile(collision);
            return;
        }

        // Diðer nesneler için portal 
        if (destination.TryGetComponent(out Portal destinationPortal))
        {
            destinationPortal.portalObjects.Add(collision.gameObject);
        }

        collision.transform.position = destination.position;
        if (portalAudioSource != null)
        {
            portalAudioSource.PlayOneShot(manager.portal, volume);
        }
        else
        {
            Debug.LogWarning("Portal AudioSource eksik, ses çalýnamadý.");
        }
    }

    private void TeleportProjectile(Collider2D collision)
    {
        // Projeyi portaldan geçiriyoruz
        Projectile projectile = collision.GetComponent<Projectile>();
        if (projectile != null)
        {
            Vector2 entryVelocity = projectile.GetComponent<Rigidbody2D>().velocity;

            // Portaldan geçtiði anki pozisyonunu hedef portala taþý
            collision.transform.position = destination.position;

            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();

            // Projeye yönünü ve hýzýný koruyarak yeniden hýz veriyoruz
            rb.velocity = entryVelocity;

            // Çift portaldan geçmesini önlemek için hedef portala ekliyoruz
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
