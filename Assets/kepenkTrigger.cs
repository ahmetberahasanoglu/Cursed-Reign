using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kepenkTrigger : MonoBehaviour
{
    Animator m_Animator;
    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_Animator.SetTrigger("acil");
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        m_Animator.SetTrigger("kapan");
    }
}
