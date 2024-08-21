using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootCheck : MonoBehaviour
{
    public float launchForce = 20f;
    public Transform groundCheck;
    public Rigidbody2D playerRB;
    public LayerMask jumpPadLayer;

    private void FixedUpdate()
    {
        if (Physics2D.OverlapCircle(groundCheck.position, 0.1f, jumpPadLayer))
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, launchForce);
        }
    }
}
