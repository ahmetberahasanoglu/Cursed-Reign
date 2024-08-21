using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;
    public UnityEvent damageableDeath;
    public UnityEvent<int, int> healthChanged;
    Animator animator;
 
    [SerializeField] int maxHealth = 100;
    
    public int MaxHealth {
        get
        {
            return maxHealth;
        }
        set {

            maxHealth = value;
        }
    }
    [SerializeField] private int _health = 100;
    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
            healthChanged?.Invoke(_health, MaxHealth);
            if (_health <= 0)
            {
                IsAlive = false;
            }
        }
    }
    [SerializeField] private bool isAlive = true;
    [SerializeField] private bool isInvincible=false;//vurulduktan sonra bir süre hasar almaz olmalý
  
  public bool IsHit { get 
        { 
            return animator.GetBool(AnimStrings.isHit); 
        }
        set
        {
            animator.SetBool(AnimStrings.isHit, value);
        }
        } 
    private float vurulduktanSonraGecenZman=0;
    [SerializeField] private float invincibilityTime=0.2f;
   
    public bool IsAlive { 
        get { 
            return isAlive;
        }
        set { isAlive= value;
            animator.SetBool(AnimStrings.isAlive, value);
            Debug.Log(value);

            if(value==false )
            {
                damageableDeath.Invoke();
            }
        }
    }
    private void Awake()
    {
        
        animator = GetComponent<Animator>();

    }
    public bool Hit(int damage, Vector2 knockback)
    {
        if (IsAlive && !isInvincible)
        {
            Health -= damage;
            isInvincible = true;

            IsHit = true;
            damageableHit?.Invoke(damage, knockback);
         
            //CharacterEvents.characterDamaged.Invoke(gameObject, damage);

            return true;
        }

        // Unable to be hit
        return false;
    }
    public bool Heal(int healAmount) {
        if(IsAlive && Health<maxHealth)
        {
            int maxHeal = Mathf.Max(maxHealth - Health, 0);//health max healthtan buyuk olursa sýfýr dondurucez
            Health +=Mathf.Min(maxHeal,healAmount);
            return true;
        }
        return false;
    }
   
    void Update()
    {
        if (isInvincible)
        {
            if(vurulduktanSonraGecenZman >invincibilityTime)
            {
                isInvincible=false;
                vurulduktanSonraGecenZman = 0;
            }
            vurulduktanSonraGecenZman += Time.deltaTime;
        }
        
    }
}
