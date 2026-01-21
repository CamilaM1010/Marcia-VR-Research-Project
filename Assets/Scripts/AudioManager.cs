using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public float defaultVolume = 0.75f;
    private const string VOLUME_KEY = "VolumeKey";
    [Header("Keep volume slider empty in scenes without settings.")]
    public Slider volumeSlider;

    void Start()
    {
        // Sets the volume based on user's previous value, or 75% if not specified
        float savedVolume = PlayerPrefs.GetFloat(VOLUME_KEY, defaultVolume);
        AudioListener.volume = savedVolume;

        // Debug.Log("Loaded AudioListener volume: " + AudioListener.volume); 

        // Updates volumeSlider to match previous volume
        if (volumeSlider != null) 
        {
            volumeSlider.value = savedVolume;
        }
    }

    public void SetVolumeFromSlider(float sliderValue)
    {
        // Sets the volume based on the slider value
        float roundedValue = Mathf.Round(sliderValue * 1000f) / 1000f;
        AudioListener.volume = Mathf.Clamp01(roundedValue);

        // Saves volume to playerprefs (persists when game is closed)
        PlayerPrefs.SetFloat(VOLUME_KEY, AudioListener.volume);
        PlayerPrefs.Save();

        // Debug.Log("Changed AudioListener volume: " + AudioListener.volume); 
        // Debug.Log("PlayerPrefs volume: " + PlayerPrefs.GetFloat(VOLUME_KEY));
    }
}