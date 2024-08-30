using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnScript : MonoBehaviour
{
    public GameObject player;
    public GameObject respawnPoint;
    public int healthPenalty = 10;  // Yeniden doðma sýrasýnda kaybedilecek can miktarý
    public float respawnDelay = 1f; // Yeniden doðma gecikmesi (saniye cinsinden)

   
    public AudioClip respawnSound;   // Yeniden doðma sesi
    public float soundVolume = 1f;   // Sesin yüksekliði

    private Damageable damageable; // Oyuncunun can sistemi
    private bool isRespawning = false; // Yeniden doðma iþlemi devam ediyor mu?

    private void Awake()
    {
        damageable = player.GetComponent<Damageable>();
    }

    private void FixedUpdate()
    {
        if (!damageable.IsAlive && !isRespawning)
        {
            StartCoroutine(Respawn());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isRespawning)
        {
            StartCoroutine(Respawn());
        }
    }

    private IEnumerator Respawn()
    {
        isRespawning = true;

        // Yeniden doðmadan önce belirli bir süre bekleyin
        yield return new WaitForSeconds(respawnDelay);

        // Can azaltma iþlemi
        if (damageable != null)
        {
            if (damageable.IsAlive)
            {
                // Oyuncu henüz ölmemiþse canýný azalt
                damageable.Health -= healthPenalty;
            }
            else
            {
                // Oyuncu ölmüþse canýný tamamen yenile
                damageable.Health = damageable.MaxHealth;
                damageable.IsAlive = true;
            }
        }

        // Oyuncunun konumunu yeniden doðma noktasýna taþý
        player.transform.position = respawnPoint.transform.position;

       

        // Ses efekti çal
        if (respawnSound != null)
        {
            AudioSource.PlayClipAtPoint(respawnSound, player.transform.position, soundVolume);
        }

        isRespawning = false;
    }
}

