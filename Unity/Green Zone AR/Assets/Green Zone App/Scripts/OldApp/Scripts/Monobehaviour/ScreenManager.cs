using System.Collections;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{

    [SerializeField] public CanvasGroup fadeCanvasGroup;
    [SerializeField] public CanvasGroup splashCanvasGroup;
    [SerializeField] public CanvasGroup menuCanvasGroup;
    [SerializeField] public CanvasGroup gameModeCanvasGroup;
    [SerializeField] public CanvasGroup winCanvasGroup;
    [SerializeField] public CanvasGroup looseCanvasGroup;

    /// <summary>
    /// Fade Out screen
    /// </summary>
    /// <returns></returns>
    public IEnumerator FadeOutWidget(float fadeDuration, CanvasGroup fadeCanvas)
    {
        fadeCanvas.gameObject.SetActive(true);
        float timeStep = 0.1f;
        for (float currenttime = 0; currenttime < fadeDuration; currenttime += timeStep)
        {
            fadeCanvas.alpha = (1 - (currenttime / fadeDuration));
            yield return new WaitForSeconds(timeStep);
        }
        fadeCanvas.alpha = 0.0f;
        fadeCanvas.gameObject.SetActive(false);
    }
    /// <summary>
    /// Fade In screen
    /// </summary>
    /// <returns></returns>
    public IEnumerator FadeInWidget(float fadeDuration, CanvasGroup fadeCanvas)
    {
        fadeCanvas.gameObject.SetActive(true);
        float timeStep = 0.1f;
        for (float currenttime = 0; currenttime < fadeDuration; currenttime += timeStep)
        {
            fadeCanvas.alpha = (currenttime / fadeDuration);
            yield return new WaitForSeconds(timeStep);
        }
        fadeCanvas.alpha = 1.0f;
    }

    public IEnumerator CallSplash()
    {
        HideWidgets();
        // fade in bg
        StartCoroutine(FadeInWidget(2.0f, fadeCanvasGroup));
        // fad in widget
        yield return StartCoroutine(FadeInWidget(2.0f, splashCanvasGroup));
        float fadeDuration = 2.0f;
        yield return new WaitForSeconds(fadeDuration);
        yield return StartCoroutine(FadeOutWidget(2.0f, splashCanvasGroup));
        //StartCoroutine(FadeOutWidget(2.0f, fadeCanvasGroup));
        yield return StartCoroutine(CallMenu());
    }

    public IEnumerator CallMenu()
    {
        HideWidgets();
        // fade in bg
        StartCoroutine(FadeInWidget(2.0f, fadeCanvasGroup));
        // fad in widget
        yield return StartCoroutine(FadeInWidget(2.0f, menuCanvasGroup));
    }
    public IEnumerator CloseMenu()
    {
        HideWidgets();
        // fade out bg
        StartCoroutine(FadeOutWidget(2.0f, fadeCanvasGroup));
        // fad out widget
        yield return StartCoroutine(FadeOutWidget(2.0f, menuCanvasGroup));
    }

    public IEnumerator CallWin()
    {
        HideWidgets();
        // fade in bg
        StartCoroutine(FadeInWidget(2.0f, fadeCanvasGroup));
        // fad in widget
        yield return StartCoroutine(FadeInWidget(2.0f, winCanvasGroup));
    }

    public IEnumerator CallLoose()
    {
        HideWidgets();
        // fade in bg
        StartCoroutine(FadeInWidget(2.0f, fadeCanvasGroup));
        // fad in widget
        yield return StartCoroutine(FadeInWidget(2.0f, looseCanvasGroup));
    }

    public IEnumerator CallGameMode()
    {
        HideWidgets();
        // fade in bg
        StartCoroutine(FadeInWidget(2.0f, fadeCanvasGroup));
        // fad in widget
        yield return StartCoroutine(FadeInWidget(2.0f, gameModeCanvasGroup));
    }

    public void HideWidgets()
    {
        fadeCanvasGroup.gameObject.SetActive(false);
        splashCanvasGroup.gameObject.SetActive(false);
        menuCanvasGroup.gameObject.SetActive(false);
        gameModeCanvasGroup.gameObject.SetActive(false);
        winCanvasGroup.gameObject.SetActive(false);
        looseCanvasGroup.gameObject.SetActive(false);
    }

}
