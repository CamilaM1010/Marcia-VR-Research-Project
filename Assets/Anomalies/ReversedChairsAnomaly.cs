using UnityEngine;
using System.Collections.Generic;

public class ReversedChairsAnomaly : Anomaly
{
    public int chairsToAffect = 9;
    [Range(0f, 1f)]
    public float activeRotationChance = 0.3f;

    public Vector2 activeRotationSpeedRange = new Vector2(-40f, 40f);

    private List<GameObject> selectedChairs = new List<GameObject>();
    private Dictionary<GameObject, float> staticRotationAmounts = new Dictionary<GameObject, float>();
    private List<GameObject> activeRotatingChairs = new List<GameObject>();
    private Dictionary<GameObject, float> activeRotationSpeeds = new Dictionary<GameObject, float>();

    private bool isActive = false;

    void Update()
    {
        if (!isActive) return;

        foreach (GameObject chair in activeRotatingChairs)
        {
            if (chair != null)
            {
                chair.transform.Rotate(
                    0f,
                    activeRotationSpeeds[chair] * Time.deltaTime,
                    0f,
                    Space.World
                );
            }
        }
    }

    public override void Activate()
    {
        base.Activate();
        Debug.Log("Chairs Anomaly Activated");

        GameObject[] chairs = GameObject.FindGameObjectsWithTag("Chair");
        if (chairs.Length < chairsToAffect)
        {
            Debug.LogWarning("Not enough chairs in the scene to rotate!");
            return;
        }

        selectedChairs.Clear();
        staticRotationAmounts.Clear();
        activeRotatingChairs.Clear();
        activeRotationSpeeds.Clear();

        // Shuffle
        List<GameObject> shuffled = new List<GameObject>(chairs);
        for (int i = 0; i < shuffled.Count; i++)
        {
            GameObject tmp = shuffled[i];
            int rand = Random.Range(i, shuffled.Count);
            shuffled[i] = shuffled[rand];
            shuffled[rand] = tmp;
        }

        // Select N chairs
        for (int i = 0; i < chairsToAffect; i++)
        {
            GameObject chair = shuffled[i];
            selectedChairs.Add(chair);

            bool makeActive = Random.value < activeRotationChance;

            if (makeActive)
            {
                activeRotatingChairs.Add(chair);

                float randomSpeed = Random.Range(
                    activeRotationSpeedRange.x,
                    activeRotationSpeedRange.y
                );
                activeRotationSpeeds.Add(chair, randomSpeed);
            }
            else
            {
                float randomRotation = Random.Range(30f, 360f);
                chair.transform.Rotate(0f, randomRotation, 0f, Space.World);
                staticRotationAmounts.Add(chair, randomRotation);
            }
        }

        isActive = true;
    }

    public override void Deactivate()
    {
        if (!isActive) return;

        Debug.Log("Chairs Anomaly Deactivated");

        foreach (var entry in staticRotationAmounts)
        {
            if (entry.Key != null)
                entry.Key.transform.Rotate(0f, -entry.Value, 0f, Space.World);
        }

        foreach (GameObject chair in activeRotatingChairs)
        {
            if (chair != null)
            {
                Vector3 euler = chair.transform.eulerAngles;
                euler.y = Mathf.Round(euler.y / 360f) * 360f;
                chair.transform.eulerAngles = euler;
            }
        }

        selectedChairs.Clear();
        staticRotationAmounts.Clear();
        activeRotatingChairs.Clear();
        activeRotationSpeeds.Clear();

        isActive = false;

        base.Deactivate();
    }
}
