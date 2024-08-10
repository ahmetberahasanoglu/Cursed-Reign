using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(TouchDirection))]
public class PlayerMovement : MonoBehaviour
{
    TouchDirection touchDirection;
    [SerializeField] private float speed;
    [SerializeField] private float airWSpeed;
    [SerializeField] private float Jump;
    private Rigidbody2D rb;
    Animator animator;
    Vector2 moveInput;

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
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchDirection= GetComponent<TouchDirection>();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput =context.ReadValue<Vector2>();

        IsMoving = moveInput != Vector2.zero;//if(moveInput!=vecor2.zero){Ismoving=true; else false;} kodu gibi
    }
    
    public void OnJump(InputAction.CallbackContext context)
    {
        /* basýp býraktýðýnda daha az zýplama. 
         if (context.canceled && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        */
        //canlý olup olmadýgýný da kontrol edicz
        if (context.started && touchDirection.IsGrounded&& CanMove) {
            animator.SetTrigger(AnimStrings.jumpTrigger);
            rb.velocity= new Vector2(rb.velocity.x, Jump);
        }

    }

   
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimStrings.attackTrigger);
        }
    }
   private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
        animator.SetFloat(AnimStrings.yVelocity, rb.velocity.y);

        if (moveInput.x > 0 )
        {
            transform.localScale = new Vector2(3,3);
        }
        else if(moveInput.x < 0 ) 
        {
            transform.localScale = new Vector2(-3, 3);
        }
       /* if (Input.GetKey(KeyCode.Space))
        {
            rb.velocity= new Vector2(rb.velocity.x, Jump);
        } */
    }
}
