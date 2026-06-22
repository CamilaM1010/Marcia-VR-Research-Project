using UnityEngine;

public class DoorCloseTrigger : MonoBehaviour
{
    [Header("Doors to control")]
    public GameObject doorA;          // the GameObject holding the Animator
    public GameObject doorB;          // the second door

    [Header("Bool parameter that opens the door")]
    public string boolParameterName = "Close";   // rename if your Animator uses a different name

    // ------------------------------------------------------------------------

    /// <summary>
    /// Hook this into the button’s OnClick().  It turns the bool on
    /// and tells each door to play its closing audio.
    /// </summary>
    public void Trigger()
    {
        SetBoolOnAnimator(doorA, true);
        SetBoolOnAnimator(doorB, true);

        PlayDoorAudio(doorA);
        PlayDoorAudio(doorB);
    }

    // ------------------------------------------------------------------------

    private void SetBoolOnAnimator(GameObject door, bool value)
    {
        if (door == null) return;

        Animator anim = door.GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogWarning($"No Animator found on {door.name}");
            return;
        }

        anim.SetBool(boolParameterName, value);
    }

    private void PlayDoorAudio(GameObject door)
    {
        if (door == null) return;

        AudioSource audio = door.GetComponent<AudioSource>();
        if (audio != null)
        {
            // `Play()` restarts the clip; use PlayOneShot() if you want to layer sounds
            audio.Play();
        }
        else
        {
            Debug.LogWarning($"No AudioSource found on {door.name}");
        }
    }
}