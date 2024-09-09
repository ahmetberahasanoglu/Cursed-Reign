using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healthRestore = 10;
    public Vector3 rotationSpeed=new Vector3(0,180,0);
    [SerializeField] float dropForce = 5f;
    [SerializeField] float volume = 0.5f;
    audiomanager manager;
    Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.up*dropForce,ForceMode2D.Impulse);
        manager = audiomanager.Instance;
        if (manager == null)
        {
            Debug.LogError("AudioManager instance bulunamadý player Movement");
        }
    }
    void Update()
    {
        transform.eulerAngles += rotationSpeed* Time.deltaTime;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Damageable damageable = collision.gameObject.GetComponent<Damageable>();

        if (damageable != null)
        {
            bool isHealed = damageable.Heal(healthRestore);
            if (isHealed)
            {
                manager.PlaySFX(manager.healthPickup,volume);
             
            }//can doldurduysak alýocak silmediysek almayacak
            Destroy(gameObject);
        }
    }
  
}
