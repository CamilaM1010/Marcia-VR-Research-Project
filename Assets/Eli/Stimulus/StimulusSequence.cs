using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;
using UnityEngine.UI;

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

    [Header("Sequencing")]
    [Tooltip("Add steps here to be triggered in sequence.")]
    [SerializeField] public List<Step> stimuli;

    [Header("UI Integration")]
    [Tooltip("The UI Button that displays the information about this Stimulus Sequence.")]
    [SerializeField] private Button button;

    [Tooltip("The GameObject tag corresponding to the TextMeshPro component containing the title of the Stimulus Sequence.")]
    [SerializeField] private string titleTag = "StimulusTitle";
    
    [Tooltip("The GameObject tag corresponding to the TextMeshPro component containing the Input Action used to trigger this Stimulus Sequence, if one is present.")]
    [SerializeField] private string inputTextTag = "StimulusText";
    // The stimulus action trigger used to get the string name of the action.
    private StimulusActionTrigger stimulusActionTrigger;
    // The TMP Text component containing the title on its button.
    private TMP_Text tmpTitleText;
    // The TMP Text component containing the action to use to trigger the Stimulus.
    private TMP_Text tmpActionText;
    // The default action string used to show how to trigger the Stimulus.
    private string defaultActionString = "";

    void Start()
    {
         // Get the TextMeshPro component references for assigning their values.
        if (button == null) return;
        button.onClick.AddListener(this.TriggerStimulusSequence);
        stimulusActionTrigger = GetComponent<StimulusActionTrigger>();

        TMP_Text[] texts = button.GetComponentsInChildren<TMP_Text>();
        foreach (var t in texts)
        {
            if (t.CompareTag(titleTag)) tmpTitleText = t;
            else if (t.CompareTag(inputTextTag)) tmpActionText = t;
        }

        // Assign the title text and the text for the key to be pressed.
        if (tmpTitleText == null) Debug.LogWarning("Button has been assigned to Stimulus Sequence on " + name + ", but it contains no TextMeshPro Text components in children tagged with " + titleTag + ". Please ensure " + button.name + " has a child GameObject tagged with " + titleTag + " and containing a TextMeshPro Text component.");
        else tmpTitleText.text = gameObject.name + "(Seq.)";

        if (tmpActionText == null) 
            Debug.LogWarning("Button has been assigned to Stimulus Sequence on " + name + ", but it contains no TextMeshPro Text components in children tagged with " + inputTextTag + ". Please ensure " + button.name + " has a child GameObject tagged with " + inputTextTag + " and containing a TextMeshPro Text component.");
        else
        {
            if (stimulusActionTrigger != null)
            {
                defaultActionString = "Click Button OR \"" + stimulusActionTrigger.toggleAction.name + "\"";
                tmpActionText.text = defaultActionString;
            }
            else
            {
                defaultActionString = "Click Button";
                tmpActionText.text = defaultActionString;
            }
        }
    }

    // Called when this sequence is triggered by an input action from a Stimulus Action Trigger.
    public void OnTriggerStimulusSequence(InputAction.CallbackContext context)
    {
        Debug.Log("Stimulus Sequence triggered for " + gameObject.name + " by Input Action " + context.action.name);
        TriggerStimulusSequence();
    }

    // Called to trigger this sequence of stimuli.
    public void TriggerStimulusSequence()
    {
        StartCoroutine(ExecuteSequence());
    }

    // Execute each trigger sequentially with the proper delays.
    private IEnumerator ExecuteSequence()
    {
        foreach (Step step in stimuli)
        {
            yield return new WaitForSeconds(step.preTriggerDelay);
            step.stimulus.TriggerStimulus();
            yield return new WaitForSeconds(step.postTriggerDelay);
        }
    }
}
