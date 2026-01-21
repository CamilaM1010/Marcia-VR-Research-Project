using UnityEngine;

public class CatSpin : MonoBehaviour
{
    [Header("Spin Settings")]
    public float startTime = 0f;      // When the cat appears and starts spinning
    public float spinSpeed = 720f;    // Degrees per second

    private bool active = false;      // Has this cat started spinning yet?

    void Start()
    {
        // Start invisible
        gameObject.SetActive(false);
    }

    // Call this from Car_anomaly to trigger the cat based on music/timeline
    public void Trigger(float currentTime)
    {
        if (!active && currentTime >= startTime)
        {
            active = true;
            gameObject.SetActive(true);
        }
    }

    void Update()
    {
        if (!active) return;

        // Spin the cat around Y axis
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f);
    }

    // Call this when the music ends
    public void Deactivate()
    {
        active = false;
        gameObject.SetActive(false);
    }
}
