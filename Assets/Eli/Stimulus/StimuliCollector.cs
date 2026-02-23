using UnityEngine;

public class StimuliCollector : MonoBehaviour
{
    // Singleton instance - ensures only one collector exists in the scene.
    private static StimuliCollector _instance;
    [SerializeField] private Stimulus[] stimuli;
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
        stimuli = FindObjectsByType<Stimulus>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        sequences = FindObjectsByType<StimulusSequence>(FindObjectsInactive.Include, FindObjectsSortMode.None);        
    }

    private void StopSequences()
    {
        foreach (StimulusSequence s in sequences)
            s.StopSequence();
    }

    public void StopAllSound()
    {
        StopSequences();
        foreach (Stimulus s in stimuli)
            if (s.isActiveAndEnabled) s.StopSound();

        Debug.Log("All stimuli have ceased playing sound.");
    }

    public void StopAllAnimations()
    {
        StopSequences();
        foreach (Stimulus s in stimuli)
            s.StopAnimation();

        Debug.Log("All stimuli have ceased animation and been reset.");
    }

    public void StopAllStimuli()
    {
        StopAllSound();
        StopAllAnimations();
    }
}
