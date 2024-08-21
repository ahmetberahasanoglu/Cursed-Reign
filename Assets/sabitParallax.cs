using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sabitParallax : MonoBehaviour

{
    [SerializeField] private Transform cameraTransform;

    void Update()
    {
        // Güneþi kameraya göre sabit bir pozisyonda tut
        transform.position = new Vector3(cameraTransform.position.x, cameraTransform.position.y, transform.position.z);
    }
}
