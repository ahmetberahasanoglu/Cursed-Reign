using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[RequireComponent(typeof(TouchDirection), typeof(Damageable))]
public class PlayerMovement : MonoBehaviour
{
    TouchDirection touchDirection;
    StaminaBar staminaBar;
    public ParticleSystem dust;
    [SerializeField] private float speed;
    [Header("Ziplama Ozellikleri")]
    [SerializeField] private float airWSpeed;
    public float Jump = 9f;
    [SerializeField] private float fallMultiplier = 2.5f; // Düþerken yerçekimi kuvvetini artýrmak için
    [SerializeField] private float coyoteTime = 0.2f; // Coyote time süresi
    [SerializeField] private float hangTime = 0.1f;

    [SerializeField] private int maxJumpCount = 2;
    [SerializeField] private int currentJumpCount;

    private float lastXDirection;
    private float coyoteTimeCounter; // Coyote time sayacý
    private float hangTimeCounter;
    private Rigidbody2D rb;

    [Header("Dash Degiskenleri")]
    [SerializeField] private bool canDash = true;
    [SerializeField] private bool isDashing;
    [SerializeField] private float dashPower = 20f;
    public int dashCooldown = 3;//Bunu gamemanager'daki dashBar'da Kullanacaðýz
    [SerializeField] private int maxdDashCount = 1;
    private float dashingTime=0.2f;
    [SerializeField] private TrailRenderer tr;

    PlayerOneWayPlatform PlayerOneWayPlatform;

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
    public Button rightM; // joystick dolayýsýyla kapadýk
    public Button dashB;
    public Button downB;

    public FixedJoystick fixedJoystick;


    private bool isJoystickUp = false; 


    playerAttack attack;
    private bool useJoystick = true;
  //  public GameObject panel;                               
   // public TextMeshProUGUI warningText;
                   
                                                           
                                                  
    public void SwitchControlJoystick()                    
    {                                                      
        useJoystick = true;
        UpdateControlUI();                                 
                                                           
    }                                                      
    public void SwitchControlButton()                      
    {                                                      
        useJoystick = false;
        
        UpdateControlUI();                                 
    }                                                      
                                                       
    private void UpdateControlUI()
    {
        if (useJoystick)
        {
            fixedJoystick.gameObject.SetActive(true);
             leftM.gameObject.SetActive(false);
             rightM.gameObject.SetActive(false);
           // PlayerOneWayPlatform.downButton.gameObject.SetActive(false);
           downB.gameObject.SetActive(false);
           // warningText.text = "You chose Joystick!";
        }
        else
        {
            fixedJoystick.gameObject.SetActive(false);
            leftM.gameObject.SetActive(true);
            rightM.gameObject.SetActive(true);
            downB.gameObject.SetActive(true);
            //PlayerOneWayPlatform.downButton.gameObject.SetActive(true);
          //  warningText.text = "You chose Buttons!";
        }
       
    }
    public float CurrentMoveSpeed
    {
        get
        {
            if (CanMove && !DialogueManager.Instance.isDialogueActive)
            {
                if (IsMoving && !touchDirection.IsOnWall)
                {
                    if (touchDirection.IsGrounded)
                    {
                        coyoteTimeCounter = coyoteTime;
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
        set
        {
            animator.SetBool(AnimStrings.canMove, value);
        }

    }
    public bool IsAlive
    {
        get
        { return animator.GetBool(AnimStrings.isAlive); }
    }

    private bool _isJumping;
    public bool IsJumping
    {
        get
        {
            return _isJumping;
        }

        private set
        {

            _isJumping = value;
        }
    }

    [SerializeField] private bool isMoving = false;
    public bool IsMoving
    {
        get
        {
            return isMoving;
        }

        private set
        {
            isMoving = value;
            animator.SetBool(AnimStrings.isMoving, value);
          /*  if (animator.GetBool(AnimStrings.isMoving) != value)
            {
                animator.SetBool(AnimStrings.isMoving, value);
            }*/
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
        touchDirection = GetComponent<TouchDirection>();
        damageable = GetComponent<Damageable>();
        PlayerOneWayPlatform=GetComponent<PlayerOneWayPlatform>();
        downB = GameObject.Find("DownButton").GetComponent<Button>();
        currentJumpCount = maxJumpCount;
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
      
        instance = this;
        DontDestroyOnLoad(gameObject);
        /* if (instance == null)
         {
             instance = this;
             DontDestroyOnLoad(gameObject);
         }
         else
         {
             Destroy(gameObject);
         }*/
        

    }
    public void OnTeknikPurchased()
    {
        animator.SetBool("Upgraded", true);
    }
    public void OnTilsimPurchased()
    {
        attack.attackDamage = 20;
    }
    public void OnDashPowerPurchased()
    {
        dashPower = 9f;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "CreditsScene"&& instance !=null)
        {
            Destroy(gameObject);
        }
    }

        private void Start()
    {
        attackB = GameObject.Find("AtckButton").GetComponent<Button>();
        JumpB = GameObject.Find("JumpButton").GetComponent<Button>();
        fireB = GameObject.Find("FireButton").GetComponent<Button>();
        leftM = GameObject.Find("LButton").GetComponent<Button>();
        rightM = GameObject.Find("RButton").GetComponent<Button>();
        dashB = GameObject.Find("dashButton").GetComponent<Button>();
       
       
            // warningText=GameObject.Find("WarningText").GetComponent<TextMeshProUGUI>();
            // panel = GameObject.Find("MovementPanel").GetComponent<GameObject>();
            manager = audiomanager.Instance;
        attack=GetComponentInChildren<playerAttack>();
        staminaBar = GameObject.Find("Stamina Bar").GetComponent<StaminaBar>();

        SceneManager.sceneLoaded += OnSceneLoaded;
        if (manager == null)
        {
            //Debug.LogError("AudioManager instance bulunamadý player Movement");
        }
        dashBar = FindObjectOfType<DashBar>();

        attackB.onClick.RemoveAllListeners();
        attackB.onClick.AddListener(OnAttackButtonPressed);

        attackB.onClick.AddListener(OnAttackButtonPressed);
        JumpB.onClick.AddListener(onJumpButtonPressed);
        fireB.onClick.AddListener(onFireButtonPressed);
        dashB.onClick.AddListener(OnDashButtonPressed);
        //downB.onClick.AddListener(PlayerOneWayPlatform.onDownButtonPressed());
      AddEventTrigger(leftM, (data) => onLeftButtonPressed(), EventTriggerType.PointerDown);
      AddEventTrigger(leftM, (data) => onLeftButtonReleased(), EventTriggerType.PointerUp);


      AddEventTrigger(rightM, (data) => onRightButtonPressed(), EventTriggerType.PointerDown);
      AddEventTrigger(rightM, (data) => onRightButtonReleased(), EventTriggerType.PointerUp);

        fixedJoystick = GameObject.Find("FixedJoystick").GetComponent<FixedJoystick>();
        useJoystick = true;
        UpdateControlUI();
  
        


    }

    void AddEventTrigger(Button button, UnityEngine.Events.UnityAction<BaseEventData> action, EventTriggerType triggerType)
    {
       
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = button.gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = triggerType
        };

        // Olayý baðla
        entry.callback.AddListener(action);

        // Entry'yi EventTrigger'a ekle
        trigger.triggers.Add(entry);
    }
   
        public void OnDashButtonPressed()
    {
        if (canDash && !isDashing) {
            manager.PlaySFX(manager.pjump, 0.1f);
            StartCoroutine(Dash());
        }
       
    }
    public void onLeftButtonPressed()
    {
        if (!useJoystick)
        {
            moveInput = new Vector2(-1, 0);
            IsMoving = true;
        }
        
       // if (IsAlive)
       // {
           
        //  }
    }

    public void onRightButtonPressed()
    {
        moveInput = new Vector2(1, 0);
        if (!useJoystick)
        {
            IsMoving = true;
         }
        
    }
    public void onLeftButtonReleased()
    {
        if (!useJoystick)
        {
            moveInput = Vector2.zero;
            IsMoving = false;
        }
      
    }

    public void onRightButtonReleased()
    {
        if (!useJoystick)
        {
            moveInput = Vector2.zero;
            IsMoving = false;
        }
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


    private bool isJumpButtonPressed = false;


    public void onJumpButtonPressed()
    {
        isJumpButtonPressed = true;
       
    }



    /*public void OnJump(InputAction.CallbackContext context)
    {
        /* basýp býraktýðýnda daha az zýplama. 
         if (context.canceled && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        
        //canlý olup olmadýgýný da kontrol edicz
        // Zýplamayý baþlatma
        if (context.started && (touchDirection.IsGrounded || coyoteTimeCounter > 0f) && CanMove)
        {
   
          //  PlayDust();
          //  rb.AddForce(Vector2.up * Jump, ForceMode2D.Impulse);
          //
          //  hangTimeCounter = hangTime; // Hang time baþlatýlýyor
          //  manager.PlaySFX(manager.pjump, 0.8f);
        }

        // Zýplama iptalini yönetmek (örneðin, düðmeye kýsa süre basarak zýplamayý iptal etme)
        /*
       if (context.canceled && rb.velocity.y > 0f)z
       {
           rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
       }


    }*/
   
    private bool isAttacking = false; 
    private float attackTimeout = 0.6f;
    private float attackTimer;
    public void OnAttackButtonPressed()
    {
        if (!isAttacking) // ztn saldýrma animasyonu oynamýyorsa
        {
            if (staminaBar.UseStamina())
            {
                staminaBar.use();
                
          

            isAttacking = true;
            animator.SetTrigger(AnimStrings.attackTrigger);
         
            attackTimer = attackTimeout;
            if (attack.triggerTetiklendi==false)//attack.dusmanaVurdu == false
            {
                manager.PlaySFX(manager.attack, avolume);
            }
            }
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


    public void OnAttackAnimationFinished()
    {
        isAttacking = false;
    }

    public void onFireButtonPressed()
    {
        animator.SetTrigger(AnimStrings.rangedAttackTrigger);

        
    }
    
    /*public void OnFire(InputAction.CallbackContext context) pcde denerken
    {
        if (context.started)
        {
            animator.SetTrigger(AnimStrings.rangedAttackTrigger);
            manager.PlaySFX(manager.laser, cvolume);
        }
    }*/
    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
        manager.PlaySFX(manager.pTakeHit, bvolume);
    }

    private DashBar dashBar;
    private IEnumerator Dash()
    {
        if (!canDash || isDashing)
        {
            yield break; // Eðer dash yapýlamýyorsa veya zaten dash yapýlýyorsa çýk
        }

        canDash = false;
        isDashing = true;
        damageable.isInvincible = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashPower, 0f);
        tr.emitting = true;
        dashBar.TriggerDashCooldown();

        yield return new WaitForSeconds(dashingTime);

        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        damageable.isInvincible = false;
       
        yield return new WaitForSeconds(dashCooldown);

       canDash = true; // Cooldown süresi doldu, dash tekrar kullanýlabilir
    }
    private void Update()
    {
        if (isJumpButtonPressed)
        {
            if ((touchDirection.IsGrounded || coyoteTimeCounter > 0f || currentJumpCount > 0) && CanMove)
            {
                rb.velocity = new Vector2(rb.velocity.x, Jump);
                manager.PlaySFX(manager.pjump, 0.6f);
                PlayDust();
                hangTimeCounter = hangTime;

                if (!touchDirection.IsGrounded)
                {
                    currentJumpCount--;
                }
            }
            isJumpButtonPressed = false; // Zýplama iþlemi tamamlandýktan sonra flag'i sýfýrla
        }

        if (isDashing)
        {
            return;
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
            HandleFootstepSound(); // Yürüme sesi kontrolü FixedUpdate içinde yapýlacak


        }
       
    }
    private void MoveCharacter(Vector2 direction)
    {
      
        Vector3 movement = new Vector3(direction.x, 0, 0) * speed;  


        rb.velocity = movement;
    }

    private void FixedUpdate()
    {
        /*if (fixedJoystick != null)
        {
            moveInput = new Vector2(fixedJoystick.Horizontal, 0);
            IsMoving = (Mathf.Abs(moveInput.x) > 0);

            if (IsMoving)
            {
                rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
            }
        }
        */

        if (isDashing)
        {
            return;
        }
        if (isAttacking) 
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                OnAttackAnimationFinished(); // Saldýrý zamanlayýcý süresi doldu //CanMove = false;
            }
           
        }
        if (touchDirection.IsGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            IsJumping = false;
            currentJumpCount = maxJumpCount; //     doubleJump = true;//zemine carapýnca doublejump tekrar aktif

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
        else
        {
            hangTimeCounter = 0;
        }


        #region hareket
        if (!damageable.IsHit && !DialogueManager.Instance.isDialogueActive)
        {
            if (useJoystick == true)
            {
                moveInput = new Vector2(fixedJoystick.Horizontal, 0);
                IsMoving = (Mathf.Abs(moveInput.x) > 0);
            }
            else                                                            //
            {                                                               //
                IsMoving = (moveInput != Vector2.zero);                     //
            }
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
            CanMove = true;
            if (!dashBar.IsCooldownActive())    
            {
                canDash = true;
            }
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y); // Diyalog sýrasýnda hareket edemiycez. Ýleride animasyonu da kapatabilirim
            CanMove = false;
            canDash = false;
        }
        #endregion

        if (platformTransform != null)
        {
            Vector2 platformVelocity = platformTransform.GetComponent<Rigidbody2D>().velocity;
            rb.velocity = new Vector2(rb.velocity.x + platformVelocity.x, rb.velocity.y);
        }





        if (rb.velocity.y < 0) // Karakter düþüþteyken
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y > 0) // Karakter yükseliyorken
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier * 0.5f - 1) * Time.fixedDeltaTime;
        }








         }
    private void PlayDust()
    {
        if (!dust.isPlaying)
        {
            dust.Play();
        }
    }
    private Transform platformTransform = null;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            platformTransform = collision.transform;

        }
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("MovingPlatform"))
        {
            _isJumping = false; // Zýplama durumu sýfýrlanýyor
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
    
        if (collision.gameObject.CompareTag("MovingPlatform"))    // Platformdan ayrýldýðýnda platform transform'u null yapýlýr
        {
            platformTransform = null;
        }


    }

}
