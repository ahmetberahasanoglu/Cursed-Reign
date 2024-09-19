using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float waypointReachedDistance = 0.1f; // Waypoint'e ne kadar yaklaþýnca duracak
    public List<Transform> waypoints; 

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

            // Waypoint'e ulaþýp ulaþmadýðýný kontrol et
            if (Vector2.Distance(transform.position, nextWaypoint.position) < waypointReachedDistance)
            {
                // Bir sonraki waypoint'e geç
                currentWaypointIndex++;
                if (currentWaypointIndex >= waypoints.Count)
                {
                    currentWaypointIndex = 0; // Eðer son waypoint'e ulaþtýysa, baþa dön
                }
                nextWaypoint = waypoints[currentWaypointIndex];
            }
        }
    }

  /*  private void OnDrawGizmos()
    {
        // Waypoint'leri görselleþtirmek için gizmos çizimleri
        Gizmos.color = Color.green;
        foreach (Transform waypoint in waypoints)
        {
            Gizmos.DrawSphere(waypoint.position, 0.2f);
        }
    }*/
    

}
