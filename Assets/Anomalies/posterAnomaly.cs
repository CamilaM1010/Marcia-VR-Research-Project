using UnityEngine;

public class PosterAnomaly : Anomaly
{
    public GameObject catPoster;      // Reference to your cat sprite object

    [Header("Scale Growth Settings")]
    public float growthSpeed = 0.2f;  // How fast the poster grows (units per second)
    public float maxScale = 3f;       // Maximum allowed scale

    private bool isActive = false;
    private Vector3 startingScale;

    void Awake()
    {
        if (catPoster != null)
        {
            startingScale = catPoster.transform.localScale;
            Debug.Log("Starting scale " + startingScale);
            catPoster.SetActive(false);
        }
    }

    void Update()
    {
        if (isActive && catPoster != null)
        {
            // Increase scale gradually
            Vector3 newScale = catPoster.transform.localScale +
                               startingScale * growthSpeed * Time.deltaTime;

            // Clamp so it doesn't grow infinitely (remove if you WANT it infinite)
            newScale = Vector3.Min(newScale, startingScale * maxScale);

            catPoster.transform.localScale = newScale;
        }
    }

    public override void Activate()
    {
        Debug.Log("Poster Anomaly Activated");
        isActive = true;

        if (catPoster != null)
        {
            catPoster.SetActive(true);

        }
    }

    public override void Deactivate()
    {
        Debug.Log("Poster Anomaly Deactivated");
        isActive = false;

        if (catPoster != null)
        {
            catPoster.SetActive(false);
            catPoster.transform.localScale = startingScale;
        }
    }
}
