using UnityEngine;

public class FootCheck : MonoBehaviour
{
    public float launchForce = 20f;
    public Transform groundCheck;
    public Rigidbody2D playerRB;
    public LayerMask jumpPadLayer;
    public Animator mushroomAnimator; 
    public Animator statueAnimator; //Animator'ler bu sahnede deðil baþka sahnede var dolayýsýyla bunu her sahnede aramasý gerekiyor olabilir
    [SerializeField] private float volume;
    audiomanager manager;
    private void Start()
    {
        manager = audiomanager.Instance;
    }
    private void FixedUpdate()
    {
        // Eðer mantarýn üzerine basýldýysa zýplama yapýlýr
        Collider2D jumpPadCollider = Physics2D.OverlapCircle(groundCheck.position, 0.1f, jumpPadLayer);

        if (jumpPadCollider != null)
        {
            // Zýplama kuvvetini uygula
            playerRB.velocity = new Vector2(playerRB.velocity.x, launchForce);

            // Eðer mantarýn Animator'u varsa, bounce animasyonunu tetikle
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
