using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

[RequireComponent(typeof(TouchDirection), typeof(Damageable) )]
public class PlayerMovement : MonoBehaviour
{
    TouchDirection touchDirection;
    public ParticleSystem dust;
    [SerializeField] private float speed;
    [Header("Ziplama Ozellikleri")]
    [SerializeField] private float airWSpeed;
    public float Jump=12f;
    [SerializeField] private float fallMultiplier = 2.5f; // Düþerken yerçekimi kuvvetini artýrmak için
    [SerializeField] private float coyoteTime = 0.2f; // Coyote time süresi
    [SerializeField] private float hangTime = 0.1f;
    private float lastXDirection;
    private float coyoteTimeCounter; // Coyote time sayacý
    private float hangTimeCounter;
    private Rigidbody2D rb;
    Animator animator;
    Vector2 moveInput;
    Damageable damageable;

    [Header("Sesler")]
    [SerializeField] float avolume = 0.5f;
    [SerializeField] float bvolume = 0.5f;
    [SerializeField] float cvolume = 0.5f;
    audiomanager manager;
    public float CurrentMoveSpeed
    {
        get
        {
            if (CanMove &&!DialogueManager.Instance.isDialogueActive)
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
                    // Idle speed 0
                    return 0;
                }
            }
            else
            {
                // yürüyemez durumdaysak hýz zaten 0 
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
        } trigger ile hasar alma mantýgý olsaydý bunu kullanabilirdik damagable instance'ýna gerek kalmzaDI*/ 

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchDirection= GetComponent<TouchDirection>();
        damageable = GetComponent<Damageable>();

    }

    private void Start()
    {
        manager = audiomanager.Instance;
        if (manager == null)
        {
            Debug.LogError("AudioManager instance bulunamadý player Movement");
        }
    }

   
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (IsAlive)
        {
            IsMoving = moveInput != Vector2.zero;//if(moveInput!=vecor2.zero){Ismoving=true; else false;} kodu gibi

            if (IsMoving && touchDirection.IsGrounded)
            {
                // Eðer ses oynatýlmýyorsa veya baþka bir ses çalýnýyorsa
                if (!manager.SFXSource.isPlaying || manager.SFXSource.clip != manager.groundTouch)
                {
                   
                    manager.SFXSource.clip = manager.groundTouch;
                    manager.SFXSource.volume = cvolume;
                    manager.SFXSource.loop = true; 
                    manager.SFXSource.Play();
                }
            }
            else
            {
                // Karakter durduðunda veya havada olduðunda yürüme sesi durduruluyor
                if (manager.SFXSource.isPlaying && manager.SFXSource.clip == manager.groundTouch)
                {
                    manager.SFXSource.loop = false;
                    manager.SFXSource.Stop();
                }
            }
        }
        else
        {
            IsMoving = false;
        }
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
        // Zýplamayý baþlatma
        if (context.started && (touchDirection.IsGrounded || coyoteTimeCounter > 0f) && CanMove)
        {
            animator.SetTrigger(AnimStrings.jumpTrigger);
            PlayDust();
            rb.velocity = new Vector2(rb.velocity.x, Jump);
            hangTimeCounter = hangTime; // Hang time baþlatýlýyor
            manager.PlaySFX(manager.pjump, 0.8f);
        }

        // Zýplama iptalini yönetmek (örneðin, düðmeye kýsa süre basarak zýplamayý iptal etme)
        /*
       if (context.canceled && rb.velocity.y > 0f)
       {
           rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
       }*/


    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimStrings.attackTrigger);
            manager.PlaySFX(manager.attack, avolume);
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimStrings.rangedAttackTrigger);
            manager.PlaySFX(manager.laser, cvolume);
        }
    }
    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
        manager.PlaySFX(manager.pTakeHit, bvolume);
    }
    private void Update()
    {
        // Coyote time sayacýný güncelleme
        if (touchDirection.IsGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        // Hang time süresi boyunca hýz çok düþük tutulur
        if (hangTimeCounter > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.9f);
            hangTimeCounter -= Time.deltaTime;
        }
    }


    
    private void FixedUpdate()
    {
        
        
            if (!damageable.IsHit && !DialogueManager.Instance.isDialogueActive) // LockVelocity ve diyalog kontrolü
            {
                rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y); // Diyalog sýrasýnda yatay hýzý sýfýrla
            }

            // Yüksek zýplamalarý veya düþüþleri yönetmek için yerçekimi kuvvetini artýrma
            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
            }

            animator.SetFloat(AnimStrings.yVelocity, rb.velocity.y);

            if (IsAlive)
            {
                if (moveInput.x > 0)
                {
                    if (lastXDirection <= 0) // Yön deðiþtiriyorsa
                    {
                        transform.localScale = new Vector2(3, 3);
                        PlayDust();
                    }
                    lastXDirection = 1;
                }
                else if (moveInput.x < 0)
                {
                    if (lastXDirection >= 0) // Yön deðiþtiriyorsa
                    {
                        transform.localScale = new Vector2(-3, 3);
                        PlayDust();
                    }
                    lastXDirection = -1;
                }
            }
        


        /* if (Input.GetKey(KeyCode.Space))
         {
             rb.velocity= new Vector2(rb.velocity.x, Jump);
         } */
    }
    private void PlayDust()
    {
        if (dust.isPlaying)
        {
            dust.Stop();
        }
        dust.Play();
    }
}
