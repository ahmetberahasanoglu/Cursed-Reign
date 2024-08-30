using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionDetection : MonoBehaviour
{
    public string collisionScript; 

    [SerializeField]
    private UnityEvent _collisionEntered;
    [SerializeField]
    private UnityEvent _collisionExit;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent(collisionScript))
        {
            _collisionEntered?.Invoke();
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
