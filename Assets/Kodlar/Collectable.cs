using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class  Collectable : MonoBehaviour
{
    public abstract void OnCollect();
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnCollect();
            Destroy(gameObject);
        }
    }
    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
           
        }
    }*/
}
