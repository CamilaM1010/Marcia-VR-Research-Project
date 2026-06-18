using UnityEngine;

public class WindowButton : MonoBehaviour
{
    public AudioSource audioSource;

    private bool isPressedV = false;
    private bool isPressedW = false;

    public GameObject[] hideableGroups;

    public void VolumeChange()
    {
        if (isPressedV)
        {
            audioSource.volume = 1f;
            isPressedV = false;
        }
        else
        {
            audioSource.volume = 0.4f;
            isPressedV = true;
        }
    }

    public void windowChange()
    {
        if (isPressedW)
        {
            foreach (GameObject group in hideableGroups)
            {
                group.SetActive(false);
            }

            isPressedW = false;
        }
        else
        {
            foreach (GameObject group in hideableGroups)
            {
                group.SetActive(true);
            }

            isPressedW = true;
        }
    }
    



}
