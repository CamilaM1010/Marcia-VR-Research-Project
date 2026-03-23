using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
///     The abstract base class for the AnimationStimulus and AudioStimulus. Provides logging functionality, debugging, UI Integration, and a triggering interface.
/// </summary>
[System.Serializable]
public abstract class StimulusBase : MonoBehaviour
{
    // Logging constants
    protected readonly static string STIMULUS_ACTION_TRIGGERED_TEXT = "STIMULUS_TRIGGERED_INPUTACTION";
    protected readonly static string STIMULUS_START_TEXT = "STIMULUS_START";

    [Header("UI Integration")]
    [Tooltip("The UI Button that displays the information about this Stimulus.")]
    [SerializeField] private Button button;

    [Tooltip("The GameObject tag corresponding to the TextMeshPro component containing the title of the Stimulus.")]
    [SerializeField] private string titleTag = "StimulusTitle";
    
    [Tooltip("The GameObject tag corresponding to the TextMeshPro component containing the Input Action used to trigger this stimulus, if one is present.")]
    [SerializeField] private string inputTextTag = "StimulusText";
    // The stimulus action trigger used to get the string name of the action.
    private StimulusActionTrigger stimulusActionTrigger;
    // The TMP Text component containing the title on its button.
    private TMP_Text tmpTitleText;
    // The TMP Text component containing the action to use to trigger the Stimulus.
    private TMP_Text tmpActionText;
    // The TMP Text component containing the "Use Action" text.
    private TMP_Text useActionText;
    // The default action string used to show how to trigger the Stimulus.
    private string defaultActionString = "";

    // Used to track whether the stimulus is currently in progress.
    protected bool isStimulusPlaying = false;
    
    // Track if we're currently showing trigger completion progress.
    private bool isShowingProgress = false;
    private Coroutine progressCoroutine = null;

    [Header("Logging")]
    [SerializeField] protected bool logActivity = true;

    [Header("Debug")]
    [SerializeField] private bool printDebugStatements = false;



    void Awake()
    {
        SetupButton();
    }

    // To be implemented by subclasses. 
    public virtual void TriggerStimulus(string triggerSource = "Manual/Button")
    {
        // Start progress display if button text exists.
        if (tmpActionText && !isShowingProgress)
        {
            if (progressCoroutine != null) StopCoroutine(progressCoroutine);
            progressCoroutine = StartCoroutine(UpdateProgressText());
        }
    }
    public virtual float GetStimulusDuration() { return 0f; }
    public virtual void StopStimulus()
    {
        CancelInvoke(nameof(ResetPlayingState));
        ResetPlayingState();
        ResetButton();
    }

    protected void LogDebug(string s) { if (printDebugStatements) Debug.Log(s); }

    // InputAction-based callback for triggering this Stimulus.
    public void OnTriggerStimulus(InputAction.CallbackContext context)
    {
        string triggerSource = $"InputAction: {context.action.name}";
        if (logActivity) StimulusLogger.Log(STIMULUS_ACTION_TRIGGERED_TEXT, gameObject.name, triggerSource);

        LogDebug($"Stimulus triggered for {gameObject.name} by InputAction {context.action.name}");
        TriggerStimulus(triggerSource);
    }

    // Update the text displaying the completion progress on the UI button, if it exists.
    private IEnumerator UpdateProgressText()
    {
        if (!tmpActionText) yield break;

        ShowProgress();
        float duration = GetStimulusDuration();
        float elapsed = 0f;
        int lastDisplayedPercentage = -1;

        useActionText.text = "Progress:";

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            int percentage = Mathf.Clamp(Mathf.FloorToInt((elapsed / duration) * 100f), 0, 100);

            if (percentage != lastDisplayedPercentage)
            {
                tmpActionText.text = $"{percentage:F0}%";
                lastDisplayedPercentage = percentage;                
            }
            yield return null;
        }

        // Restore original button state
        tmpActionText.text = "100%";
        ResetButton();
    }

    protected void ShowProgress()
    {
        isShowingProgress = true;
    }

    protected void ResetPlayingState()
    {
        isShowingProgress = false;
        isStimulusPlaying = false;
    }

    private void ResetButton()
    {
        if (!button) return;
        useActionText.text = "Use Action";
        tmpActionText.text = defaultActionString;
        ResetPlayingState();
    }

    // If this component is attached to a UI button, assign the button's text values.
    private void SetupButton()
    {
        // Get the TextMeshPro component references for assigning their values.
        if (!button) return;
        button.onClick.AddListener(() => TriggerStimulus($"Button: {button.gameObject.name}"));
        stimulusActionTrigger = GetComponent<StimulusActionTrigger>();

        TMP_Text[] texts = button.GetComponentsInChildren<TMP_Text>();
        foreach (var t in texts)
        {
            if (t.CompareTag(titleTag)) tmpTitleText = t;
            else if (t.CompareTag(inputTextTag)) tmpActionText = t;
            else useActionText = t;
        }

        // Assign the title text and the text for the key to be pressed.
        if (!tmpTitleText) 
            Debug.LogWarning($"Button has been assigned to Stimulus on {name}, but it contains no TextMeshPro Text components in children tagged with {titleTag}. Please ensure {button.name} has a child GameObject tagged with {titleTag} and containing a TextMeshPro Text component.");
        else 
            tmpTitleText.text = gameObject.name;

        if (!tmpActionText) 
            Debug.LogWarning($"Button has been assigned to Stimulus on {name}, but it contains no TextMeshPro Text components in children tagged with {inputTextTag}. Please ensure {button.name} has a child GameObject tagged with {inputTextTag} and containing a TextMeshPro Text component.");
        else
        {
            if (stimulusActionTrigger)
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
}