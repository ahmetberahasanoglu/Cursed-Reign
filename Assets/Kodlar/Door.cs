using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    
    Animator animator;
    bool isOpen=false;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void Open()
    {
        animator.SetTrigger("Open");
        isOpen = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&&isOpen )
        {
            gameManager.instance.NextLevel();
        }
    }
}
