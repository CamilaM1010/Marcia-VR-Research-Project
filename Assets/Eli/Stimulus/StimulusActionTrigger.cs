using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;


public class StimulusActionTrigger : MonoBehaviour
{
    [Tooltip("The Unity Input Action used to trigger the stimulus.")]
    [SerializeField] public InputActionReference toggleAction;

    // If this is being used on a stimulus, the sequence flag is false. Otherwise, attempt to use a sequence. If a sequence is found, the sequence flag is true.
    private Stimulus stimulus;
    private bool useSeq = false;
    private StimulusSequence stimSeq;

    void Start()
    {
        if (toggleAction == null)
        {
            Debug.LogError("No Input Action Reference is assigned to the Stimulus Action Trigger on " + gameObject.name + ". Assign it in the inspector.");
            return;
        }

        // Get the Stimulus component reference.
        if (gameObject.TryGetComponent(out stimulus))
            Debug.Log("Stimulus component found. Stimulus Action Trigger will use Input Action " + toggleAction.action.name + " to trigger Stimulus " + stimulus.gameObject.name + ".");
        else if (gameObject.TryGetComponent(out stimSeq))
        {
            useSeq = true;
            Debug.Log("Stimulus Sequence component found. Stimulus Action Trigger will use Input Action " + toggleAction.action.name + " to trigger Stimulus Sequence " + stimulus.gameObject.name + ".");
        }
        else 
        {
            Debug.LogError("No Stimulus or Stimulus Sequence found.");
            return;
        }

        // Add the appropriate OnTriggerStimulus function to the Input Action's callback list.
        toggleAction.action.performed += useSeq ? stimSeq.OnTriggerStimulusSequence : stimulus.OnTriggerStimulus;
    }

    void OnDestroy()
    {
        if (toggleAction != null)
            toggleAction.action.performed -= useSeq ? stimSeq.OnTriggerStimulusSequence : stimulus.OnTriggerStimulus;
    }
}
