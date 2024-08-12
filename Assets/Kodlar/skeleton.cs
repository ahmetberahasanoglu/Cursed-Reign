using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchDirection))]
public class skeleton : MonoBehaviour
{
    public float speed = 3f;
    [SerializeField] private float walkStopRate=0.6f;
    public DetectionZone attackZone;
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
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchDirection = GetComponent<TouchDirection>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
    }

    private void Update()
    {
        HasTarget=attackZone.detectedColliders.Count> 0;
       
    }
    void FixedUpdate()
    {

        if (touchDirection.IsOnWall && touchDirection.IsGrounded)
        {
            FlipDirection();
        }
        if(!damageable.IsHit)
        {

        

        if(CanMove) { rb.velocity = new Vector2(speed * walkDirectionVector.x, rb.velocity.y); }
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
}
