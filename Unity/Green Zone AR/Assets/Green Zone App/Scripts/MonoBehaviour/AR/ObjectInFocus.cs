using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ObjectInFocus : MonoBehaviour
{
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    [SerializeField] private ObjectInteraction FocusObject;

    // ARRaycastManager m_RaycastManager;
    // static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    void Awake()
    {
        // m_RaycastManager = FindObjectOfType<ARSessionOrigin>().GetComponent<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Vector3 positionInWorldSpace = Camera.main.ScreenToWorldPoint(touch.position);
                var ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit))
                // if (m_RaycastManager.Raycast(touch.position, s_Hits, FocusObject.gameObject))
                {
                    FocusObject.IsInFocus();
                }
                else
                {
                    FocusObject.IsOutOfFocus();
                }
            }
        }
    }

}
