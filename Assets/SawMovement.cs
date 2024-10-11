using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float waypointReachedDistance = 0.1f; // Waypoint'e ne kadar yakla��nca duracak
    public List<Transform> waypoints;
    public int damage = 1;

    private int currentWaypointIndex = 0;
    private Transform nextWaypoint;
    private Rigidbody2D rb;

    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        nextWaypoint = waypoints[currentWaypointIndex];
    }

    private void FixedUpdate()
    {
        MovePlatform();
    }

    private void MovePlatform()
    {
        if (nextWaypoint != null)
        {
            Vector2 direction = (nextWaypoint.position - transform.position).normalized;
            rb.velocity = direction * moveSpeed;

            // Waypoint'e ula��p ula�mad���n� kontrol et
            if (Vector2.Distance(transform.position, nextWaypoint.position) < waypointReachedDistance)
            {
                // Bir sonraki waypoint'e ge�
                currentWaypointIndex++;
                if (currentWaypointIndex >= waypoints.Count)
                {
                    currentWaypointIndex = 0; // E�er son waypoint'e ula�t�ysa, ba�a d�n
                }
                nextWaypoint = waypoints[currentWaypointIndex];
            }
        }
    }

  


}
