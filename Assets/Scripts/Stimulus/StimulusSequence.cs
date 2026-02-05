using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class StimulusSequence : MonoBehaviour
{
    // Stores the name of the Stimulus trigger function so it is not used directly.
    private const string stimulusTriggerFunctionName = "TriggerStimulus";

    [System.Serializable]
    public class Step
    {
        [Tooltip("The Stimulus to be triggered in this step.")]
        public Stimulus stimulus = null;

        [Tooltip("The delay time in seconds before triggering the stimulus in this step.")]
        [Range(0f, 120f)] public float preTriggerDelay = 0f;

        [Tooltip("The delay time in seconds after triggering the stimulus in this step.")]
        [Range(0f, 120f)] public float postTriggerDelay = 0f;
    }

    [Tooltip("Add steps here to be triggered in sequence.")]
    [SerializeField] public List<Step> stimuli;

    void Awake() { return; }

    // Called when this sequence is triggered by an input action from a Stimulus Action Trigger.
    public void OnTriggerStimulusSequence(InputAction.CallbackContext context)
    {
        Debug.Log("Stimulus Sequence triggered for " + gameObject.name + " by Input Action " + context.action.name);
        TriggerStimulusSequence();
    }

    // Called to trigger this sequence of stimuli.
    public void TriggerStimulusSequence()
    {
        // Trigger the step's stimulus after its pre-trigger delay and proceed to the next step after the current's post-trigger delay.
        foreach (var step in stimuli) 
        {
            step.stimulus.Invoke(stimulusTriggerFunctionName, step.preTriggerDelay);
            
            Invoke(nameof(HoldUp), step.postTriggerDelay);
        }
    }

    private void HoldUp()
    {
        return;
    }
}
