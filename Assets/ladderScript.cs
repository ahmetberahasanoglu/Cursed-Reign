using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ladderScript : MonoBehaviour
{
    Animator animator;
    PunchTrap trap;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        trap = FindObjectOfType<PunchTrap>();
    }
    private void Update()
    {
        if(trap.punched==true)
        {
            animator.SetTrigger("tapped");

        }
        
    }
   
}
