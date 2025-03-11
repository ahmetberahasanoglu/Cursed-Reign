using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private RespawnScript[] respawnScripts;
    //  private BoxCollider2D checkPointCol; Eski checkpointleri kapama seyi
    private void Awake()
    {
        // Tüm respawn noktalarýný bul ve onlarýn RespawnScript bileþenlerini al
        //  respawn = GameObject.FindGameObjectWithTag("respawn").GetComponent<RespawnScript>();
        GameObject[] respawnPoints = GameObject.FindGameObjectsWithTag("respawn");
        respawnScripts = new RespawnScript[respawnPoints.Length];
        for (int i = 0; i < respawnPoints.Length; i++)
        {
            respawnScripts[i] = respawnPoints[i].GetComponent<RespawnScript>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       /* if (collision.gameObject.CompareTag("Player"))
        {
            respawn.respawnPoint = this.gameObject;
            // checkPointCol.enabled = false;
        }*/
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach (RespawnScript respawn in respawnScripts)
            {
                if (respawn != null)
                {
                    respawn.respawnPoint = this.gameObject;
                }
                else
                {
                    Debug.LogError("Bir 'RespawnScript' bulunamadý!");
                }
            }
        }
    }
}



