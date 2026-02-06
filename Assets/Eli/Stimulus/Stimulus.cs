using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class Stimulus : MonoBehaviour
{
    [Tooltip("The animator used to play the Stimulus' animation. The Stimulus assumes the animator will be in the idle, untriggered state when the scene is run.")]
    [SerializeField] private Animator animator;

    [Tooltip("The name of the animation parameter used to trigger the animation.")]
    [SerializeField] private string animationTriggerParameterName = "";


    [Tooltip("The audio source used to play an audio clip when the stimulus is triggered.")]
    [SerializeField] private AudioSource audioSource;

    [Tooltip("The sound played when the simulus is triggered.")]
    [SerializeField] private AudioClip stimulusSound;

    [Tooltip("If enabled, return this Stimulus' animator to its original state. Only disable this flag if the animator resets itself or the Stimulus' ResetTrigger() is called elsewhere.")]
    [SerializeField] private bool resetAfterTrigger = true;

    [Tooltip("If disabled, use a manual delay to determine when to reset the animator. NOTE: Reset After Trigger must be enabled to reset the animator.")]
    [SerializeField] private bool resetAfterAnimationEnds = true;

    [Tooltip("The wait time in seconds after triggering the stimulus before resetting. ")]
    [SerializeField] [Range(0.0f, 120.0f)] private float manualAnimationResetDelay = 0.0f;

    // The default state of the animator. Assumes the animator's default state is idle.
    private bool idleState;

    // Used to track the presence of the Animator, the Audio Source, and the Stimulus Sound.
    private bool hasAnimator;
    private bool hasAudioSource;
    private bool hasStimulusSound;

    void Start()
    {
        hasAnimator = animator != null;
        hasAudioSource = audioSource != null;
        hasStimulusSound = stimulusSound != null;

        if (!hasAnimator && !hasAudioSource) 
        {
            Debug.LogWarning("No animator or audio source attached to Stimulus on " + gameObject.name + ". Stimulus cannot be triggered until an animator or audio source and audio clip are assigned in the inspector.");
            enabled = false;
            return;
        }

        if (hasAnimator)
            idleState = animator.GetBool(animationTriggerParameterName);

        if (hasAudioSource && !hasStimulusSound)
            Debug.LogWarning("Audio Source assigned to Stimulus on " + gameObject.name + " without assigned Stimulus sound. Sound will not be played unless assigned in the inspector.");
    }

    // Input Action-based callback for triggering this Stimulus.
    public void OnTriggerStimulus(InputAction.CallbackContext context)
    {
        Debug.Log("Stimulus triggered for " + gameObject.name + " by Input Action " + context.action.name);
        TriggerStimulus();
    }

    // Trigger the Stimulus and set the animator's trigger parameter to triggered. Only call if the animation has been reset.
    public void TriggerStimulus()
    {
        if (hasAnimator)
        {
            bool state = animator.GetBool(animationTriggerParameterName);
            
            // Only trigger this Stimulus if it is idle. 
            if (state != idleState) 
                Debug.LogWarning("Attempted to trigger animation for Stimulus " + gameObject.name + ", use ResetStimulus() to reset its state.");

            // Trigger the Stimulus and log its state to the console.
            state = !state;
            animator.SetBool(animationTriggerParameterName, state);
            Debug.Log("Animation triggered successfully for " + gameObject.name + " Stimulus.");

            // Reset this stimulus.
            if (resetAfterTrigger) 
            {
                float delay = resetAfterAnimationEnds ? GetAnimationLength() : manualAnimationResetDelay;
                Invoke(nameof(this.ResetAnimation), delay);
            }
        }

        // If the AudioSource and Sound are specified, play the sound. Otherwise, log their state to the console and don't play the sound.
        if (hasAudioSource && hasStimulusSound)
        {
            audioSource.PlayOneShot(stimulusSound);
            Debug.Log("Audio triggered successfully for " + gameObject.name + " Stimulus.");
        }
        else
            Debug.LogWarning("Audio Source " + (hasAudioSource ? "not set" : "set") + ", Stimulus Sound " + (hasStimulusSound ? "not set" : "set") + ", not playing sound for " + gameObject.name + ".");      
    }

    // Preconditions: hasAnimator == true
    private float GetAnimationLength()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.length / stateInfo.speed;
    }

    // Reset the animator's parameter and the local flag.
    public void ResetAnimation()
    {
        if (!hasAnimator)
        {
            Debug.LogError("ResetAnimation called for Stimulus " + gameObject.name + " without an assigned Animator. Please assign it in the inspector.");
            return;
        }

        // If designated to do so, reset this Stimulus and its animator once the animator is no longer in transition and following the specified delay.
        if (!resetAfterTrigger) return;

        if (animator.IsInTransition(0))
        {
            Invoke(nameof(this.ResetAnimation), 0.05f);
            return;
        }

        bool state = animator.GetBool(animationTriggerParameterName);
        animator.SetBool(animationTriggerParameterName, !state);
        Debug.Log(gameObject.name + " Stimulus animation has been reset");
    }

    public void StopSound()
    {
        if (hasAudioSource && audioSource.isPlaying) audioSource.Stop();
    }

    public void StopAnimation()
    {
        if (!hasAnimator || !animator.IsInTransition(0)) return;
        
        bool state = animator.GetBool(animationTriggerParameterName);
        animator.SetBool(animationTriggerParameterName, !state);
    }
}
