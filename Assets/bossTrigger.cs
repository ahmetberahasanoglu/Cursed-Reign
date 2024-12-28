using UnityEngine;

public class bossTrigger : MonoBehaviour
{
    private audiomanager manager;
    public boss bossObject;
    public AudioClip bossMusic;


    void Start()
    {
        manager = audiomanager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            manager.PlayEventMusic(manager.bossMusic);
            bossObject.SetBossAnimatorBool(true);
            Destroy(gameObject);
        }
    }

}
