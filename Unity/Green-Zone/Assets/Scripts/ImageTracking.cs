using UnityEngine.XR.ARFoundation;
using UnityEngine;

public class ImageTracking : MonoBehaviour
{
    private ARTrackedImageManager arTrackedImagemManager;

    private void Awake()
    {
        arTrackedImagemManager = FindObjectOfType<ARTrackedImageManager>();
    }

    public void OnEnable()
    {
        arTrackedImagemManager.trackedImagesChanged += OnImageChanged;
    }

    public void OnDisable()
    {
        arTrackedImagemManager.trackedImagesChanged -= OnImageChanged;
    }

    public void OnImageChanged(ARTrackedImagesChangedEventArgs args)
    {

    }
}