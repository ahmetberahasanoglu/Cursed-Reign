using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text manaText;
    private string manaFormat = "{0}/{1}";
    Animator animator;
    private int maxMana;
    [SerializeField] private int currentMana;

    void Start()
    {
        animator = GetComponent<Animator>();
        maxMana = 3; 
        currentMana = 0;
        slider.value = 0;
        SetMaxMana(maxMana);
    }

    public void SetMaxMana(int maxMana)
    {
        this.maxMana = maxMana;
        slider.maxValue = maxMana;
        UpdateManaText(currentMana, maxMana);
    }

    public void SetMana(int mana)
    {
        currentMana = mana;
        slider.value = mana;
        UpdateManaText(currentMana, maxMana);
    }

    public bool RestoreMana(int amount)
    {
        if (currentMana < maxMana)
        {
            currentMana = Mathf.Min(currentMana + amount, maxMana);
            SetMana(currentMana); // Mana güncelleniyor
            return true;
        }
        return false;
    }
    public void noMana()
    {
        animator.SetTrigger("noMana");
    }
    public int GetCurrentMana()
    {
        return currentMana;
    }

    public int GetMaxMana()
    {
        return maxMana;
    }

    private void UpdateManaText(int currentMana, int maxMana)
    {
        manaText.text = string.Format(manaFormat, currentMana, maxMana);
    }
}
