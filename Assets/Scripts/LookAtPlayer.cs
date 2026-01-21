using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public Transform player;

    void LateUpdate()
    {
        //transform.rotation = originalRotation;
        if (player == null) return;

        Vector3 targetPos = player.position;
        transform.LookAt(targetPos);
    }
}
