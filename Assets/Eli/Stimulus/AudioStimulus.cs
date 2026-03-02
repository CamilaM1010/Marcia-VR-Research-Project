using UnityEngine;

/// <summary>
/// When triggered, plays a one-shot audio clip from an audio source. Inherits from StimulusBase.cs
/// </summary>
[System.Serializable]
public class AudioStimulus : StimulusBase
{
    // Logging constants
    private readonly static string STIMULUS_AUDIO_START_TEXT = "AUDIO_START";
    private readonly static string STIMULUS_AUDIO_STOP_TEXT = "AUDIO_STOP";

    [Header("Audio")]
    [SerializeField, Tooltip("The AudioSource used to play an AudioClip when the stimulus is triggered.")]
    private AudioSource audioSource = null;

    [SerializeField, Tooltip("The sound played when the stimulus is triggered.")]
    private AudioClip audioClip = null;



    void Start()
    {
        if (!audioSource || !audioClip)
        {
            Debug.LogWarning($"Audio Source {(audioSource ? "" : "not ")}assigned, Audio Clip {(audioClip ? "" : "not ")}assigned to AudioStimulus on {gameObject.name}. Sound will not be played unless an Audio Source and an Audio Clip are assigned in the inspector.");
            enabled = false;
            return;
        }
    }
    
    // If not already playing, play a one shot of the audio clip through the specified audio source and update the UI button as it plays.
    public override void TriggerStimulus(string triggerSource = "Manual/Button")
    {
        if (isStimulusPlaying) return;

        // Log the trigger event
        if (logActivity) StimulusLogger.Log(STIMULUS_START_TEXT, gameObject.name, triggerSource);
        isStimulusPlaying = true;
        Invoke(nameof(ResetPlayingState), GetStimulusDuration());

        // If the AudioSource and Sound are specified, play the sound. Otherwise, log their state to the console and don't play the sound.
        if (audioSource && audioClip)
        {
            audioSource.PlayOneShot(audioClip);

            // Log audio start to StimulusLogger
            if (logActivity)
                StimulusLogger.Log(
                    STIMULUS_AUDIO_START_TEXT,
                    gameObject.name,
                    triggerSource,
                    $"Clip: {audioClip.name}, Duration: {audioClip.length}s"
                );

            LogDebug($"Audio triggered successfully for {gameObject.name} Audio Stimulus");
        }

        base.TriggerStimulus(triggerSource);
    }

    // Returns the length of this AudioStimulus' audio clip, if it exists.
    public override float GetStimulusDuration()
    {
        return (audioSource && audioClip) ? audioClip.length : base.GetStimulusDuration();
    }

    // Stop this audio clip if playing.
    public override void StopStimulus()
    {
        if (!audioSource || !audioSource.isPlaying) return;

        StopAllCoroutines();
        base.StopStimulus();
        audioSource.Stop();

        // Log audio stop
        if (logActivity) StimulusLogger.Log(STIMULUS_AUDIO_STOP_TEXT, gameObject.name, "Manual Stop");
    }
}
