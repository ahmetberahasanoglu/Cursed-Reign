using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public Transform launchPoint;
    public GameObject projectilePrefab;
    public void FireProjectile()
    {
       GameObject projectile= Instantiate(projectilePrefab, launchPoint.position, projectilePrefab.transform.rotation);   
        Vector3 originScale=projectile.transform.localScale;
        projectile.transform.localScale=new Vector3(originScale.x *transform.localScale.x > 1? 1:-1,originScale.y, originScale.z);
    }
    
   
}
