using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchDirection))]
public class Wizard : MonoBehaviour
{
    #region Public Degiskenler
    public HealthBar healthBar;
    public DetectionZone attackZone;
    public DetectionZone cliffDetection;
    public DetectionZone trackDetection;
    public float moveSpeed;
    public Transform LeftLimit;
    public Transform RightLimit;
    #endregion

    #region Privates
    private Transform target;
    private Animator anim;
    private TouchDirection touchDirection;
    private Damageable damageable;
    private Rigidbody2D rb;
    private bool inArea = false;
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
        // Hedef varsa attackZone'dan hedef al
        HasTarget = attackZone.detectedColliders.Count > 0;
        if (trackDetection.detectedColliders.Count > 0)
        {
            target = trackDetection.detectedColliders[0].transform;
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
        get { return hasTarget; }
        private set
        {
            hasTarget = value;
            anim.SetBool("hasTarget", value);
        }
    }

    public bool CanMove
    {
        get { return anim.GetBool("canMove"); }
    }

    public float AttackCooldown
    {
        get { return anim.GetFloat(AnimStrings.attackCooldown); }
        set { anim.SetFloat(AnimStrings.attackCooldown, Mathf.Max(value, 0)); }
    }

    private void FixedUpdate()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("hasTarget") && !InsideOfLimits())
        {
            SelectTarget();
            Debug.Log("Has target="+anim.GetCurrentAnimatorStateInfo(0).IsName("hasTarget"));
        }

        if (touchDirection.IsOnWall && touchDirection.IsGrounded)
        {
            FlipDirection();
        }

        if (!damageable.IsHit && touchDirection.IsGrounded)
        {
            // Hareket fonksiyonunu çaðýrýyoruz
            Move(target);
        }
    }

    private void Move(Transform targetTransform)
    {
        if (targetTransform != null)
        {
            Vector2 targetPosition = targetTransform.position;

            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("hasTarget")) // Saldýrmýyorsak
            {
                anim.SetBool("canMove", true);

                // Karakterin saðýnda mý solunda mý olduðunu kontrol et
                if ((targetPosition.x < transform.position.x && transform.localScale.x > 0) ||
                    (targetPosition.x > transform.position.x && transform.localScale.x < 0))
                {
                    // Karakterin yönünü hedefe doðru döndür
                    FlipDirection();
                }

                // Hedefin konumuna doðru hareket et
                Vector2 newPosition = new Vector2(targetPosition.x, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);
            }
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
        // Karakterin yönünü deðiþtir
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }

    private bool InsideOfLimits()
    {
        bool inside = transform.position.x > LeftLimit.position.x && transform.position.x < RightLimit.position.x;
        Debug.Log("Inside of limits: " + inside);
        return inside;
    }

    private void SelectTarget()
    {
        float distanceToLeft = Vector2.Distance(transform.position, LeftLimit.position);
        float distanceToRight = Vector2.Distance(transform.position, RightLimit.position);

        if (distanceToLeft > distanceToRight)
        {
            target = LeftLimit;
        }
        else
        {
            target = RightLimit;
        }
        FlipDirection();
    }
}
