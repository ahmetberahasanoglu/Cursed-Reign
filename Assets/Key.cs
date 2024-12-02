using UnityEngine;

public class Key : MonoBehaviour
{
    private Door door;
    Rigidbody2D rb;
    [SerializeField] float dropForce = 5f;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.up * dropForce, ForceMode2D.Impulse);
        door = FindObjectOfType<Door>();
        if (door == null)
        {
            Debug.LogError("Kapý referansý atanmamýþ!");
        }
    }

    public void OnKeyCollected()
    {
        door.Open(); 
       
    }
}
