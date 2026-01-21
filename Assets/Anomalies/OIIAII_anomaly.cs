using UnityEngine;

public class OIIAII_anomaly : Anomaly
{

    public Dialogue dialogue;

    void Start()
    {
    }

    public override void Activate()
    {
        if(dialogue != null) 
        {
            dialogue.AddLine("Oh look, a cat.... I should pet it.");
        }
        SetChildrenActive(true);
    }

    public override void Deactivate()
    {
        foreach (var child in GetComponentsInChildren<Car_anomaly>())
        {
            child.ResetSequence();
        }
        
        SetChildrenActive(false);
    }

    private void SetChildrenActive(bool state)
    {
        foreach (Transform child in transform)
        {   
             if (child.CompareTag("IgnoreAnomaly"))
                continue;
                
            child.gameObject.SetActive(state);
        }
    }
}
