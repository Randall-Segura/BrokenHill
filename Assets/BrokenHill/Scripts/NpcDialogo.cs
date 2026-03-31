using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NpcDialogo : MonoBehaviour
{
   [Header("Diálogo")]
    public string npcName = "Aldeano";
    
    [TextArea(2, 5)]
    public string[] dialogueLines; // Las frases del NPC}


    [Header("UI")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    [Header("Configuración")]
    public float detectionRadius = 2f;
    public KeyCode interactKey = KeyCode.E;
    public float typingSpeed = 0.05f;

    private bool playerInRange = false;
    private bool isTyping = false;
    private int currentLine = 0;
    private bool dialogueActive = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            if (!dialogueActive)
                StartDialogue();
            else if (!isTyping)
                NextLine();
        }
    }

    void StartDialogue()
    {
        dialogueActive = true;
        currentLine = 0;
        dialoguePanel.SetActive(true);
        nameText.text = npcName;
        StartCoroutine(TypeLine(dialogueLines[currentLine]));
    }

    void NextLine()
    {
        currentLine++;

        if (currentLine < dialogueLines.Length)
            StartCoroutine(TypeLine(dialogueLines[currentLine]));
        else
            EndDialogue();
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void EndDialogue()
    {
        dialogueActive = false;
        dialoguePanel.SetActive(false);
        currentLine = 0;
    }

    // Detección por proximidad con un Collider2D en trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            EndDialogue();
        }
    }
}
