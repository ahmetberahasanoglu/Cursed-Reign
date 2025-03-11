using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CollisionDetection : MonoBehaviour
{
    public string collisionScript; 
    audiomanager manager;
    [SerializeField] float volume=0.6f;
    [SerializeField]
    private UnityEvent _collisionEntered;
    [SerializeField]
    private UnityEvent _collisionExit;

    private void Start()
    {
        manager = audiomanager.Instance;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent(collisionScript))
        {
            _collisionEntered?.Invoke();
            manager.PlaySFX(manager.coinPickup,volume);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent(collisionScript))
        {
            _collisionExit?.Invoke();
        }
    }
}
