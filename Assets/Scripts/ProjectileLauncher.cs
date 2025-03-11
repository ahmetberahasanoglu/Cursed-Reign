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
    audiomanager manager;

    private void Start()
    {
        manaBar = GameObject.Find("Mana Bar").GetComponent<ManaBar>();
        currentMana = manaBar.GetCurrentMana();
        maxMana = manaBar.GetMaxMana();
        manager = audiomanager.Instance;
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
            manager.PlaySFX(manager.laser, 0.2f);
            currentMana--;
            manaBar.SetMana(currentMana);
        }
        else
        {
            manaBar.noMana();
        }
    }

}
