using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    public GameObject dialogueBox;         // Assign DialogueBox prefab/UI
    public AudioSource audioSource;
    [TextArea(3,5)]
    public string[] npcLines;              // Each NPC can have its own lines

    private Dialogue dialogueScript;

    private void Start()
    {
        dialogueScript = dialogueBox.GetComponent<Dialogue>();
    }

    private void OnTriggerEnter(Collider other)
    {
       if (other.CompareTag("Player"))
        {
            dialogueBox.SetActive(true);
            
            // Override the DialogueBox lines with this NPC's lines
            dialogueScript.lines = npcLines;
            dialogueScript.StartDialogue();
            
            // Play audio
            if(audioSource != null) 
            {
                audioSource.Play();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueBox.SetActive(false);
        }
    }
}
