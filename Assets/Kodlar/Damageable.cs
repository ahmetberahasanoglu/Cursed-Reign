using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    Animator animator;
    [SerializeField] int maxHealth = 100;
    public int MaxHeath {
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
            if (_health < 0)
            {
                IsAlive = false;
            }
        }
    }
    [SerializeField] private bool isAlive = true;
    [SerializeField] private bool isInvincible=false;//vurulduktan sonra bir süre hasar almaz olmalý
    private float vurulduktanSonraGecenZman=0;
    [SerializeField] private float invincibilityTime=0.2f;

    public bool IsAlive { 
        get { 
            return isAlive;
        }
        set { isAlive= value;
            animator.SetBool(AnimStrings.isAlive, value);
            Debug.Log(value);
        }
    }
    private void Awake()
    {
        
        animator = GetComponent<Animator>();
    }
    public void Hit(int Damage)
    {
        if (IsAlive && !isInvincible)
        {
            Health -= Damage;
            isInvincible = true;
        }
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
