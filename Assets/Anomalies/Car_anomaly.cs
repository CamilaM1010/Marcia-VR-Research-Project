using UnityEngine;

public class Car_anomaly : MonoBehaviour
{
    [Header("References")]
    public AudioSource carAudioSource;
    public ParticleSystem catParticles;
    public Transform player;
    public GameObject redLight;
    public CatSpin[] extraCats;

    [Header("Timing")]
    public float triggerTime = 2f;
    public float particleStart = 45.301f;
    public float particleDuration = 12.825f;

    public float normalSpinSpeed = 720f;
    public float slowSpinSpeed = 90f;

    private float sequenceTime = 0f;
    private bool sequenceRunning = false;
    private bool sequenceLocked = false;   // becomes true after triggerTime
    private bool playerInside = false;

    private Quaternion originalRotation;
    private Vector3 originalPosition;
    public GameObject gateObject;

    void Start()
    {
        originalRotation = transform.rotation;
        originalPosition = transform.position;
    }

    void Update()
    {
        // If sequence is running (activated or past trigger)
        if (sequenceRunning)
        {
            sequenceTime += Time.deltaTime;
            foreach (var c in extraCats)
            {
                c.Trigger(sequenceTime);
            }

            // Lock sequence once we cross trigger time
            if (sequenceTime >= triggerTime)
                sequenceLocked = true;

            UpdateParticles();
            UpdateCatSpin();
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (!other.CompareTag("Player"))
            return;

        playerInside = true;

        if (gateObject != null)
            gateObject.SetActive(true);
                    
        // If sequence already locked, re-entering does nothing
        if (sequenceLocked)
            return;

        StartSequence();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = false;

        if (sequenceLocked)
            return;

        if (sequenceTime < triggerTime)
            ResetSequence();
    }

    private void StartSequence()
    {
        Debug.Log("Start sequence called. sequenceRunning=" + sequenceRunning + " time=" + sequenceTime);

        sequenceRunning = true;
        sequenceTime = 0f;

        if (carAudioSource != null)
        {
            carAudioSource.Stop();
            carAudioSource.time = 0f;
            carAudioSource.Play();
        }

        if (catParticles != null)
            catParticles.Stop();
    }

    public void ResetSequence()
    {
        sequenceRunning = false;
        sequenceTime = 0f;
        sequenceLocked = false;
        playerInside = false;
        transform.rotation = originalRotation;

        if (carAudioSource != null)
        {
            carAudioSource.Stop();
            carAudioSource.time = 0f;
        }

        if (catParticles != null)
            catParticles.Stop();

        // Reset positon to ground
        transform.position = originalPosition;

        Debug.Log("RESET CALLED. sequenceRunning=" + sequenceRunning + " time=" + sequenceTime);
    }

    private void UpdateParticles()
    {
        if (catParticles == null)
            return;

        if (sequenceTime >= particleStart &&
            sequenceTime < particleStart + particleDuration)
        {
            if (!catParticles.isPlaying)
                catParticles.Play();
        }
        else
        {
            if (catParticles.isPlaying)
                catParticles.Stop();
        }
    }

    private void UpdateCatSpin()
    {
        float t = sequenceTime;

        // BEFORE triggerTime → only spin if player inside
        if (!sequenceLocked && !playerInside)
        {
            ReturnToOriginalRotation();
            return;
        }

        // 0 - 0.216s = no spin (2.184s)
        if (t < 0.216f) 
        {
            ReturnToOriginalRotation();
            return;
        }
        // 0.216 - 1.740s = normal spin
        if (t < 1.740f)
        {
            RotateNormal();
            return;
        }

        // 1.76 - 3.044s = no spin (2.184s)
        if (t < 3.066f)
        {
            ReturnToOriginalRotation();
            return;
        }

        // 3.944 - 5.087s = slow spin (1.143s)
        if (t < 5.230f)
        {
            RotateSlow();
            return;
        }

        // 5.087 - 6.262s = no spin (1.175s)
        if (t < 6.262f)
        {
            ReturnToOriginalRotation();
            return;
        }

        // 6.262 - 7.963s = normal spin (1.701s)
        if (t < 8.140f)
        {
            if (redLight != null)
                redLight.SetActive(true);
            RotateNormal();
            return;
        }

        // 7.963 - 9.655s = no spin (1.692s)
        if (t < 9.655f)
        {
            ReturnToOriginalRotation();
            return;
        }

        // 9.655 - 11.255s = normal spin (1.6s)
        if (t < 11.255f)
        {
            RotateNormal();
            return;
        }

        // 11.255 - 12.868s = no spin (1.613s)
        if (t < 12.868f)
        {
            ReturnToOriginalRotation();
            return;
        }

        // 12.868 - 14.654s = normal spin (1.786s)
        if (t < 14.654f)
        {
            RotateNormal();
            return;
        }

        // 14.654 - 16.050s = no spin (1.396s)
        if (t < 16.050f)
        {
            ReturnToOriginalRotation();
            return;
        }

        // 16.050 - 17.532s = normal spin (1.482s)
        if (t < 17.532f)
        {
            RotateNormal();
            return;
        }

        // 17.532 - 19.251s = no spin (1.719s)
        if (t < 19.251f)
        {
            ReturnToOriginalRotation();
            return;
        }

        // 19.251 - 20.809s = normal spin (1.558s)
        if (t < 20.809f)
        {
            RotateNormal();
            return;
        }

        // 20.809 - 22.461s = no spin (1.652s)
        if (t < 22.461f)
        {
            ReturnToOriginalRotation();
            return;
        }

        // 22.461 - 24 .059s = normal spin (2.598s)
        if (t < 24.059f)
        {
            RotateNormal();
            return;
        }

        // 24.059 - 25.656s = no spin (1.652s)
        if (t < 25.656f)
        {
            ReturnToOriginalRotation();
            return;
        }

        // 25.059 - 29.653s = 
        if (t < 29.653f)
        {
            RotateNormal();
            return;
        }

        // 29.653 - 32.058 - cat continues to spin cat rises up
        if (t < 32.058f)
        {
            float riseSpeed = 0.6f;
            RotateNormal();
            transform.position += Vector3.up * riseSpeed * Time.deltaTime;
            return;
        }
        //

        // 45.301s = normal spin (19.648s)
        if (t < 45.301f)
        {
            RotateNormal();
            return;
        }

        // 45.301 - 58.126s = normal spin continues (12.825s)
        if (t < 58.126f)
        {
            RotateNormal();
            return;
        }

        // After 58.126s -> sequence done, but cat stare
        if (t >= 58.126f)
        {
            // No more additional cats
            foreach (var c in extraCats)
                c.Deactivate();

            // No more invisiible wall    
            if (gateObject != null)
                gateObject.SetActive(false);

             if (redLight != null)
                redLight.SetActive(false);

            ReturnToOriginalRotation(); //stares at player
            
            return;
        }
    }

    private void RotateNormal()
    {
        transform.Rotate(0f, normalSpinSpeed * Time.deltaTime, 0f);
    }

    private void RotateSlow()
    {
        transform.Rotate(0f, slowSpinSpeed * Time.deltaTime, 0f);
    }

    private void ReturnToOriginalRotation()
    {
        //transform.rotation = originalRotation;
        if (player == null) return;

        Vector3 targetPos = player.position;
        targetPos.y = transform.position.y; // keep cat upright (no tilting)

        transform.LookAt(targetPos);
    }
}
