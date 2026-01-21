using UnityEngine;
// This file should be what manages/invokes the anomalies
public class Anomaly : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    // This method should be overrided by each anomaly so it can be activted in a custom manner
    public virtual void Activate()
    {
        Debug.Log("Generic Activate");
        gameObject.SetActive(true);
    }

    // This method may or may not need to be overrided by each anomaly so it can be deactivted in a custom manner
    public virtual void Deactivate()
    {
        Debug.Log("Generic Deactivate");
        gameObject.SetActive(false);
    }
}
