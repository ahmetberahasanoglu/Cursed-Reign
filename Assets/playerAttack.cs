using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAttack : MonoBehaviour
{

    public int attackDamage = 10;
    public Vector2 knockback = Vector2.zero;
    audiomanager manager;
    public bool dusmanaVurdu = false;
    private PolygonCollider2D col;

    private bool sesCaldi = false;

    private void Start()
    {
        manager = audiomanager.Instance;
        col = GetComponent<PolygonCollider2D>();
    }
    public bool triggerTetiklendi = false;
    private void Update()
    {
        if (col.enabled && !sesCaldi && triggerTetiklendi)
        {

            onAttack();
            sesCaldi = true;
            triggerTetiklendi = false;

        }

        if (!col.enabled)
        {
          
            dusmanaVurdu = false;
            sesCaldi = false;
            triggerTetiklendi = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable != null && !dusmanaVurdu)
        {
            dusmanaVurdu = true;
            triggerTetiklendi = true;
            Vector2 deliveredKnockback = transform.parent.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);
            bool gotHit = damageable.Hit(attackDamage, deliveredKnockback);
        }
    }
    private void onAttack()
    {
       
        if (dusmanaVurdu)
        {
            manager.PlaySFX(manager.swordHit, 0.19f);
            
        }
        else
        {
            manager.PlaySFX(manager.attack, 0.2f);

        }
    }


}
