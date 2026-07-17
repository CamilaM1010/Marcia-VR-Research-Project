using UnityEngine;
using UnityEngine.Playables;
using System.Collections;

public class TimelineStart : MonoBehaviour
{
    // Gets playable director (timeline object)
    public PlayableDirector director;
    private bool started = false;

    // This is needed for starting game with space bard
    /*void Update()
    {
        if (!started && Input.GetKeyDown(KeyCode.Space))
        {
            director.Play();
            started = true;
        }
    }*/

    IEnumerator Start()
    {
        yield return new WaitForSeconds(20f);

        if (!started)
        {
            director.Play();
            started = true;
        }
    }


}
