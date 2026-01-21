using UnityEngine;
using System.Collections;

public class SoundAnomaly : Anomaly
{
    [Header("Sound Settings")]
    public AudioClip[] anomalyClips; // list of possible sounds
    public float minDelay = 2f;
    public float maxDelay = 6f;

    [Header("Sound Locations")]
    public Transform[] soundPoints; // your wall point objects
    public AnomalyCaption captions;

    private bool isActive = false;
    private Coroutine soundRoutine;

    public override void Activate()
    {
        Debug.Log("Sound Anomaly Activated");

        if (!isActive)
        {
            isActive = true;
            soundRoutine = StartCoroutine(PlayRandomSound());
        }
    }

    public override void Deactivate()
    {
        Debug.Log("Sound Anomaly Deactivated");
        isActive = false;

        if (soundRoutine != null)
            StopCoroutine(soundRoutine);
    }

    IEnumerator PlayRandomSound()
{
    while (isActive)
    {
        // Random wait
        yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));

        if (soundPoints.Length == 0 || anomalyClips.Length == 0)
            continue;

        // Pick a random location
        Transform point = soundPoints[Random.Range(0, soundPoints.Length)];

        // Pick a random sound clip
        AudioClip clip = anomalyClips[Random.Range(0, anomalyClips.Length)];

        // Get saved volume from PlayerPrefs
        int closedCaptions = PlayerPrefs.GetInt("CC_KEY", 0);

        // Play sound at that position with saved volume
        AudioSource.PlayClipAtPoint(clip, point.position, 0.5f);
        // if (captions != null && closedCaptions == 1)
        // {
        //     if (!captions.gameObject.activeSelf)
        //     {
        //         captions.gameObject.SetActive(true);
        //     }
        //     captions.ShowCaption("scratching", point);
        // }
    }
}

}
