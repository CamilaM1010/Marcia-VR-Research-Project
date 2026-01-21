using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImageAnomaly : Anomaly
{
    [Header("Image Settings")]
    public GameObject[] imagePrefabs;
    public float minDelay = 2f;
    public float maxDelay = 5f;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    [Header("Audio Settings")]
    public AudioClip[] spawnSounds;

    [Header("References")]
    public Transform player;

    private Coroutine routine;
    private bool isActive = false;

    private List<Transform> unusedPoints;
    private List<GameObject> spawnedObjects = new List<GameObject>();

    // ---------------------------------------------------

    public override void Activate()
    {
        if (isActive) return;

        isActive = true;

        // Copy spawn points into a working list
        unusedPoints = new List<Transform>(spawnPoints);

        routine = StartCoroutine(SpawnLoop());
    }

    public override void Deactivate()
    {
        isActive = false;

        if (routine != null)
            StopCoroutine(routine);

        // Destroy all spawned images
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
                Destroy(obj); // or obj.SetActive(false) to hide
        }

        spawnedObjects.Clear();
    }

    // ---------------------------------------------------

    IEnumerator SpawnLoop()
    {
        while (isActive)
        {
            if (unusedPoints.Count == 0)
            {
                isActive = false;
                yield break;
            }

            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));

            int index = Random.Range(0, unusedPoints.Count);
            Transform point = unusedPoints[index];
            unusedPoints.RemoveAt(index);

            GameObject prefab = imagePrefabs[Random.Range(0, imagePrefabs.Length)];

            // Spawn
            GameObject spawned = Instantiate(prefab, point.position, Quaternion.identity);

            // Track it for cleanup
            spawnedObjects.Add(spawned);

            // Always face player
            StartCoroutine(FacePlayerContinuously(spawned.transform));

            // Play sound
            PlaySpawnSound(point.position);
        }
    }

    // ---------------------------------------------------

    private IEnumerator FacePlayerContinuously(Transform obj)
    {
        while (obj != null)
        {
            if (player != null)
            {
                Vector3 targetPos = player.position;
                obj.LookAt(targetPos);
            }
            yield return null;
        }
    }

    private void PlaySpawnSound(Vector3 pos)
    {
        if (spawnSounds == null || spawnSounds.Length == 0)
            return;

        AudioClip clip = spawnSounds[Random.Range(0, spawnSounds.Length)];
        AudioSource.PlayClipAtPoint(clip, pos, 1.0f);
    }
}
