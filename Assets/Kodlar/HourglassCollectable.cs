using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HourglassCollectable : Collectable
{
    [SerializeField] float volume = 0.5f;
    Rigidbody2D rb;
    [SerializeField] float dropForce = 5f;
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
      ActionsListener.OnHourglassCollected();
        manager.PlaySFX(manager.coinPickup, volume);
    }

   
}
