using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    public void SendToPosition(Transform t)
    {
        gameObject.transform.position = t.position;
    }
}
