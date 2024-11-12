using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnScript : MonoBehaviour
{
    private GameObject player;
    public GameObject respawnPoint;
    public int healthPenalty = 10;  
    public float respawnDelay = 1f; 
audiomanager manager;
  
    public float soundVolume = 1f;  

    private Damageable damageable; 
    private bool isRespawning = false; // Yeniden doðma iþlemi devam ediyor mu?

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        damageable = player.GetComponent<Damageable>();
    }
    private void Start()
    {
       
        manager = audiomanager.Instance;
     
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

  
        yield return new WaitForSeconds(respawnDelay);

        // Can azaltma iþlemi
        if (damageable != null)
        {
            if (damageable.IsAlive)
            {
                
                damageable.Health -= healthPenalty;
            }
            else
            {
                
                damageable.Health = damageable.MaxHealth;
                damageable.IsAlive = true;
            }
        }

        player.transform.position = respawnPoint.transform.position;


        manager.PlaySFX(manager.checkPoint, soundVolume);

        isRespawning = false;
    }
}

