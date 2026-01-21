using UnityEngine;
using System.Collections;

public class LectureAudioAnomaly : Anomaly
{
    public AudioSource audioSource;
    public AnomalyCaption captions;

    [Header("Pitch Oscillation Settings")]
    public float minPitch = 0.8f;         // lowest speed
    public float maxPitch = 1.3f;         // highest speed

    public float minOscillationSpeed = 0.5f;  // slow wobble
    public float maxOscillationSpeed = 2.5f;  // fast wobble

    public float minCycleDuration = 2f;       // how long a wobble pattern lasts
    public float maxCycleDuration = 6f;

    private Coroutine oscillationRoutine;

    IEnumerator PitchOscillationLoop()
    {
        while (true)
        {
            // Each cycle has its own parameters
            float low = Random.Range(minPitch, 1f);
            float high = Random.Range(1f, maxPitch);

            float speed = Random.Range(minOscillationSpeed, maxOscillationSpeed);
            float duration = Random.Range(minCycleDuration, maxCycleDuration);

            float elapsed = 0f;

            while (elapsed < duration)
            {
                // Use sine wave for smooth oscillation
                float sine = Mathf.Sin(Time.time * speed);
                float t = (sine + 1f) * 0.5f;         // convert -1..1 → 0..1

                // Interpolate between low → high
                audioSource.pitch = Mathf.Lerp(low, high, t);

                elapsed += Time.deltaTime;
                yield return null;
            }

            // After each cycle, reset pitch briefly so clips don't drift
            audioSource.pitch = 1f;
        }
    }

    public override void Activate()
    {
        int closedCaptions = PlayerPrefs.GetInt("CC_KEY", 0);
        if (captions != null && closedCaptions == 1)
        {
            if (!captions.gameObject.activeSelf)
            {
                captions.gameObject.SetActive(true);
            }
            captions.displayTime = 9999f;
            captions.ShowCaption("lecturing", this.transform);
        }

        if (!audioSource.isPlaying)
            audioSource.Play();

        if (oscillationRoutine != null)
            StopCoroutine(oscillationRoutine);

        oscillationRoutine = StartCoroutine(PitchOscillationLoop());
    }

    public override void Deactivate()
    {
        captions.displayTime = 2.5f;
        if(captions != null && captions.gameObject.activeSelf) 
        {
            captions.gameObject.SetActive(false);
        }
        if (oscillationRoutine != null)
            StopCoroutine(oscillationRoutine);

        audioSource.pitch = 1f;
        audioSource.Stop();
        oscillationRoutine = null;
    }
}
