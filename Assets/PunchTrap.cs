using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchTrap : MonoBehaviour
{
   Animator animator;
    public GameObject ladder;
    public bool punched = false;
    public int ladder_2 = 1;    
    audiomanager manager;
    void Awake()
    {
        animator = GetComponent<Animator>();
        manager = audiomanager.Instance;
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        animator.SetTrigger("punched");
        manager.PlaySFX(manager.punch, 0.5F);
        ladder.SetActive(true);
        punched = true;
        ladder_2 ++;
    }
}
