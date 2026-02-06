using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class StimulusSequence : MonoBehaviour
{
    [System.Serializable]
    public class Step
    {
        [Tooltip("The Stimulus to be triggered in this step.")]
        public Stimulus stimulus = null;

        [Tooltip("The delay time in seconds after triggering the stimulus in this step.")]
        [Range(0f, 120f)] public float delayAfter = 0f;
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
    // The TMP Text component containing the action to use to trigger the Stimulus Sequence.
    private TMP_Text tmpActionText;
    // The TMP Text component containing the "Use Action" text.
    private TMP_Text useActionText;
    // The default action string used to show how to trigger the Stimulus Sequence.
    private string defaultActionString = "";
    
    // Track if we're currently showing sequence completion progress
    private bool isShowingProgress = false;
    private Coroutine progressCoroutine = null;

    [Header("Debug")]
    [SerializeField] private bool printDebugStatements = false;

    private void LogDebug(string s)
    {
        if (printDebugStatements) Debug.Log(s);
    }

    void Start()
    {
        SetupButton();
    }

    private void SetupButton()
    {
        // Get the TextMeshPro component references for assigning their values.
        if (button == null) return;
        button.onClick.AddListener(TriggerStimulusSequence);
        stimulusActionTrigger = GetComponent<StimulusActionTrigger>();

        TMP_Text[] texts = button.GetComponentsInChildren<TMP_Text>();
        foreach (var t in texts)
        {
            if (t.CompareTag(titleTag)) tmpTitleText = t;
            else if (t.CompareTag(inputTextTag)) tmpActionText = t;
            else useActionText = t;
        }

        // Assign the title text and the text for the key to be pressed.
        if (tmpTitleText == null) Debug.LogWarning($"Button has been assigned to Stimulus Sequence on {name}, but it contains no TextMeshPro Text components in children tagged with {titleTag}. Please ensure {button.name} has a child GameObject tagged with {titleTag} and containing a TextMeshPro Text component.");
        else tmpTitleText.text = $"{gameObject.name} (Seq.)";

        if (tmpActionText == null) 
            Debug.LogWarning($"Button has been assigned to Stimulus Sequence on {name}, but it contains no TextMeshPro Text components in children tagged with {inputTextTag}. Please ensure {button.name} has a child GameObject tagged with {inputTextTag} and containing a TextMeshPro Text component.");
        else
        {
            if (stimulusActionTrigger != null)
            {
                defaultActionString = $"Click Button OR \"{stimulusActionTrigger.toggleAction.name}\"";
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
        LogDebug($"Stimulus Sequence triggered for {gameObject.name} by Input Action {context.action.name}");
        TriggerStimulusSequence();
    }

    // Called to trigger this sequence of stimuli.
    public void TriggerStimulusSequence()
    {
        StartCoroutine(ExecuteSequence());

        // Start progress display
        if (tmpActionText != null && !isShowingProgress)
        {
            if (progressCoroutine != null)
                StopCoroutine(progressCoroutine);
            progressCoroutine = StartCoroutine(UpdateSequenceProgressText());
        }
    }

    // Execute each trigger sequentially with the proper delays.
    private IEnumerator ExecuteSequence()
    {
        foreach (Step step in stimuli)
        {   
            float stimulusDuration = step.stimulus.GetStimulusDuration();
            
            // Wait for stimulus to complete.
            step.stimulus.TriggerStimulus();
            yield return new WaitForSeconds(stimulusDuration);

            // Optional delay before next stimulus
            if (step.delayAfter > 0)
                yield return new WaitForSeconds(step.delayAfter);
        }
    }

    // Update the text on the UI Button, if it exists, to show the progress of this sequence's completion.
    private IEnumerator UpdateSequenceProgressText()
    {
        if (tmpActionText == null) yield break;

        useActionText.text = "Progress:";
        isShowingProgress = true;
        int totalSteps = stimuli.Count;
        int currentStep = 0;

        // Calculate total sequence duration for overall percentage.
        float totalSequenceDuration = 0f;
        foreach (Step step in stimuli)
            totalSequenceDuration += step.stimulus.GetStimulusDuration() + step.delayAfter;

        float overallElapsed = 0f;

        foreach (Step step in stimuli)
        {
            currentStep++;

            // Calculate this step's total duration.
            float stepDuration = step.stimulus.GetStimulusDuration() + step.delayAfter;

            float stepElapsed = 0f;

            while (stepElapsed < stepDuration)
            {
                stepElapsed += Time.deltaTime;
                overallElapsed += Time.deltaTime;

                float stepPercentage = Mathf.Clamp01(stepElapsed / stepDuration) * 100f;
                float overallPercentage = Mathf.Clamp01(overallElapsed / totalSequenceDuration) * 100f;

                tmpActionText.text = $"Step {currentStep}/{totalSteps} - {stepPercentage:F0}% | Total: {overallPercentage:F0}%";
                yield return null;
            }
        }
        
        // Reset original button state.
        ResetButton();
    }

    private void ResetButton()
    {
        useActionText.text = "Use Action";
        tmpActionText.text = defaultActionString;
        isShowingProgress = false;
    }

    public void StopSequence()
    {
        StopAllCoroutines();
        ResetButton();
    }
}
