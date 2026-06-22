using UnityEngine;

public class DualDoorCloser : MonoBehaviour
{
    // ---------- Left door ----------
    public Animator leftDoorAnimator;   // must have a "CloseDoor" trigger
    public AudioSource leftCloseSource; // already holds the closing clip

    // ---------- Right door ----------
    public Animator rightDoorAnimator;  // must have a "CloseDoor" trigger
    public AudioSource rightCloseSource; // already holds the closing clip

    // ---------- Shared outside noise ----------
    public AudioSource outsideAmbient;  // plays background noise
    [Range(0f,1f)] public float closedVolume = 0.3f; // volume while doors are closed
    float originalAmbientVolume;

    // ---------- State ----------
    bool doorsClosed = false;

    void Awake()
    {
        if (outsideAmbient != null)
            originalAmbientVolume = outsideAmbient.volume;
    }

    // Call this to toggle both doors together
    public void ToggleDoors()
    {
        if (!doorsClosed)
            CloseAll();
    }

    void CloseAll()
    {
        // Left door
        leftDoorAnimator?.SetTrigger("CloseDoor");
        leftCloseSource?.Play();

        // Right door
        rightDoorAnimator?.SetTrigger("CloseDoor");
        rightCloseSource?.Play();

        // Damp out ambient
        if (outsideAmbient != null)
            outsideAmbient.volume = closedVolume;

        doorsClosed = true;
    }

}