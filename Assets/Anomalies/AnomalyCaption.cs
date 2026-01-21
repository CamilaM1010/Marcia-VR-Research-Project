using UnityEngine;
using TMPro;
using System.Collections;

public class AnomalyCaption : MonoBehaviour
{
    public TextMeshProUGUI captionText;
    public float textSpeed = 0.05f; 
    public float displayTime = 2.5f;
    public Vector3 offset = new Vector3(0, 2f, 0); // above the anomaly

    private Coroutine typingCoroutine;
    private Transform target;

    void LateUpdate()
    {
        // Only follow target if there is one
        if (target != null)
        {
            // Move above the target
            transform.position = target.position + offset;

            // Face the camera
            if (Camera.main != null)
                transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        }
    }

    public void ShowCaption(string line, Transform followTarget)
    {
        target = followTarget;

        // Activate the panel if it’s inactive
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        // Stop any ongoing typing
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeLine(line));
    }

    private IEnumerator TypeLine(string line)
    {
        captionText.text = "";

        // Type out the text
        foreach (char c in line)
        {
            captionText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        // Wait for displayTime
        yield return new WaitForSeconds(displayTime);

        // Clear text and hide panel completely
        captionText.text = "";
        gameObject.SetActive(false);

        // Reset target so LateUpdate does nothing
        target = null;
    }
}
