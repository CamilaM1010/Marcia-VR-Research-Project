using UnityEngine;

public class StimuliCollector : MonoBehaviour
{
    // Singleton instance - ensures only one collector exists in the scene.
    private static StimuliCollector _instance;
    [SerializeField] private AnimationStimulus[] animationStimuli;
    [SerializeField] private AudioStimulus[] audioStimuli;
    [SerializeField] private StimulusSequence[] sequences;

    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            // If a collector already exists, destroy this duplicate
            Destroy(this.gameObject);
    }

    void Start()
    {
        animationStimuli = FindObjectsByType<AnimationStimulus>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        audioStimuli = FindObjectsByType<AudioStimulus>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        sequences = FindObjectsByType<StimulusSequence>(FindObjectsInactive.Include, FindObjectsSortMode.None);
    }

    private void StopSequences()
    {
        if (sequences == null) return;
        foreach (StimulusSequence s in sequences)
            s.StopSequence();
    }

    public void StopAllSound()
    {
        StopSequences();
        foreach (AudioStimulus s in audioStimuli)
            if (s.isActiveAndEnabled) s.StopStimulus();

        Debug.Log("All stimuli have ceased playing sound.");
    }

    public void StopAllAnimations()
    {
        StopSequences();
        foreach (AnimationStimulus s in animationStimuli)
            if (s.isActiveAndEnabled) s.StopStimulus();

        Debug.Log("All stimuli have ceased animation and been reset.");
    }

    public void StopAllStimuli()
    {
        StopSequences();
        foreach (AnimationStimulus s in animationStimuli)
            if (s.isActiveAndEnabled) s.StopStimulus();
        foreach (AudioStimulus s in audioStimuli)
            if (s.isActiveAndEnabled) s.StopStimulus();
    }
}
