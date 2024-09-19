using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] Vector2 speed = new Vector2(5, 0);
    public int damage = 10;
    public Vector2 knockback = Vector2.zero;
    Rigidbody2D rb;

    [SerializeField] private float lifetime = 3f; // Merminin yok olma s�resi

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Mermiyi h�zla hareket ettir
        rb.velocity = new Vector2(speed.x * transform.localScale.x, speed.y);

        // Belirli bir s�re sonra mermiyi yok et
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Damageable damageable = collision.collider.GetComponent<Damageable>();
        if (damageable != null)
        {
            Vector2 deliveredKnockback = transform.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);

            // Hedefi vur
            bool gotHit = damageable.Hit(damage, deliveredKnockback);

            if (gotHit)
                Debug.Log(collision.collider.name + " hit for " + damage);
        }

        // �arpt��� her durumda mermiyi yok et
        Destroy(gameObject);
    }
}
