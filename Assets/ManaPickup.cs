using UnityEngine;

public class ManaPickup : MonoBehaviour
{
    public int manaRestore = 1;  
    public Vector3 rotationSpeed = new Vector3(0, 180, 0); 
    [SerializeField] float dropForce = 5f;  
    [SerializeField] float volume = 0.5f;   
    audiomanager manager;                   
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();  
        rb.AddForce(Vector2.up * dropForce, ForceMode2D.Impulse);  
        manager = audiomanager.Instance;
        if (manager == null)
        {
            Debug.LogError("AudioManager instance bulunamadý ManaPickup.");
        }
    }

    void Update()
    {
        // Pickup nesnesinin dönmesini saðlar
        transform.eulerAngles += rotationSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
          
            ManaBar manaBar = gameManager.instance.GetManaBar();
            if (manaBar != null)
            {
                bool isRestored = manaBar.RestoreMana(manaRestore);
                if (isRestored)
                {
                    manager.PlaySFX(manager.manaPickup, volume);
                }
                Destroy(gameObject);  
            }
        }
    }
}
