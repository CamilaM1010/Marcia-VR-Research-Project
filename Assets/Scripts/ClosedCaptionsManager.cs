using UnityEngine;
using UnityEngine.UI;

public class ClosedCaptionsManager : MonoBehaviour
{
    public int defaultCC = 0;
    private const string CC_KEY = "CC_KEY";
    [Header("Keep volume slider empty in scenes without settings.")]
    public Toggle ccToggle;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int toggleState = PlayerPrefs.GetInt(CC_KEY, defaultCC);
        ccToggle.isOn = (toggleState == 1);
        ccToggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    public void OnToggleValueChanged(bool newValue)
    {
        PlayerPrefs.SetInt(CC_KEY, newValue ? 1 : 0);
        PlayerPrefs.Save();
    }
}
