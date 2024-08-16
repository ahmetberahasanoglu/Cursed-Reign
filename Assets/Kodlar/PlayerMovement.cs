using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(TouchDirection), typeof(Damageable) )]
public class PlayerMovement : MonoBehaviour
{
    TouchDirection touchDirection;
    [SerializeField] private float speed;
    [SerializeField] private float airWSpeed;
    [SerializeField] private float Jump;
    [SerializeField] private float fallMultiplier = 2.5f; // D��erken yer�ekimi kuvvetini art�rmak i�in
    [SerializeField] private float lowJumpMultiplier = 2f; // K�sa z�plamalar i�in yer�ekimi kuvvetini art�rmak i�in
    [SerializeField] private float coyoteTime = 0.2f; // Coyote time s�resi
    [SerializeField] private float hangTime = 0.1f;
    private float coyoteTimeCounter; // Coyote time sayac�
    private float hangTimeCounter;
    private Rigidbody2D rb;
    Animator animator;
    Vector2 moveInput;
    Damageable damageable;
    public float CurrentMoveSpeed
    {
        get
        {
            if (CanMove)
            {
                if (IsMoving && !touchDirection.IsOnWall)
                {
                    if (touchDirection.IsGrounded)
                    {
    
                            return speed;
           
                    }
                    else
                    {
                        // Air Move
                        return airWSpeed;
                    }
                }
                else
                {
                    // Idle speed is 0
                    return 0;
                }
            }
            else
            {
                // Movement locked
                return 0;
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
    public bool IsAlive { get
        { return animator.GetBool(AnimStrings.isAlive); } 
    }

    private bool _isJumping;
    public bool IsJumping { get
        {
            return _isJumping;
        }

        private set {
             _isJumping = value;
        } }

    [SerializeField] private bool isMoving = false;
    public bool IsMoving { get {
            return isMoving;
        }
        
         private set {
            isMoving = value;
            animator.SetBool(AnimStrings.isMoving, value);
        }
    }

  /*  public bool LockVelocity { get 
        {
            return animator.GetBool(AnimStrings.lockVelocity)}
        } trigger ile hasar alma mant�g� olsayd� bunu kullanabilirdik damagable instance'�na gerek kalmzaDI*/ 

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchDirection= GetComponent<TouchDirection>();
        damageable = GetComponent<Damageable>();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput =context.ReadValue<Vector2>();
        if(IsAlive) { 
        IsMoving = moveInput != Vector2.zero;//if(moveInput!=vecor2.zero){Ismoving=true; else false;} kodu gibi
        }
        else
        {
            IsMoving = false;
        }
    }
    
    public void OnJump(InputAction.CallbackContext context)
    {
        /* bas�p b�rakt���nda daha az z�plama. 
         if (context.canceled && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        */
        //canl� olup olmad�g�n� da kontrol edicz
        // Z�plamay� ba�latma
        if (context.started && (touchDirection.IsGrounded || coyoteTimeCounter > 0f) && CanMove)
        {
            animator.SetTrigger(AnimStrings.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, Jump);
            hangTimeCounter = hangTime; // Hang time ba�lat�l�yor
        }

        // Z�plama iptalini y�netmek (�rne�in, d��meye k�sa s�re basarak z�plamay� iptal etme)
        /*
       if (context.canceled && rb.velocity.y > 0f)
       {
           rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
       }*/


    }
    private void Update()
    {
        // Coyote time sayac�n� g�ncelleme
        if (touchDirection.IsGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        // Hang time s�resi boyunca h�z �ok d���k tutulur
        if (hangTimeCounter > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.9f);
            hangTimeCounter -= Time.deltaTime;
        }
    }


    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimStrings.attackTrigger);
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimStrings.rangedAttackTrigger);
        }
    }
    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }
    private void FixedUpdate()
    {
        if (!damageable.IsHit)//LockVelocity
        {
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
        }

        // Y�ksek z�plamalar� veya d����leri y�netmek i�in yer�ekimi kuvvetini art�rma
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }

        animator.SetFloat(AnimStrings.yVelocity, rb.velocity.y);
      
        if (IsAlive) { 
        if (moveInput.x > 0 )
        {
            transform.localScale = new Vector2(3,3);
        }
        else if(moveInput.x < 0 ) 
        {
            transform.localScale = new Vector2(-3, 3);
        }
        }
        /* if (Input.GetKey(KeyCode.Space))
         {
             rb.velocity= new Vector2(rb.velocity.x, Jump);
         } */
    }
}
