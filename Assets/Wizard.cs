using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using static skeleton; // Eðer `skeleton` sýnýfýndaki bir þeye ihtiyacýnýz yoksa, bu satýrý kaldýrabilirsiniz.

[RequireComponent(typeof(Rigidbody2D), typeof(TouchDirection))]
public class Wizard : MonoBehaviour
{
    #region Public degiskenler
    public Transform raycast;
    public LayerMask raycastMask;
    public float rayCastLength;
    public HealthBar healthBar;
    public float attackDistance;
    public DetectionZone attackZone;
    public DetectionZone cliffDetection;
    public float moveSpeed;
    public float timer;
    #endregion

    #region Privates
    private RaycastHit2D hit;
    private GameObject target;
    private Animator anim;
    TouchDirection touchDirection;
    Damageable damageable;
    Rigidbody2D rb;
    private float distance;
    private bool attackMode;
    private bool inRange;
    private bool cooling;
    private float initTimer;
    #endregion

    private void Awake()
    {
        initTimer = timer;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        touchDirection = GetComponent<TouchDirection>();
        healthBar.SetHealth(damageable.Health, damageable.MaxHealth);
    }
    private void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;
        healthBar.SetHealth(damageable.Health, damageable.MaxHealth);
        if (inRange)
        {
            hit = Physics2D.Raycast(raycast.position, Vector2.left, rayCastLength, raycastMask);
            RaycastDebugger();
        }

        if(hit.collider!=null)
        {
            EnemyLogic();
        }
        else if (hit.collider == null)
        {
            inRange = false;

        }
        if (inRange == false)
        {
            anim.SetBool("canMove", false);
            StopAttack();
        }
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
            anim.SetBool(AnimStrings.hasTarget, value);
        }

    }
    private void FixedUpdate()
    {
        if (touchDirection.IsOnWall && touchDirection.IsGrounded)        
        {
            FlipDirection();
        }
    }

    private void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position, target.transform.position);
        if (distance > attackDistance)
        {
            Move();
            StopAttack();
        }
        else if (attackDistance>=distance &&cooling==false)
        {
            AttackFunc();
        }
        if (cooling)
        {
            Cooldown();
            anim.SetBool("hasTarget", false);
        }
    }

    private void AttackFunc()
    {
        timer = initTimer;
        attackMode = true;

        anim.SetBool("canMove", false);
        anim.SetBool("hasTarget", true);
    }

    private void Move()
    {
        anim.SetBool("canMove", true);
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("hasTarget"))
        {
            Vector2 targetPosition = new Vector2(target.transform.position.x,transform.position.y);
            transform.position=Vector2.MoveTowards(transform.position,targetPosition,moveSpeed*Time.deltaTime);
        }
    }

    private void StopAttack()
    {
     cooling = false;
        attackMode= false;
        anim.SetBool("hasTarget", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            target=collision.gameObject;
        }
    }
    void RaycastDebugger()
    {
        if (distance > attackDistance)
        {
            Debug.DrawRay(raycast.position,Vector2.left*rayCastLength,Color.red);
        }
        else if(attackDistance>distance) {
            Debug.DrawRay(raycast.position, Vector2.left * rayCastLength, Color.green);
        }

    }
    public void TriggerCooling()
    {
        cooling=true;
    }
    void Cooldown()
    {
        timer -=Time.deltaTime;

        if(timer<=0&& cooling&&attackMode)
        {
            cooling = false;
            timer = initTimer;
        }
    }
    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }
    public void OnCliffDetected()
    {
        if (touchDirection.IsGrounded)
        {
            FlipDirection();
        }
    }
    private void FlipDirection()
    {
        // Karakterin yönünü deðiþtir
        transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, transform.localScale.y);
    }
}
