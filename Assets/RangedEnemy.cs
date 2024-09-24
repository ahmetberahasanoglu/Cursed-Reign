using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchDirection))]
public class RangedEnemy : MonoBehaviour
{
    #region Public Degiskenler
    public HealthBar healthBar;
    public DetectionZone attackZone;
    public DetectionZone cliffDetection;
    public DetectionZone trackDetection;
    public float moveSpeed;
    public Transform LeftLimit;
    public Transform RightLimit;
    public float patrolThreshold = 0.1f;
    public GameObject rangedBaslangic; // Projectile ba�lang�� noktas�
    public GameObject projectilePrefab; // Eklenen projectile prefab'�
    public float attackCooldownTime = 2f; // Sald�r� i�in bekleme s�resi
    public float footstepInterval = 0.3f; // Ad�m aral��� s�resi
    public float cvolume = 0.5f; // Footstep ses seviyesi
    #endregion

    #region Privates
    private Transform target;
    private Animator anim;
    private TouchDirection touchDirection;
    private Damageable damageable;
    private Rigidbody2D rb;
    private bool inArea = false;
    private bool isChasing = false;
    private float lastAttackTime; // Son sald�r� zaman�
    private float footstepTimer; // Ad�m zamanlay�c�s�
     audiomanager manager;
    #endregion

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        touchDirection = GetComponent<TouchDirection>();
        damageable = GetComponent<Damageable>();
        healthBar.SetHealth(damageable.Health, damageable.MaxHealth);

        
    }
    private void Start()
    {
        manager = audiomanager.Instance;
    }

    private void Update()
    {
        // Hedef varsa attackZone'dan hedef al
        HasTarget = attackZone.detectedColliders.Count > 0;

        if (trackDetection.detectedColliders.Count > 0)
        {
            target = trackDetection.detectedColliders[0].transform;
            isChasing = true;
        }
        else
        {
            target = null;
            isChasing = false; // Hedef olmad���nda kovalama biter
        }

        inArea = target != null;
        if (AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }

        healthBar.SetHealth(damageable.Health, damageable.MaxHealth);

        HandleFootstepSound(); // Ayak sesi kontrol�

        // Hedef varsa ve sald�r� s�resi dolmu�sa, sald�r
        if (HasTarget && Time.time - lastAttackTime > attackCooldownTime)
        {
            lastAttackTime = Time.time; // Son sald�r� zaman�n� g�ncelle
        }
    }

    private void HandleFootstepSound()
    {
        // E�er hareket ediyorsa ve yere temas ediyorsa ad�m sesi ��kar
        if (CanMove)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                // Ses �alma
                manager.PlaySFX(manager.skeletonWalk, cvolume);
                footstepTimer = footstepInterval; // Zamanlay�c�y� s�f�rla
                Debug.Log("ses cal�ndi");
            }

        }
        else
        {
            footstepTimer = footstepInterval; // Hareket etmiyorsa zamanlay�c� s�f�rlan�r
        }
    }

        private bool IsMoving()
    {
        // Karakterin hareket edip etmedi�ini kontrol et
        return Mathf.Abs(rb.velocity.x) > 0.1f;
    }

  

    public void FireProjectile()
    {
        if (projectilePrefab != null && rangedBaslangic != null)
        {
            // Projectile instantiate et
            GameObject projectile = Instantiate(projectilePrefab, rangedBaslangic.transform.position, Quaternion.identity);

            // Projectile'�n y�n�n� d��man�n y�n�ne g�re ayarla
            Vector3 projectileScale = projectile.transform.localScale;
            projectileScale.x *= transform.localScale.x > 0 ? 1 : -1; // D��man�n y�n�ne g�re y�n de�i�tir
            projectile.transform.localScale = projectileScale;

            Debug.Log("Projectile f�rlat�ld�!");
        }
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
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("hasTarget"))
        {
            if (!isChasing && !InsideOfLimits())
            {
                SelectTarget();
            }
            else if (isChasing) // Hedef varsa kovalama ba�las�n
            {
                Move(target);
            }
        }

        if (touchDirection.IsOnWall && touchDirection.IsGrounded)
        {
            FlipDirection();
        }

        if (!damageable.IsHit && touchDirection.IsGrounded && target != null)
        {
            Move(target);
        }
        else if (!damageable.IsHit && touchDirection.IsGrounded && !isChasing)
        {
            Move(SelectTarget());
        }
    }

    public float treshold = 2f;
    public float minChaseDistance = 1f;

    private void Move(Transform targetTransform)
    {
        if (targetTransform != null)
        {
            Vector2 targetPosition = targetTransform.position;
            float distanceToTarget = Mathf.Abs(targetPosition.x - transform.position.x);
            // Debug.Log("Hedefe uzakl�k: " + distanceToTarget);

            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("hasTarget"))
            {
                anim.SetBool("canMove", true);

                if (distanceToTarget > minChaseDistance && distanceToTarget > treshold)
                {
                    // Hedef karakterin solunda m� sa��nda m� onu kontrol et
                    if ((targetPosition.x < transform.position.x && transform.localScale.x > 0) ||
                        (targetPosition.x > transform.position.x && transform.localScale.x < 0))
                    {
                        FlipDirection();
                    }
                }

                if (distanceToTarget > minChaseDistance)
                {
                    Vector2 newPosition = new Vector2(targetPosition.x, transform.position.y);
                    transform.position = Vector2.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);
                }
                else
                {
                    // Hedef �ok yak�n, hareketi durdur
                    anim.SetBool("canMove", false);
                }
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
        // Karakterin y�n�n� de�i�tir
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }

    private bool InsideOfLimits()
    {
        bool inside = transform.position.x > LeftLimit.position.x && transform.position.x < RightLimit.position.x;
        return inside;
    }

    private Transform SelectTarget()
    {
        // Devriye hedefini se�
        float distanceToLeft = Vector2.Distance(transform.position, LeftLimit.position);
        float distanceToRight = Vector2.Distance(transform.position, RightLimit.position);

        // Karakter devriye alan�n�n ortas�nda ise sa�a m� sola m� gidece�ine karar verememesini engellemek i�in threshold ekliyoruz
        if (Mathf.Abs(distanceToLeft - distanceToRight) < patrolThreshold)
        {
            return null; // Ortada ise hedefe gitme
        }

        if (distanceToLeft > distanceToRight)
        {
            return LeftLimit;
        }
        else
        {
            return RightLimit;
        }
    }
}
