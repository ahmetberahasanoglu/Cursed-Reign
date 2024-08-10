using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Transform followTarget;

    Vector2 startingPos;

    float startingZ;
    Vector2 camMoveSinceStart => (Vector2)cam.transform.position - startingPos;
    float zDistanceFromTarget => transform.position.z - followTarget.transform.position.z;

    float clippingZone => (cam.transform.position.z + (zDistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));
    float parallaxFactor => Mathf.Abs(zDistanceFromTarget) / clippingZone;
    void Start()
    {
        startingPos = transform.position;
        startingZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPosition = startingPos + camMoveSinceStart * parallaxFactor;
        transform.position = new Vector3(newPosition.x, newPosition.y, startingZ);

    }
}