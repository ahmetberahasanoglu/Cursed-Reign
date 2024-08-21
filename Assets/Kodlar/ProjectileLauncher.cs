using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public Transform launchPoint;
    public GameObject projectilePrefab;
    public ManaBar manaBar;
    public int maxMana = 3;
    private int currentMana;

    private void Start()
    {
        currentMana = maxMana;
        manaBar.SetMaxMana(maxMana);
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

    public void RegenerateMana(int amount)
    {
        currentMana = Mathf.Min(currentMana + amount, maxMana);
        manaBar.SetMana(currentMana);
    }
}
