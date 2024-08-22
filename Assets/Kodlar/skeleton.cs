using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchDirection))]
public class skeleton : MonoBehaviour
{
    public float ivme = 3f;
    public float maxSpeed= 3f;
    [SerializeField] private float walkStopRate=0.6f;
    public DetectionZone attackZone;
    public DetectionZone cliffDetection;
    public HealthBar healthBar;
    Rigidbody2D rb;
    TouchDirection touchDirection;
    Animator animator;

    Damageable damageable;

    

    public enum WalkableDirection
    {
        Sag, Sol
    }
    private WalkableDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.left;
    public WalkableDirection WalkDirection {
        get { return _walkDirection; }
        set {
            if (_walkDirection != value)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);

                if (value == WalkableDirection.Sag)
                {
                    walkDirectionVector = Vector2.right;

                } else if (value == WalkableDirection.Sol)
                {
                    walkDirectionVector = Vector2.left;
                }
            }
            _walkDirection = value;

        }
    }
    private bool hasTarget = false;


    public bool HasTarget
    {
        get
        {
            return hasTarget;
        }
        private set {
            hasTarget = value;
            animator.SetBool(AnimStrings.hasTarget, value);
        }
    
    }
 
    public bool CanMove {  
        get
        { 
            return animator.GetBool(AnimStrings.canMove);
        }
    }

    public float AttackCooldown
    {
        get
        {
            return animator.GetFloat(AnimStrings.attackCooldown);
        }
        set
        { 
            animator.SetFloat(AnimStrings.attackCooldown, Mathf.Max(value,0));
        }
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchDirection = GetComponent<TouchDirection>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
        healthBar.SetHealth(damageable.Health, damageable.MaxHealth);
    }

    private void Update()
    {
        HasTarget=attackZone.detectedColliders.Count> 0;
        if(AttackCooldown> 0) {
            AttackCooldown -= Time.deltaTime;
        }
        healthBar.SetHealth(damageable.Health, damageable.MaxHealth);

    }
    void FixedUpdate()
    {

        if (touchDirection.IsOnWall && touchDirection.IsGrounded)        //|| cliffDetection.detectedColliders.Count  == 0
        {
            FlipDirection();
        }

        if(!damageable.IsHit)
        {
            if(CanMove&& touchDirection.IsGrounded) 
            { 
               
                rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x + (ivme * walkDirectionVector.x * Time.fixedDeltaTime), -maxSpeed, maxSpeed), rb.velocity.y); //eSKÝDEN HÝZ * walkDirectionVector.x ÝDÝ
            }
            else
            {
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x,0,walkStopRate),rb.velocity.y);
            }

        }

    }

    private void FlipDirection()
    {
        if(WalkDirection == WalkableDirection.Sag)
        {
            WalkDirection=  WalkableDirection.Sol;
        }else if(WalkDirection== WalkableDirection.Sol)
        {
            WalkDirection= WalkableDirection.Sag;
        }
        else
        {
            Debug.Log("saga ya da sola yürümüpo");
        }
    }
    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }
    public void OnCliffDetected()
    {
        if(touchDirection.IsGrounded) {
            FlipDirection();
        }
    }
}
