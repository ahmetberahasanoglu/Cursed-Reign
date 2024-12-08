using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondLadder : MonoBehaviour
{
    Animator animator;
    PunchTrap trap;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        GameObject trapObject = GameObject.FindGameObjectWithTag("mami");
        GameObject trapObject2 = GameObject.FindGameObjectWithTag("yumruk");
        trap = trapObject.GetComponent<PunchTrap>();
     
        
    }
    private void Update()
    {
        if (trap.ladder_2 == 2)
        {
            animator.SetInteger("ladder_2", 2);

        }
        else if(trap.ladder_2 == 4)
        {
            animator.SetInteger("ladder_2", 4);
        }

    }

}
