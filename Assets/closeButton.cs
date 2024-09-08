using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class closeButton : MonoBehaviour
{
    public GameObject scrollView;
    public Animator animator; 
    public GameObject shopCanvas;

    void Start()
    {
     
        if (scrollView != null)
        {
            animator = scrollView.GetComponent<Animator>();
        }
    }

    // Butona t�klanma olay�
    public void OnCloseButtonClicked()
    {
        if (animator != null)
        {
            animator.Play("shopanim-1"); 
            StartCoroutine(CloseShopAfterAnimation()); 
        }
        else
        {
            Debug.LogError("Animator bile�eni bulunamad�.");
        }
    }

    
    IEnumerator CloseShopAfterAnimation()
    {
   
        yield return new WaitForSeconds(2f);

        shopCanvas.SetActive(false);
    }
}
