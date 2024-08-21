using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healthRestore = 10;
    public Vector3 rotationSpeed=new Vector3(0,180,0);
    AudioSource pickupSource;

    // Start is called before the first frame update
    private void Awake()
    {
        pickupSource = GetComponent<AudioSource>();
    }
   
    void Update()
    {
        transform.eulerAngles += rotationSpeed* Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable != null) {
           bool isHealed= damageable.Heal(healthRestore);
            if(isHealed) {  
                AudioSource.PlayClipAtPoint(pickupSource.clip, gameObject.transform.position, pickupSource.volume);
                Destroy(gameObject);
            }//can doldurduysak alýocak silmediysek almayacak
            
        }
    }
}
