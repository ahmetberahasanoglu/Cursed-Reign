using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public Image characterIcon;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;
    audiomanager manager;
    private Queue<DialogueLine> lines;

    public bool isDialogueActive = false;
    public float typingSpeed = 0.2f;
    public Animator animator;
    public GameObject shopCanvas;

    // Shop g�sterilip g�sterilmeyece�ini belirleyen flag
    public bool isShopShow = false;
    private void Start()
    {
        manager = audiomanager.Instance;
    }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        lines = new Queue<DialogueLine>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        isDialogueActive = true;

        animator.Play("show");

        lines.Clear();

        foreach (DialogueLine dialogueLine in dialogue.dialogueLines)
        {
            lines.Enqueue(dialogueLine);
        }

        DisplayNextDialogueLine();
    }

    public void DisplayNextDialogueLine()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine currentLine = lines.Dequeue();

        characterIcon.sprite = currentLine.character.icon;
        characterName.text = currentLine.character.name;

        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentLine));
    }

    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        dialogueArea.text = "";
        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            dialogueArea.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        animator.Play("hide");

        // Diyalog bittikten sonra shop'un g�sterilme durumu kontrol edilir
        if (isShopShow)
        {
            OpenShop();
        }
    }

    // ShopCanvas'� aktif eden fonksiyon
    void OpenShop()
    {
        if (shopCanvas != null)
        {
            Instantiate(shopCanvas);
            manager.PlaySFX(manager.shopOpen,1f);
        }
        else
        {
            Debug.LogError("ShopCanvas referans� atanmad�.");
        }
    }

    // Sahneye g�re isShopShow'u g�ncelleyen fonksiyon
    public void UpdateShopVisibility(bool showShop)
    {
        isShopShow = showShop;
    }
}
