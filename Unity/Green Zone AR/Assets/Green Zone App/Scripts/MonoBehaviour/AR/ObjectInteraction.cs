using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    [Tooltip("The Dialog Canves for this Interactve Object")]
    [SerializeField] private GameObject DialogWidget;
    private bool isInFocus = false;
    
    float sec = 3.0f;

    public void IsInFocus()
    {
        isInFocus = true;
        StartCoroutine(StartTalking(sec));
    }

    public void IsOutOfFocus()
    {
        isInFocus = false;
    }

    /// <summary>
    /// Show Icon for given time using Coroutine
    /// </summary>
    /// <param name="sec">seconds to show icons</param>
    /// <returns>pause</returns>
    IEnumerator StartTalking(float sec)
    {
        yield return new WaitForSeconds(0.1f);
        DialogWidget.SetActive(true);
        yield return new WaitForSeconds(sec);
        DialogWidget.SetActive(false);
    }

}
