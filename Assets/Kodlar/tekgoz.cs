using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TekGoz : MonoBehaviour
{
    public DetectionZone isirmaDetectionZone;
    Damageable damageable;
    Animator animator;
    Rigidbody2D rb;
    public List<Transform> wayPoints;
    Transform nextWayPoint;
    public float waypointReachedDistance = 0.15f;
    public float ucusHizi = 2f;

    int waypointsayisi = 0;
    public HealthBar healthBar;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        damageable = GetComponent<Damageable>();

        if (healthBar != null)
        {
            healthBar.SetHealth(damageable.Health, damageable.MaxHealth);
        }
        else
        {
            Debug.LogWarning("HealthBar is not assigned!");
        }
    }

    private void Start()
    {
        if (wayPoints != null && wayPoints.Count > 0)
        {
            nextWayPoint = wayPoints[waypointsayisi];
        }
        else
        {
            Debug.LogError("Waypoints are not assigned or empty!");
        }
    }

    private void Update()
    {
        if (healthBar != null)
        {
            healthBar.SetHealth(damageable.Health, damageable.MaxHealth);
        }

        HasTarget = isirmaDetectionZone.detectedColliders.Count > 0;
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
            if (hasTarget != value)
            {
                hasTarget = value;
                animator.SetBool(AnimStrings.hasTarget, value);
            }
        }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimStrings.canMove);
        }
    }

    private void FixedUpdate()
    {
        if (damageable.IsAlive)
        {
            if (CanMove && wayPoints.Count > 0)
            {
                Flight();
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }
    }

    void Flight()
    {
        // Next waypoint'e uç
        Vector2 directionToWayPoint = (nextWayPoint.position - transform.position).normalized;

        // Waypoint'e ulaþýlýp ulaþýlmadýðýný kontrol et
        float distance = Vector2.Distance(nextWayPoint.position, transform.position);

        rb.velocity = directionToWayPoint * ucusHizi;

        // Waypoint deðiþtirme
        if (distance <= waypointReachedDistance)
        {
            waypointsayisi++;
            if (waypointsayisi >= wayPoints.Count)
            {
                waypointsayisi = 0; // Baþlangýç waypoint'ine dön
            }
            nextWayPoint = wayPoints[waypointsayisi];
        }
    }
}
