using UnityEngine;

public class ExitSignManager : MonoBehaviour
{
public GameObject leftRedExitSign;
public GameObject leftGreenExitSign;
public GameObject rightRedExitSign;
public GameObject rightGreenExitSign;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateExitSignColor(bool playerWalkedThroughRightDoor)
    {
        leftGreenExitSign.SetActive(playerWalkedThroughRightDoor);
        // rightRedExitSign.SetActive(playerWalkedThroughRightDoor);
        rightGreenExitSign.SetActive(!playerWalkedThroughRightDoor);
        // leftRedExitSign.SetActive(!playerWalkedThroughRightDoor);
    }
}
