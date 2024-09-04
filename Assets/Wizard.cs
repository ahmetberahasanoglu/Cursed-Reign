using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchDirection))]
public class Wizard : MonoBehaviour
{
    #region Public degiskenler
    public HealthBar healthBar;
    public DetectionZone attackZone;
    public DetectionZone cliffDetection;
    public float moveSpeed;
    public float timer;
    #endregion

    #region Privates
    private GameObject target;
    private Animator anim;
    private TouchDirection touchDirection;
    private Damageable damageable;
    private Rigidbody2D rb;
    #endregion

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        touchDirection = GetComponent<TouchDirection>();
        damageable = GetComponent<Damageable>();  // damageable'ý burada baþlatýyoruz.
        healthBar.SetHealth(damageable.Health, damageable.MaxHealth);
    }

    private void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;
        healthBar.SetHealth(damageable.Health, damageable.MaxHealth);
    }

    private bool hasTarget = false;

    public bool HasTarget
    {
        get
        {
            return hasTarget;
        }
        private set
        {
            hasTarget = value;
            anim.SetBool("hasTarget", value);
        }
    }

    public bool CanMove
    {
        get
        {
            return anim.GetBool("canMove");
        }
    }

    private void FixedUpdate()
    {
        if (touchDirection.IsOnWall && touchDirection.IsGrounded)
        {
            FlipDirection();
        }

        if (!damageable.IsHit && touchDirection.IsGrounded)
        {
            Move();
        }
    }

    private void Move()
    {
        anim.SetBool("canMove", true);
        if (target != null && !anim.GetCurrentAnimatorStateInfo(0).IsName("hasTarget"))
        {
            Vector2 targetPosition = new Vector2(target.transform.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            target = collision.gameObject;
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    public void OnCliffDetected()
    {
        if (touchDirection.IsGrounded)
        {
            FlipDirection();
        }
    }

    private void FlipDirection()
    {
        // Karakterin yönünü deðiþtir
        transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, transform.localScale.y);
    }
}
