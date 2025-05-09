using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FootCheck : MonoBehaviour
{
    public float launchForce = 20f;
    public Transform groundCheck;
    public Rigidbody2D playerRB;
    public LayerMask jumpPadLayer;
    public Animator mushroomAnimator; 
    public Animator statueAnimator; //Animator'ler bu sahnede değil başka sahnede var dolayısıyla bunu her sahnede araması gerekiyor olabilir
    [SerializeField] private float volume;
    audiomanager manager;
    private void Start()
    {
        manager = audiomanager.Instance;
        
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Sahne değiştirilince olay bağlantısını kaldırıyoruz
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "level1"|| scene.name=="level2") 
        {
            GameObject mushroom = GameObject.FindWithTag("Mushroom");
            mushroomAnimator = mushroom.GetComponent<Animator>();
            launchForce = 19f;
            
        }
     
      
        else if (scene.name=="level3")
        {
            GameObject mushroom = GameObject.FindWithTag("Mushroom");
            mushroomAnimator = mushroom.GetComponent<Animator>();
            launchForce = 16f;
        }
        else if (scene.name == "level4")
        {
            GameObject mushroom = GameObject.FindWithTag("Mushroom");
            mushroomAnimator = mushroom.GetComponent<Animator>();
            launchForce = 16f;

        }
        
        else
        {
          
            mushroomAnimator = null;
            statueAnimator = null;
        }
    }
    private void FixedUpdate()
    {
   
        Collider2D jumpPadCollider = Physics2D.OverlapCircle(groundCheck.position, 0.1f, jumpPadLayer);

        if (jumpPadCollider != null)
        {
       
            playerRB.velocity = new Vector2(playerRB.velocity.x, launchForce);

          
            if (mushroomAnimator != null)
            {
                mushroomAnimator.SetTrigger("bounceTrigger");
                manager.PlaySFX(manager.bounceSound, volume);
            }
            if (statueAnimator != null)
            {
                statueAnimator.SetTrigger("bounceTrigger");
                manager.PlaySFX(manager.bounceSound, volume);
            }
        }
    }
}
