using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

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

    public static PlayerMovement instance;

    public Button attackB;
    public Button JumpB;
    public Button fireB;
    public Button leftM;
    public Button rightM;   
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
                        // havadaki hýz
                        return airWSpeed;
                    }
                }
                else
                {
                   //duzDururkenHýz
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
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        manager = audiomanager.Instance;
        if (manager == null)
        {
            Debug.LogError("AudioManager instance bulunamadý player Movement");
        }
        
        attackB.onClick.AddListener(OnAttackButtonPressed);
        JumpB.onClick.AddListener(onJumpButtonPressed);
        fireB.onClick.AddListener(onFireButtonPressed);
     
    }
    
    public void onLeftButtonPressed()
    {
        moveInput = new Vector2(-1, 0);
        if (IsAlive)
        {
            IsMoving = true;
        }
        Debug.Log("Sol tuþ basýldý");
    }
    public void onRightButtonPressed()
    {
        moveInput = new Vector2(1, 0);
            if (IsAlive)
            {
                IsMoving = true;
            }
        Debug.Log("Sað tuþ basýldý.");
    }
    public void onLeftButtonReleased()
    {
        moveInput = Vector2.zero;
        IsMoving = false;
        Debug.Log("Sol tuþ býrakýldý, karakter durmalý");
    }

    public void onRightButtonReleased()
    {
        moveInput = Vector2.zero;
        IsMoving = false;
        Debug.Log("Sað tuþ býrakýldý, karakter durmalý");
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (IsAlive)
        {
            IsMoving = moveInput != Vector2.zero; // Karakterin hareket edip etmediðini kontrol et

        }
        else
        {
            IsMoving = false;
        }
    }

    [SerializeField] private float footstepInterval = 0.5f; // Ýki adým sesi arasýndaki süre
    private float footstepTimer;

    private void HandleFootstepSound()
    {
        if (IsMoving && touchDirection.IsGrounded)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                manager.PlaySFX(manager.groundTouch, cvolume);
                footstepTimer = footstepInterval; // Zamanlayýcýyý yeniden baþlat
            }
        }
        else
        {
            footstepTimer = 0f; // Hareket etmezse zamanlayýcý sýfýrlanýr
        }
    }



    public void onJumpButtonPressed()
    {
        if ((touchDirection.IsGrounded || coyoteTimeCounter > 0f) && CanMove)
        {
            animator.SetTrigger(AnimStrings.jumpTrigger);
            PlayDust();
            rb.AddForce(Vector2.up * Jump, ForceMode2D.Impulse);

            hangTimeCounter = hangTime; // Hang time baþlatýlýyor
            manager.PlaySFX(manager.pjump, 0.8f);
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
            rb.AddForce(Vector2.up * Jump, ForceMode2D.Impulse);

            hangTimeCounter = hangTime; // Hang time baþlatýlýyor
            manager.PlaySFX(manager.pjump, 0.8f);
        }

        // Zýplama iptalini yönetmek (örneðin, düðmeye kýsa süre basarak zýplamayý iptal etme)
        /*
       if (context.canceled && rb.velocity.y > 0f)z
       {
           rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
       }*/


    }
    private bool isAttacking = false; //yeni
    private float attackTimeout = 0.6f; 
    private float attackTimer;
    public void OnAttackButtonPressed()
    {
        if (!isAttacking) // ztn saldýrma animasyonu oynamýyorsa
        {
            isAttacking = true;
            animator.SetTrigger(AnimStrings.attackTrigger);
            manager.PlaySFX(manager.attack, avolume);
            attackTimer = attackTimeout;
        }
    }
    /*
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started && !isAttacking) // ztn saldýrma animasyonu oynamýyorsa
        {
            isAttacking = true;
            animator.SetTrigger(AnimStrings.attackTrigger);
            manager.PlaySFX(manager.attack, avolume);
            attackTimer = attackTimeout; 
        }
    }*/

    //yeni
    public void OnAttackAnimationFinished()
    {
        isAttacking = false;
    }

    public void onFireButtonPressed()
    {
        animator.SetTrigger(AnimStrings.rangedAttackTrigger);
        manager.PlaySFX(manager.laser, cvolume);
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
 

    
    private void FixedUpdate()
    {
        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                OnAttackAnimationFinished(); // Saldýrý zamanlayýcý süresi doldu
            }
        }
        if (touchDirection.IsGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.fixedDeltaTime;
        }
        
      
        if (hangTimeCounter > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.9f);
            hangTimeCounter -= Time.fixedDeltaTime;
        }


        #region hareket
        if (!damageable.IsHit && !DialogueManager.Instance.isDialogueActive)
        {
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y); // Diyalog sýrasýnda hareket edemiycez. Ýleride animasyonu da kapatabilirim
        }
        #endregion

        if (platformTransform != null)
        {
            Vector2 platformVelocity = platformTransform.GetComponent<Rigidbody2D>().velocity;
            rb.velocity = new Vector2(rb.velocity.x + platformVelocity.x, rb.velocity.y);
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

            // Yürüme sesi kontrolü FixedUpdate içinde yapýlacak
            HandleFootstepSound();
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
    private Transform platformTransform = null;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Platforma temas ettiðinde platformun transform'unu alýyoruz
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            platformTransform = collision.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Platformdan ayrýldýðýnda platform transform'u null yapýlýr
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            platformTransform = null;
        }
    }

}
