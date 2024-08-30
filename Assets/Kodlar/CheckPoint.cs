using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private RespawnScript respawn;
  //  private BoxCollider2D checkPointCol; Eski checkpointleri kapama seyi
    private void Awake()
    {
        respawn = GameObject.FindGameObjectWithTag("respawn").GetComponent<RespawnScript>();
        //  checkPointCol = GetComponent<BoxCollider2D>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            respawn.respawnPoint = this.gameObject;
            // checkPointCol.enabled = false;
        }
    }
}
