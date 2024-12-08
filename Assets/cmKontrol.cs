using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cmKontrol : MonoBehaviour
{
    public CinemachineVirtualCamera camera;
    GameObject player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (player != null)
            {
                camera.Follow = player.transform;
            }
        }

    }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

   
}
