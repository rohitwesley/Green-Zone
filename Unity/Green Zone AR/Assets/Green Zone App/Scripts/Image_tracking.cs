using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine;


public class Image_tracking : MonoBehaviour
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
