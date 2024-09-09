using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField] float volume = 0.5f;
    audiomanager manager;
    Animator animator;
    bool isOpen=false;
    private void Awake()
    {
        animator = GetComponent<Animator>();
       
    }
    private void Start()
    {
        manager = audiomanager.Instance;
        if (manager == null)
        {
            Debug.LogError("AudioManager instance bulunamadý!");
        }
    }
    public void Open()
    {
        animator.SetTrigger("Open");
        isOpen = true;
        manager.PlaySFX(manager.doorOpen,volume);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&&isOpen )
        {
            gameManager.instance.NextLevel();
        }
    }
}
