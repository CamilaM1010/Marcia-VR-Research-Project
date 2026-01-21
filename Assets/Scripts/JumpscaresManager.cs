using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JumpscaresManager : MonoBehaviour
{
    [Header("UI Reference")]
    public Toggle jumpscareToggle;

    [Header("Jumpscare Settings")]
    [Tooltip("All possible jumpscare images. One will be chosen at random each time.")]
    public GameObject[] scareImages;

    [Tooltip("Sound to play during scare (optional).")]
    public AudioSource scareSound;

    [Tooltip("How long the jumpscare is visible.")]
    public float scareDuration = 0.5f;

    [Tooltip("The black background for jumpscares.")]
    public GameObject jumpscareBackground;

    [Header("Scale Settings")]
    public Vector3 startScale = new Vector3(1f, 1f, 1f);
    public Vector3 endScale = new Vector3(1.5f, 1.5f, 1.5f);

    private bool isScaring = false;
    private bool ready = false;  // Prevents accidental scare on scene load

    void Start()
    {
        // Add listener for the toggle click
        jumpscareToggle.onValueChanged.AddListener(OnToggleClicked);

        // Enable triggering starting next frame (prevents scene-load activation)
        StartCoroutine(EnableScareNextFrame());
    }

    private IEnumerator EnableScareNextFrame()
    {
        yield return null;
        ready = true;
    }

    void OnToggleClicked(bool value)
    {
        if (!ready) return;     // Ignore the first automatic event
        TriggerJumpscare();
    }

    public void TriggerJumpscare()
    {
        if (isScaring) return;
        StartCoroutine(JumpscareRoutine());
    }

    private IEnumerator JumpscareRoutine()
    {
        isScaring = true;

        // Turn on background
        if (jumpscareBackground != null)
        {
            jumpscareBackground.SetActive(true);
        } 

        // Choose image from jumpscare images
        GameObject randomImage = scareImages[Random.Range(0, scareImages.Length)];

        // Show the chosen image
        randomImage.SetActive(true);

        // Play spooky sound
        if (scareSound != null)
        {
            scareSound.Play();
        } 

        // Animate scale over scareDuration
        float time = 0f;
        while (time < scareDuration)
        {
            time += Time.deltaTime;
            float lerp = time / scareDuration;

            // Smooths scale increase
            lerp = lerp * lerp * (3f - 2f * lerp);

            randomImage.transform.localScale = Vector3.Lerp(startScale, endScale, lerp);

            yield return null;
        }

        // Hide image and background
        randomImage.SetActive(false);
        if (jumpscareBackground != null) 
        {
            jumpscareBackground.SetActive(false);
        }
        
        isScaring = false;
    }
}
