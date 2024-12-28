using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();


    }

    public void SetBossAnimatorBool(bool value)
    {

        animator.SetBool("BossReach", value);
    }
}
