using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnScript : MonoBehaviour
{
    public GameObject player;
    public GameObject respawnPoint;
    public int healthPenalty = 10;  // Yeniden do�ma s�ras�nda kaybedilecek can miktar�
    public float respawnDelay = 1f; // Yeniden do�ma gecikmesi (saniye cinsinden)

   
    public AudioClip respawnSound;   // Yeniden do�ma sesi
    public float soundVolume = 1f;   // Sesin y�ksekli�i

    private Damageable damageable; // Oyuncunun can sistemi
    private bool isRespawning = false; // Yeniden do�ma i�lemi devam ediyor mu?

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

        // Yeniden do�madan �nce belirli bir s�re bekleyin
        yield return new WaitForSeconds(respawnDelay);

        // Can azaltma i�lemi
        if (damageable != null)
        {
            if (damageable.IsAlive)
            {
                // Oyuncu hen�z �lmemi�se can�n� azalt
                damageable.Health -= healthPenalty;
            }
            else
            {
                // Oyuncu �lm��se can�n� tamamen yenile
                damageable.Health = damageable.MaxHealth;
                damageable.IsAlive = true;
            }
        }

        // Oyuncunun konumunu yeniden do�ma noktas�na ta��
        player.transform.position = respawnPoint.transform.position;

       

        // Ses efekti �al
        if (respawnSound != null)
        {
            AudioSource.PlayClipAtPoint(respawnSound, player.transform.position, soundVolume);
        }

        isRespawning = false;
    }
}

