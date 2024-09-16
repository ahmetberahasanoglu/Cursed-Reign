using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDirection : MonoBehaviour
{
    [SerializeField] ContactFilter2D castFilter;
    [SerializeField] float groundDistance = 0.05f;
    [SerializeField] float tavanDistance = 0.05f;
    [SerializeField] float wallDistance = 0.2f;
    CapsuleCollider2D touchCol;
    RaycastHit2D[] groundHit = new RaycastHit2D[5];
    RaycastHit2D[] wallHit = new RaycastHit2D[5];
    RaycastHit2D[] tavanHit = new RaycastHit2D[5];
    private Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;
    Animator animator;

    [SerializeField] private bool _isGrounded;
    public bool IsGrounded
    {
        get { return _isGrounded; }
        private set
        {
            _isGrounded = value;
            animator.SetBool(AnimStrings.isGrounded, value);
        }
    }

    [SerializeField] private bool _isOnWall;
    public bool IsOnWall
    {
        get
        {
            return _isOnWall;
        }
        private set
        {
            _isOnWall = value;
            animator.SetBool(AnimStrings.isOnWall, value);
        }
    }

    [SerializeField] private bool _isOnTavan;

    public bool IsOnTavan
    {
        get
        {
            return _isOnTavan;
        }
        private set
        {
            _isOnTavan = value;
            animator.SetBool(AnimStrings.isOnTavan, value);
        }
    }
 
    private void Awake()
    {
        touchCol = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        IsGrounded = touchCol.Cast(Vector2.down, castFilter, groundHit, groundDistance) > 0;
        IsOnWall = CheckWallCollision();
        IsOnTavan = touchCol.Cast(Vector2.up, castFilter, tavanHit, tavanDistance) > 0;
    }

    private void OnDrawGizmos()
    {
        if (touchCol == null) return;

        // Ground check ray
        Gizmos.color = Color.green;
        Gizmos.DrawLine(touchCol.bounds.center, touchCol.bounds.center + Vector3.down * groundDistance);

        // Wall check ray
        Gizmos.color = Color.red;
        Gizmos.DrawLine(touchCol.bounds.center, touchCol.bounds.center + (Vector3)wallCheckDirection * wallDistance);

        // Ceiling check ray (tavan)
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(touchCol.bounds.center, touchCol.bounds.center + Vector3.up * tavanDistance);
    }
    private bool CheckWallCollision()
    {
        int hitCount = touchCol.Cast(wallCheckDirection, castFilter, wallHit, wallDistance);
        for (int i = 0; i < hitCount; i++)
        {
            if (wallHit[i].collider.CompareTag("Player"))
            {
                // Oyuncu ise bunu yok sayýyoruz
                continue;
            }

            // Eðer oyuncu deðilse, bu bir duvar olabilir
            return true;
        }
        return false;
    }

}