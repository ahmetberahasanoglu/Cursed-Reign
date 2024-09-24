using UnityEngine;

public class FootCheck : MonoBehaviour
{
    public float launchForce = 20f;
    public Transform groundCheck;
    public Rigidbody2D playerRB;
    public LayerMask jumpPadLayer;
    public Animator mushroomAnimator; // Mantar�n animator bile�eni
    [SerializeField] private float volume;
    audiomanager manager;
    private void Start()
    {
        manager = audiomanager.Instance;
    }
    private void FixedUpdate()
    {
        // E�er mantar�n �zerine bas�ld�ysa z�plama yap�l�r
        Collider2D jumpPadCollider = Physics2D.OverlapCircle(groundCheck.position, 0.1f, jumpPadLayer);

        if (jumpPadCollider != null)
        {
            // Z�plama kuvvetini uygula
            playerRB.velocity = new Vector2(playerRB.velocity.x, launchForce);

            // E�er mantar�n Animator'u varsa, bounce animasyonunu tetikle
            if (mushroomAnimator != null)
            {
                mushroomAnimator.SetTrigger("bounceTrigger");
                manager.PlaySFX(manager.bounceSound, volume);
            }
        }
    }
}
