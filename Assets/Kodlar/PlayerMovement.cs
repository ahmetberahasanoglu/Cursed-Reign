using System.Collections;
using System.Collections.Generic;
using System.Data;
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
    [SerializeField] private float fallMultiplier = 2.5f; // D��erken yer�ekimi kuvvetini art�rmak i�in
    [SerializeField] private float coyoteTime = 0.2f; // Coyote time s�resi
    [SerializeField] private float hangTime = 0.1f;

    [SerializeField] private int maxJumpCount = 2;
    [SerializeField] private int currentJumpCount;

    private float lastXDirection;
    private float coyoteTimeCounter; // Coyote time sayac�
    private float hangTimeCounter;
    private Rigidbody2D rb;

    [Header("Dash Degiskenleri")]
    [SerializeField] private bool canDash = true;
    [SerializeField] private bool isDashing;
    [SerializeField] private float dashPower = 20f;
    public int dashCooldown = 3;//Bunu gamemanager'daki dashBar'da Kullanaca��z
    [SerializeField] private int maxdDashCount = 1;
    private float dashingTime=0.2f;
    [SerializeField] private TrailRenderer tr;



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
    //public Button leftM;
   // public Button rightM; joystick dolay�s�yla kapad�k
    public Button dashB;

    public FixedJoystick fixedJoystick;

    private bool isJoystickUp = false; // Joystick yukar� kald�r�ld� m�?


    playerAttack attack;
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
                        // havadaki h�z
                        return airWSpeed;
                    }
                }
                else
                {
                    //duzDururkenH�z
                    return 0;
                }
            }
            else
            {
                // y�r�yemez durumdaysak h�z zaten 0 
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
          } trigger ile hasar alma mant�g� olsayd� bunu kullanabilirdik damagable instance'�na gerek kalmzaDI*/

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchDirection = GetComponent<TouchDirection>();
        damageable = GetComponent<Damageable>();
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
       // leftM = GameObject.Find("LButton").GetComponent<Button>();
       // rightM = GameObject.Find("RButton").GetComponent<Button>();
        dashB = GameObject.Find("dashButton").GetComponent<Button>();
        manager = audiomanager.Instance;
        attack=GetComponentInChildren<playerAttack>();
        staminaBar = GameObject.Find("Stamina Bar").GetComponent<StaminaBar>();

        SceneManager.sceneLoaded += OnSceneLoaded;
        if (manager == null)
        {
            //Debug.LogError("AudioManager instance bulunamad� player Movement");
        }
        dashBar = FindObjectOfType<DashBar>();

        attackB.onClick.RemoveAllListeners();
        attackB.onClick.AddListener(OnAttackButtonPressed);

        attackB.onClick.AddListener(OnAttackButtonPressed);
        JumpB.onClick.AddListener(onJumpButtonPressed);
        fireB.onClick.AddListener(onFireButtonPressed);
        dashB.onClick.AddListener(OnDashButtonPressed);

      //  AddEventTrigger(leftM, (data) => onLeftButtonPressed(), EventTriggerType.PointerDown);
       // AddEventTrigger(leftM, (data) => onLeftButtonReleased(), EventTriggerType.PointerUp);


      //  AddEventTrigger(rightM, (data) => onRightButtonPressed(), EventTriggerType.PointerDown);
       // AddEventTrigger(rightM, (data) => onRightButtonReleased(), EventTriggerType.PointerUp);

        fixedJoystick = GameObject.Find("Fixed Joystick").GetComponent<FixedJoystick>();
        //joystick

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

        // Olay� ba�la
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
        moveInput = new Vector2(-1, 0);
       // if (IsAlive)
       // {
            IsMoving = true;
      //  }
        // Debug.Log("Sol tu� bas�ld�");
    }

    public void onRightButtonPressed()
    {
        moveInput = new Vector2(1, 0);
       // if (IsAlive)
   //     {
            IsMoving = true;
     //   }
        //Debug.Log("Sa� tu� bas�ld�.");
    }
    public void onLeftButtonReleased()
    {
        moveInput = Vector2.zero;
        IsMoving = false;
        //Debug.Log("Sol tu� b�rak�ld�, karakter durmal�");
    }

    public void onRightButtonReleased()
    {
        moveInput = Vector2.zero;
        IsMoving = false;
        //  Debug.Log("Sa� tu� b�rak�ld�, karakter durmal�");
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (IsAlive)
        {
            IsMoving = moveInput != Vector2.zero; // Karakterin hareket edip etmedi�ini kontrol et

        }
        else
        {
            IsMoving = false;
        }
    }

    [SerializeField] private float footstepInterval = 0.5f; // �ki ad�m sesi aras�ndaki s�re
    private float footstepTimer;

    private void HandleFootstepSound()
    {
        if (IsMoving && touchDirection.IsGrounded)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                manager.PlaySFX(manager.groundTouch, cvolume);
                footstepTimer = footstepInterval; // Zamanlay�c�y� yeniden ba�lat
            }
        }
        else
        {
            footstepTimer = 0f; // Hareket etmezse zamanlay�c� s�f�rlan�r
        }
    }





    public void onJumpButtonPressed()
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
    }



    /*public void OnJump(InputAction.CallbackContext context)
    {
        /* bas�p b�rakt���nda daha az z�plama. 
         if (context.canceled && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        
        //canl� olup olmad�g�n� da kontrol edicz
        // Z�plamay� ba�latma
        if (context.started && (touchDirection.IsGrounded || coyoteTimeCounter > 0f) && CanMove)
        {
   
          //  PlayDust();
          //  rb.AddForce(Vector2.up * Jump, ForceMode2D.Impulse);
          //
          //  hangTimeCounter = hangTime; // Hang time ba�lat�l�yor
          //  manager.PlaySFX(manager.pjump, 0.8f);
        }

        // Z�plama iptalini y�netmek (�rne�in, d��meye k�sa s�re basarak z�plamay� iptal etme)
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
        if (!isAttacking) // ztn sald�rma animasyonu oynam�yorsa
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
        if (context.started && !isAttacking) // ztn sald�rma animasyonu oynam�yorsa
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
            yield break; // E�er dash yap�lam�yorsa veya zaten dash yap�l�yorsa ��k
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

       canDash = true; // Cooldown s�resi doldu, dash tekrar kullan�labilir
    }
    private void Update()
    {
        if (isDashing)
        {
            return;
        }
        animator.SetFloat(AnimStrings.yVelocity, rb.velocity.y);
        if (IsAlive)
        {
            if (moveInput.x > 0)
            {
                if (lastXDirection <= 0) // Y�n de�i�tiriyorsa
                {
                    transform.localScale = new Vector2(3, 3);
                    PlayDust();
                }
                lastXDirection = 1;
            }
            else if (moveInput.x < 0)
            {
                if (lastXDirection >= 0) // Y�n de�i�tiriyorsa
                {
                    transform.localScale = new Vector2(-3, 3);
                    PlayDust();
                }
                lastXDirection = -1;
            }
            HandleFootstepSound();
            // Y�r�me sesi kontrol� FixedUpdate i�inde yap�lacak

        }
       
    }
    private void MoveCharacter(Vector2 direction)
    {
        // Hareket vekt�r� olu�tur
        Vector3 movement = new Vector3(direction.x, 0, 0) * speed;

        // Rigidbody'nin h�z�n� ayarla
        rb.velocity = movement;
    }

    private void FixedUpdate()
    {
        if (fixedJoystick != null)
        {
            moveInput = new Vector2(fixedJoystick.Horizontal, 0);
            IsMoving = (Mathf.Abs(moveInput.x) > 0); ;
        }


        if (IsMoving)
        {

            MoveCharacter(moveInput);
        }
        if (isDashing)
        {
            return;
        }
        if (isAttacking) 
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                OnAttackAnimationFinished(); // Sald�r� zamanlay�c� s�resi doldu
            }
            //CanMove = false;
        }
        if (touchDirection.IsGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            IsJumping = false;
            currentJumpCount = maxJumpCount;
            //     doubleJump = true;//zemine carap�nca doublejump tekrar aktif
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
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
            CanMove = true;
            if (!dashBar.IsCooldownActive())
            {
                canDash = true;
            }
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y); // Diyalog s�ras�nda hareket edemiycez. �leride animasyonu da kapatabilirim
            CanMove = false;
            canDash = false;
        }
        #endregion

        if (platformTransform != null)
        {
            Vector2 platformVelocity = platformTransform.GetComponent<Rigidbody2D>().velocity;
            rb.velocity = new Vector2(rb.velocity.x + platformVelocity.x, rb.velocity.y);
        }





        if (rb.velocity.y < 0) // Karakter d����teyken
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y > 0) // Karakter y�kseliyorken
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier * 0.5f - 1) * Time.fixedDeltaTime;
        }








        /* if (Input.GetKey(KeyCode.Space))
         {
             rb.velocity= new Vector2(rb.velocity.x, Jump);
         } */
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
        // Platforma temas etti�inde platformun transform'unu al�yoruz
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            platformTransform = collision.transform;

        }
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("MovingPlatform"))
        {
            _isJumping = false; // Z�plama durumu s�f�rlan�yor
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Platformdan ayr�ld���nda platform transform'u null yap�l�r
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            platformTransform = null;
        }


    }

}
