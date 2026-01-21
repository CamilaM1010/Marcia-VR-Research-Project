using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public GameObject settingsMenuPanel;
    public GameObject settingsIcon;

    void Start()
    {
        // Hides the cursor at start of game
        Cursor.visible = false;

        // Locks the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked; 
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ToggleSettingsMenu();

        // Ensure cursor state matches menu
        if (settingsMenuPanel != null)
        {
            if (settingsMenuPanel.activeSelf)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }


    public void ToggleSettingsMenu()
    {
        if (settingsMenuPanel != null)
        {
            if(settingsIcon != null)
            {
                settingsIcon.SetActive(!settingsIcon.activeSelf);
            }
            settingsMenuPanel.SetActive(!settingsMenuPanel.activeSelf);

            if (settingsMenuPanel.activeSelf)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}