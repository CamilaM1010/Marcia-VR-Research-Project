using UnityEngine;

public class LaptopReplacementAnomaly : Anomaly
{

    public GameObject laptop;
    public GameObject replacement;
    public AudioSource audioSource;
    public AnomalyCaption captions;
    
    public override void Activate()
    {
        base.Activate();

        laptop.SetActive(false);
        replacement.SetActive(true);
        InvokeRepeating(nameof(SFX), 0f, 1f);
        
    }

    public override void Deactivate()
    {
        base.Deactivate();
        replacement.SetActive(false);
        laptop.SetActive(true);
        CancelInvoke();
    }

    private void SFX()
    {
        if (Random.Range(0, 3) == 0)
        {
            Debug.Log("SFX");
            audioSource.Play();
            int closedCaptions = PlayerPrefs.GetInt("CC_KEY", 0);
            if (captions != null && closedCaptions == 1)
            {
                if (!captions.gameObject.activeSelf)
                {
                    captions.gameObject.SetActive(true);
                }
                captions.ShowCaption("hiss", this.transform);
            }
        }
        else
        {
            Debug.Log("Silence");
        }
    }
}
