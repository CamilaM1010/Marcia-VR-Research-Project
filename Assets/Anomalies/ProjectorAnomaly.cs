using UnityEngine;
using System.Collections;

public class ProjectorAnomaly : Anomaly
{
    [Header("Anomaly Targets")]
    // The GameObject that contains the light/mesh for the projector. 
    // This is the object that will be enabled/disabled.
    public GameObject projectorVisuals; 
    public GameObject whiteboardLeft;
    public GameObject whiteboardRight;
    
    [Header("Flicker Timing")]
    public float checkInterval = 0.1f;     // How often the anomaly checks if it should flicker.
    public float flickerDuration = 0.05f;  // How long the light stays ON during a flicker.

    [Header("Random Chance")]
    public int startingRandomRange = 12;   // Start 1-in-12 chance of flickering ON.
    public int endingRandomRange = 1;      // End 1-in-1 (always flicker ON).
    public float rampDuration = 10f;       // Time it takes to tighten randomness (in seconds).

    private Coroutine flickerRoutine;

    // --- Coroutine Loop ---
    IEnumerator FlickerLoop()
    {
        float elapsed = 0f;
        
        // Ensure the visual component is OFF before entering the loop (The anomaly's starting state)
        projectorVisuals.SetActive(false); 

        while (true)
        {
            // 1. Calculate Ramp-Up (Escalating Intensity)
            float t = Mathf.Clamp01(elapsed / rampDuration);
            float lerped = Mathf.Lerp(startingRandomRange, endingRandomRange, t);
            int currentRange = Mathf.Max(1, Mathf.RoundToInt(lerped)); 
            
            // 2. Check for Flicker ON (Anomaly manifests)
            if (Random.Range(0, currentRange) == 0)
            {
                // Flicker ON: Brief manifestation
                projectorVisuals.SetActive(true);
                Debug.Log("Flicker ON");
                
                // Wait for the duration of the flicker (light is on)
                yield return new WaitForSeconds(flickerDuration);

                // Flicker OFF: Return to stable anomaly state
                projectorVisuals.SetActive(false);
                Debug.Log("Flicker OFF");
                
                // 3. Wait for the remainder of the check interval
                // This keeps the loop scheduled consistently.
                float remainingWait = checkInterval - flickerDuration;
                if (remainingWait > 0)
                {
                    yield return new WaitForSeconds(remainingWait);
                }
            }
            else
            {
                // Projector remains OFF for this interval (Stable anomaly state)
                Debug.Log("Projector OFF (Stable)");
                
                // 3. Wait for the full check interval
                yield return new WaitForSeconds(checkInterval);
            }
            
            elapsed += checkInterval;
        }
    }

    // --- Anomaly Interface Methods ---
    public override void Activate()
    {
        Debug.Log("Projector Activated (Anomaly ON)");

        // Set the room anomaly state
        whiteboardLeft.SetActive(false);
        whiteboardRight.SetActive(false);
        
        // Start the flickering loop
        flickerRoutine = StartCoroutine(FlickerLoop());
    }

    public override void Deactivate()
    {
        // Stop the flicker loop
        if (flickerRoutine != null) StopCoroutine(flickerRoutine);
        flickerRoutine = null;
        
        CancelInvoke();
        // Reset to normal scene state (Projector ON, Whiteboards ON)
        projectorVisuals.SetActive(false);
        whiteboardLeft.SetActive(true);
        whiteboardRight.SetActive(true);
        
        Debug.Log("Projector Deactivated (Anomaly OFF)");
    }
}