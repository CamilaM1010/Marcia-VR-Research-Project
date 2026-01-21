using UnityEngine;
using System.Collections;

public class SmokeDetector : Anomaly
{
    public AudioSource audioSource;
    public AnomalyCaption captions;

    [Header("Beep Timing")]
    public float baseInterval = 1.0f;      // how often we check for a beep

    [Header("Random Chance")]
    public int startingRandomRange = 8;    // start 1-in-8
    public int endingRandomRange = 1;      // end 1-in-1 (always beep)
    public float rampDuration = 10f;       // time it takes to tighten randomness

    [Header("Volume Ramp")]
    public float startVolume = 0.35f;
    public float endVolume = 0.75f;

    private Coroutine beepRoutine;

    void Beep(int currentRange)
    {
        if (Random.Range(0, currentRange) == 0)
        {
            Debug.Log("Beep (vol " + audioSource.volume + ")");
            audioSource.Play();
            int closedCaptions = PlayerPrefs.GetInt("CC_KEY", 0);
            if (captions != null && closedCaptions == 1)
            {
                if (!captions.gameObject.activeSelf)
                {
                    captions.gameObject.SetActive(true);
                }
                captions.ShowCaption("beep", this.transform);
            }
        }
        else
        {
            Debug.Log("Silence");
        }
    }

    IEnumerator BeepLoop()
    {
        float elapsed = 0f;

        while (true)
        {
            // Lerp the random range from large → small
            float t = Mathf.Clamp01(elapsed / rampDuration);

            float lerped = Mathf.Lerp(startingRandomRange, endingRandomRange, t);
            int currentRange = Mathf.Max(1, Mathf.RoundToInt(lerped)); // never < 1

            // Lerp volume from quiet to loud
            audioSource.volume = Mathf.Lerp(startVolume, endVolume, t);

            // Attempt a beep using current randomness
            Beep(currentRange);

            // Wait for the fixed interval
            yield return new WaitForSeconds(baseInterval);
            elapsed += baseInterval;
        }
    }

    public override void Activate()
    {
        if (beepRoutine != null)
            StopCoroutine(beepRoutine);

        audioSource.volume = startVolume;
        beepRoutine = StartCoroutine(BeepLoop());
    }

    public override void Deactivate()
    {
        if (beepRoutine != null)
            StopCoroutine(beepRoutine);

        beepRoutine = null;
    }
}
