using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;

    private int index;

    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            if (textComponent.text == lines[index]) {
                NextLine();
            }
            else {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    public void StartDialogue() {
        index = 0;
        textComponent.text = string.Empty; 
        StopAllCoroutines();               
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine() {
        foreach (char c in lines[index].ToCharArray()) {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine() {
        if (index < lines.Length -1) {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else {
            gameObject.SetActive(false);
        }
    }

    public void AddLine(string line)
    {
        // Activate the dialogue box if it’s inactive
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        // Replace previous lines with the new line
        lines = new string[] { line };

        // Reset index to 0
        index = 0;

        // Stop any typing in progress
        StopAllCoroutines();

        // Clear any existing text
        textComponent.text = string.Empty;

        // Start typing the new line
        StartCoroutine(TypeLine());
    }
}
