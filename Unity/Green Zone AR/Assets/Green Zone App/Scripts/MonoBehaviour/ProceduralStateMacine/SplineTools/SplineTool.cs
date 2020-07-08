using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MovementTools
{
    public class SplineTool : MonoBehaviour
    {
        /// <summary>
        /// SplineTool Model
        /// </summary>
        [Tooltip("Show Debug Gizmos")]
        [SerializeField] public bool isDebug = true;

        [HideInInspector]
        public BezierCurve path;

        [Tooltip("Spline Anchor Gizmo")]
        [SerializeField] public Color anchorCol = Color.red;
        [Tooltip("Spline Control Point Gizmo")]
        [SerializeField] public Color controlCol = Color.white;
        [Tooltip("Spline Segment Gizmo")]
        [SerializeField] public Color segmentCol = Color.green;
        [Tooltip("Spline Select Gizmo")]
        [SerializeField] public Color selectedSegmentCol = Color.yellow;
        [Tooltip("Spline Anchor Gizmo Diameter")]
        [SerializeField] public float anchorDiameter = .1f;
        [Tooltip("Spline Control Gizmo Diameter")]
        [SerializeField] public float controlDiameter = .075f;
        [Tooltip("Show Control Points Gizmos")]
        [SerializeField] public bool displayControlPoints = true;

        Transform splinePath;

        private void Update()
        {
            if (splinePath != transform)
                Reset();
        }
        public void CreatePath()
        {
            path = new BezierCurve(splinePath.position);
        }

        public void Reset()
        {
            splinePath = transform;
            CreatePath();
        }


    }


}
