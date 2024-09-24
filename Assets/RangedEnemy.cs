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
    public GameObject rangedBaslangic; // Projectile baþlangýç noktasý
    public GameObject projectilePrefab; // Eklenen projectile prefab'ý
    public float attackCooldownTime = 2f; // Saldýrý için bekleme süresi
    public float footstepInterval = 0.3f; // Adým aralýðý süresi
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
    private float lastAttackTime; // Son saldýrý zamaný
    private float footstepTimer; // Adým zamanlayýcýsý
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
            isChasing = false; // Hedef olmadýðýnda kovalama biter
        }

        inArea = target != null;
        if (AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }

        healthBar.SetHealth(damageable.Health, damageable.MaxHealth);

        HandleFootstepSound(); // Ayak sesi kontrolü

        // Hedef varsa ve saldýrý süresi dolmuþsa, saldýr
        if (HasTarget && Time.time - lastAttackTime > attackCooldownTime)
        {
            lastAttackTime = Time.time; // Son saldýrý zamanýný güncelle
        }
    }

    private void HandleFootstepSound()
    {
        // Eðer hareket ediyorsa ve yere temas ediyorsa adým sesi çýkar
        if (CanMove)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                // Ses çalma
                manager.PlaySFX(manager.skeletonWalk, cvolume);
                footstepTimer = footstepInterval; // Zamanlayýcýyý sýfýrla
                Debug.Log("ses calýndi");
            }

        }
        else
        {
            footstepTimer = footstepInterval; // Hareket etmiyorsa zamanlayýcý sýfýrlanýr
        }
    }

        private bool IsMoving()
    {
        // Karakterin hareket edip etmediðini kontrol et
        return Mathf.Abs(rb.velocity.x) > 0.1f;
    }

  

    public void FireProjectile()
    {
        if (projectilePrefab != null && rangedBaslangic != null)
        {
            // Projectile instantiate et
            GameObject projectile = Instantiate(projectilePrefab, rangedBaslangic.transform.position, Quaternion.identity);

            // Projectile'ýn yönünü düþmanýn yönüne göre ayarla
            Vector3 projectileScale = projectile.transform.localScale;
            projectileScale.x *= transform.localScale.x > 0 ? 1 : -1; // Düþmanýn yönüne göre yön deðiþtir
            projectile.transform.localScale = projectileScale;

            Debug.Log("Projectile fýrlatýldý!");
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
            else if (isChasing) // Hedef varsa kovalama baþlasýn
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
            // Debug.Log("Hedefe uzaklýk: " + distanceToTarget);

            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("hasTarget"))
            {
                anim.SetBool("canMove", true);

                if (distanceToTarget > minChaseDistance && distanceToTarget > treshold)
                {
                    // Hedef karakterin solunda mý saðýnda mý onu kontrol et
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
                    // Hedef çok yakýn, hareketi durdur
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
        // Karakterin yönünü deðiþtir
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }

    private bool InsideOfLimits()
    {
        bool inside = transform.position.x > LeftLimit.position.x && transform.position.x < RightLimit.position.x;
        return inside;
    }

    private Transform SelectTarget()
    {
        // Devriye hedefini seç
        float distanceToLeft = Vector2.Distance(transform.position, LeftLimit.position);
        float distanceToRight = Vector2.Distance(transform.position, RightLimit.position);

        // Karakter devriye alanýnýn ortasýnda ise saða mý sola mý gideceðine karar verememesini engellemek için threshold ekliyoruz
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
