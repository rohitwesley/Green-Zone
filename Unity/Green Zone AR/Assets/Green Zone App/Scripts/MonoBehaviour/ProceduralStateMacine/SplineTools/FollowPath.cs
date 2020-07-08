using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MovementTools
{
    public class FollowPath : MonoBehaviour
    {
        /// <summary>
        /// FollowPath Model
        /// </summary>
        [Tooltip("Show Debug Gizmos")]
        [SerializeField] public bool isDebug = true;
        
        [Tooltip("Resoluton of the interpolation")]
        [SerializeField] private SplineTool bazierPath;
        [Tooltip("Object to follow Path")]
        [SerializeField] private Transform bazierObject;
        [Tooltip("Flip Object following Path")]
        [SerializeField] private bool flipObject = false;

        [Tooltip("Spacing between each interpolated point on Spline")]
        [Range(1.0f, 100.0f)]
        [SerializeField] private float segmentResolution = 100f;
        [Tooltip("Resoluton of the interpolation")]
        [Range(0.0f, 0.1f)]
        [SerializeField] private float splineResolution = 0.01f;
        [Tooltip("Spped of Path")]
        [Range(0.0f,1.0f)]
        [SerializeField] private float speed = 0.0003f;
        
        float distance = 0;
        Vector3[] splinePath;

        private void Update()
        {
            UpdateSplinePath();
            MoveObjectOnSpline();
        }

        private void UpdateSplinePath()
        {
            // TODO check if this actualy only updates when the path is updated and not every update.
            if (splinePath != bazierPath.path.CalculateEvenlySpacedPoints(1 / segmentResolution, splineResolution))
                splinePath = bazierPath.path.CalculateEvenlySpacedPoints(1 / segmentResolution, splineResolution);
        }

        private void MoveObjectOnSpline()
        {
            UpdateDistanceOnSpline();
            UpdateObjectPostionOnSpline();
            UpdateObjectRotationOnSpline();
        }

        private void UpdateDistanceOnSpline()
        {
            // Update the current position on the spline based on the speed and spline segment count
            float speedSpline = (float)splinePath.Length * speed;
            distance += speedSpline + Time.deltaTime;
            if (distance >= splinePath.Length)
            {
                distance = splinePath.Length - distance;
            }
        }

        private void UpdateObjectPostionOnSpline()
        {
            // Update object posoition on spline
            Vector3 currentPosition;
            int segmentIndex = (int)distance;
            currentPosition = Vector3.Lerp(splinePath[segmentIndex], splinePath[segmentIndex + 1], 1.0f / distance % speed);
            bazierObject.position = currentPosition;
        }

        private void UpdateObjectRotationOnSpline()
        {
            // TODO smoothly look forward
            // Update object rotation on spline
            Vector3 currentPosition;
            int segmentIndex = (int)distance;
            currentPosition = Vector3.Lerp(splinePath[segmentIndex], splinePath[segmentIndex + 1], 1.0f);
            bazierObject.LookAt(currentPosition);
            if (flipObject) bazierObject.rotation = Quaternion.LookRotation(-bazierObject.forward, bazierObject.up);
        }

        /// <summary>
        /// FollowPath Debuger 
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            if (isDebug)
            {
                if(splinePath == null)
                {
                    UpdateSplinePath();
                    MoveObjectOnSpline();
                }

                if(splinePath.Length>0)
                {
                    foreach (Vector3 p in splinePath)
                    {
                        Gizmos.color = Color.blue;
                        Gizmos.DrawSphere(p, .1f);
                    }
                    
                    Vector3 currentPosition;
                    
                    int index = (int)distance;
                    currentPosition = splinePath[index];
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawSphere(currentPosition, .4f);

                    currentPosition = Vector3.Lerp(splinePath[index], splinePath[index + 1], 1.0f / distance % speed);
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(currentPosition, .4f);
                }
            }

        }


    }

}
