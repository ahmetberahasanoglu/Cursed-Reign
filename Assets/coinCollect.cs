using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinCollect : Collectable
{
    [SerializeField] float volume = 0.5f;
    audiomanager manager;
    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
        manager = audiomanager.Instance;
        if (manager == null)
        {
            Debug.LogError("AudioManager instance bulunamadư!");
        }
    }
    public override void OnCollect()
    {
        ActionsListener.OnCoinCollected();
        manager.PlaySFX(manager.coinPickup, volume);
        animator.SetTrigger("Collected");
    }
    
}
