using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crownCollect : Collectable
{
    [SerializeField] float volume = 0.5f;
    [SerializeField] float dropForce = 5f;
    Rigidbody2D rb;
    audiomanager manager;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.up * dropForce, ForceMode2D.Impulse);
        manager = audiomanager.Instance;
        if (manager == null)
        {
            Debug.LogError("AudioManager instance bulunamadý!");
        }
    }
    public override void OnCollect()
    {
        ActionsListener.OnCrownCollected();
        manager.PlaySFX(manager.coinPickup, volume);
    }

}
