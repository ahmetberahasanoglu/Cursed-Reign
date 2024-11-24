using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowTrap : MonoBehaviour
{
    [SerializeField] GameObject arrow;
    [SerializeField] private Transform firePoint;
    public void ArrowSend()
    {
        Instantiate(arrow, firePoint.position, firePoint.rotation);
    }
    IEnumerator asd()
    {

        while (true)
        {
            ArrowSend();
            yield return new WaitForSeconds(2000); 
        }
    }
}
