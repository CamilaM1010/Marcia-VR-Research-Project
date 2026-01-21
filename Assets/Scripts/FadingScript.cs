using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeController : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 0.15f;    // faster blink
    public int blinkCount = 1;           // how many blinks at start
    public float pauseBetweenBlinks = 0.03f;

    public static bool blinkedOnce = false;

    void Start()
    {

        Debug.Log("FadeController Start running");
        if (!blinkedOnce)
        {
            blinkedOnce = true;
            StartCoroutine(BlinkSequence());
        }
        else
        {
            StartCoroutine(FadeIn());
        }
    }

    private IEnumerator BlinkSequence()
    {
        // start fully black (closed eyes)
        SetAlpha(1);

        // Do several blinks
        for (int i = 0; i < blinkCount; i++)
        {
            yield return StartCoroutine(FadeIn());   // open eyes
            yield return new WaitForSeconds(pauseBetweenBlinks);
            yield return StartCoroutine(FadeOut());  // close eyes
        }

        // Finally open eyes fully and stay open
        yield return StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn()
    {
        Color c = fadeImage.color;
        for (float t = 1; t >= 0; t -= Time.deltaTime / fadeDuration)
        {
            c.a = t;
            fadeImage.color = c;
            yield return null;
        }
    }

    public IEnumerator FadeOut()
    {
        Color c = fadeImage.color;
        for (float t = 0; t <= 1; t += Time.deltaTime / fadeDuration)
        {
            c.a = t;
            fadeImage.color = c;
            yield return null;
        }
    }

    private void SetAlpha(float a)
    {
        Color c = fadeImage.color;
        c.a = a;
        fadeImage.color = c;
    }
}
