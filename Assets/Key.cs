using UnityEngine;

public class Key : MonoBehaviour
{
    private Door door; 

    private void Start()
    {
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
