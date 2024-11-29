using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public Transform launchPoint;
    public GameObject projectilePrefab;
    public ManaBar manaBar;
    public int maxMana;
    private int currentMana;

    private void Start()
    {
        manaBar = GameObject.Find("Mana Bar").GetComponent<ManaBar>();
        currentMana = manaBar.GetCurrentMana();
        maxMana = manaBar.GetMaxMana();
       
       // manaBar=FindObjectOfType<ManaBar>();
        
    }
    private void FixedUpdate()
    {
        currentMana=manaBar.GetCurrentMana();
        maxMana = manaBar.GetMaxMana();
        
    }
    public void FireProjectile()
    {
        if (currentMana > 0)
        {
            GameObject projectile = Instantiate(projectilePrefab, launchPoint.position, projectilePrefab.transform.rotation);
            Vector3 originScale = projectile.transform.localScale;
            projectile.transform.localScale = new Vector3(originScale.x * (transform.localScale.x > 1 ? 1 : -1), originScale.y, originScale.z);

            currentMana--;
            manaBar.SetMana(currentMana);
        }
        else
        {
            Debug.Log("Yetersiz mana!");
        }
    }

}
