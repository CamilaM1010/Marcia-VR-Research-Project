using UnityEngine;

public class FootstepMovement : MonoBehaviour
{
    public CharacterController controller;
    public AudioSource source;

    public AudioClip walkLoop;
    public AudioClip sprintLoop;

    public float walkPitch = 1f;
    public float sprintPitch = 1.3f;
    public float minMoveSpeed = 0.2f; // how much velocity to count as moving

    void Update()
    {
        // Detect WASD input
        bool inputMoving =
            Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.D);

        // Actual movement (prevents wall-stuck footsteps)
        float trueSpeed =
            new Vector3(controller.velocity.x, 0, controller.velocity.z).magnitude;

        bool reallyMoving = inputMoving && trueSpeed > minMoveSpeed;

        bool isSprinting = Input.GetKey(KeyCode.LeftShift);

        if (reallyMoving)
        {
            AudioClip desiredClip = isSprinting ? sprintLoop : walkLoop;

            // Switch clips instantly if needed
            if (source.clip != desiredClip)
            {
                source.clip = desiredClip;
                source.pitch = isSprinting ? sprintPitch : walkPitch;
                source.Stop();
                source.Play();
            }

            // Start loop if not already playing
            if (!source.isPlaying)
                source.Play();
        }
        else
        {
            // Stop immediately when not moving
            if (source.isPlaying)
                source.Stop();
        }
    }
}
