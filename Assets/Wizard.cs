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
    public DetectionZone trackDetection;
    public float moveSpeed;
    public float rayDistance; // Raycast uzakl���n� belirlemek i�in
    public LayerMask playerLayer; // Sadece player layer'� ile �arp��acak raycast
    #endregion

    #region Privates
    private GameObject target;
    private Animator anim;
    private TouchDirection touchDirection;
    private Damageable damageable;
    private Rigidbody2D rb;
    private bool inArea = false;
    private Vector2 rayDirection; // Raycast y�n�n� saklamak i�in
    #endregion

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        touchDirection = GetComponent<TouchDirection>();
        damageable = GetComponent<Damageable>();  // damageable'� burada ba�lat�yoruz.
        healthBar.SetHealth(damageable.Health, damageable.MaxHealth);
    }

    private void Update() 
    {
        HasTarget = attackZone.detectedColliders.Count > 0;
        if (trackDetection.detectedColliders.Count > 0)
        {
            target = trackDetection.detectedColliders[0].gameObject;
        }
        else
        {
            target = null;
        }
        inArea = target != null;
        if (AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }
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
    public float AttackCooldown
    {
        get
        {
            return anim.GetFloat(AnimStrings.attackCooldown);
        }
        set
        {
            anim.SetFloat(AnimStrings.attackCooldown, Mathf.Max(value, 0));
        }
    }

    private void FixedUpdate()
    {
        if (touchDirection.IsOnWall && touchDirection.IsGrounded)
        {
            FlipDirection();
        }

        if (!damageable.IsHit && touchDirection.IsGrounded && inArea)
        {
            PerformRaycast(); // Raycast'i �a��r�yoruz.
        }
    }

    private void PerformRaycast()
    {
        rayDirection = target.transform.position.x < transform.position.x ? Vector2.left : Vector2.right;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, rayDistance, playerLayer);

        if (hit.collider != null && hit.collider.gameObject.CompareTag("Player"))
        {
            // Hedefe do�ru hareket et
            Move(hit.collider.gameObject.transform.position);
        }
    }

    private void Move(Vector2 targetPosition)
    {
        if (target != null && !anim.GetCurrentAnimatorStateInfo(0).IsName("hasTarget"))
        {
            anim.SetBool("canMove", true);

            // Hedefin sa��nda m� solunda m� oldu�unu kontrol et
            if ((targetPosition.x < transform.position.x && transform.localScale.x > 0) ||
                (targetPosition.x > transform.position.x && transform.localScale.x < 0))
            {
                // Karakterin y�n�n� hedefe do�ru d�nd�r
                FlipDirection();
            }

            // Hedefin konumuna do�ru hareket et
            Vector2 newPosition = new Vector2(targetPosition.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);
        }
        else
        {
            anim.SetBool("canMove", false);
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
        // Karakterin y�n�n� de�i�tir
        transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, transform.localScale.y);
    }

    // Gizmos ile Raycast'i g�ster
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(rayDirection * rayDistance));
    }
}
