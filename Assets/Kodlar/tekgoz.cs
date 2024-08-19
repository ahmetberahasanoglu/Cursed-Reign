using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tekgoz : MonoBehaviour
{
 
    public DetectionZone isirmaDetectionZone;
    Damageable damageable;
    Animator animator;
    Rigidbody rb;
    public List<Transform> wayPoints;
    Transform nextWayPoint;
    public float waypointReachedDistance=0.15f;
    public float ucusHizi = 2f;

    int waypointsayisi = 0;

    //public HealthBar healthBar;

    private void Awake()
    {
        animator = GetComponent<Animator>();    
        rb = GetComponent<Rigidbody>();
      //  healthBar.SetHealth(damageable.Health, damageable.MaxHealth);
    }
    private void Start()
    {
        nextWayPoint = wayPoints[waypointsayisi];
    }
    private void Update()
    {
      //  healthBar.SetHealth(damageable.Health, damageable.MaxHealth);
        HasTarget = isirmaDetectionZone.detectedColliders.Count>0;
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
            animator.SetBool(AnimStrings.hasTarget, value);
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
            if (CanMove)
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
        //Soonraki waypointe uc
        Vector2 directionToWayPoint= (nextWayPoint.position-transform.position).normalized;

        //ulastýk mý kontorl
        float distance = Vector2.Distance(nextWayPoint.position, transform.position);

        rb.velocity = directionToWayPoint * ucusHizi;
        //waypoint degistirmel imiyiz
        if(distance <= waypointReachedDistance)
        {
            //sonraki wayP
            waypointsayisi++;
            if(waypointsayisi >= wayPoints.Count)
            {
                //baslangýc waypointine don
                waypointsayisi = 0;
            }
            nextWayPoint = wayPoints[waypointsayisi];
        }
    }
}
