using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crackable : MonoBehaviour
{
    bool hitted = false; // Vurulmuþ olup olmadýðýný takip eder
    Animator animator;
 

    [Header("Loot")]
    public List<LootItem> lootTable = new List<LootItem>();

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (hitted)
            return;

        
        animator.SetTrigger("hitted");
        hitted = true; 

        // Loot tablosunu kontrol et
        foreach (LootItem item in lootTable)
        {
            if (Random.Range(0f, 100f) <= item.dropChance)
            {
                InstantiateLoot(item.itemPrefab);
                break;
            }
        }
    }

    void InstantiateLoot(GameObject loot)
    {
        if (loot)
        {

            GameObject droppedLoot = Instantiate(loot, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            // droppedLoot.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
}
