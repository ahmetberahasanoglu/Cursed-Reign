using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinCollect : Collectable
{
    [SerializeField] float volume = 0.5f;
    audiomanager manager;
    private void Start()
    {
        manager = audiomanager.Instance;
        if (manager == null)
        {
            Debug.LogError("AudioManager instance bulunamadý!");
        }
    }
    public override void OnCollect()
    {
        ActionsListener.OnCoinCollected();
        manager.PlaySFX(manager.coinPickup, volume);
    }
}
