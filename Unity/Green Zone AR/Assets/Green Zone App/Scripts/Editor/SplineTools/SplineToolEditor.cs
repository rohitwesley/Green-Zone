using UnityEditor;
using UnityEngine;

namespace MovementTools
{
    [CustomEditor(typeof(SplineTool))]
    public class SplineToolEditor : Editor
    {
        
        SplineTool creator;
        BezierCurve Path
        {
            get
            {
                return creator.path;
            }
        }

        const float segmentSelectDistanceThreshold = .1f;
        int selectedSegmentIndex = -1;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUI.BeginChangeCheck();
            if (GUILayout.Button("Create new"))
            {
                Undo.RecordObject(creator, "Create new");
                creator.CreatePath();
            }

            bool isClosed = GUILayout.Toggle(Path.IsClosed, "Closed");
            if (isClosed != Path.IsClosed)
            {
                Undo.RecordObject(creator, "Toggle closed");
                Path.IsClosed = isClosed;

            }

            bool autoSetControlPoints = GUILayout.Toggle(Path.AutoSetControlPoints, "Auto Set Control Points");
            if (autoSetControlPoints != Path.AutoSetControlPoints)
            {
                Undo.RecordObject(creator, "Toggle auto set controls");
                Path.AutoSetControlPoints = autoSetControlPoints;
            }

            if (EditorGUI.EndChangeCheck())
            {
                SceneView.RepaintAll();
            }
        }

        void OnSceneGUI()
        {
            // Get User Input to draw Spline 
            Input();
            // Draw Spline GUI Info in World Space
            Draw();
        }

        void Input()
        {
            Event guiEvent = Event.current;
            Vector3 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;

            // if shift + mouse click then create new Control point where mouse is
            if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
            {
                if (selectedSegmentIndex != -1)
                {
                    Undo.RecordObject(creator, "Split segment");
                    Path.SplitSegment(mousePos, selectedSegmentIndex);
                }
                else if (!Path.IsClosed)
                {
                    Undo.RecordObject(creator, "Add segment");// record for undo event
                    Path.AddSegment(mousePos);// add segments
                }
            }

            if (guiEvent.type == EventType.MouseDown && guiEvent.button == 1)
            {
                float minDstToAnchor = creator.anchorDiameter * .5f;
                int closestAnchorIndex = -1;

                for (int i = 0; i < Path.NumPoints; i += 3)
                {
                    float dst = Vector3.Distance(mousePos, Path[i]);
                    if (dst < minDstToAnchor)
                    {
                        minDstToAnchor = dst;
                        closestAnchorIndex = i;
                    }
                }

                if (closestAnchorIndex != -1)
                {
                    Undo.RecordObject(creator, "Delete segment");
                    Path.DeleteSegment(closestAnchorIndex);
                }
            }

            if (guiEvent.type == EventType.MouseMove)
            {
                float minDstToSegment = segmentSelectDistanceThreshold;
                int newSelectedSegmentIndex = -1;

                for (int i = 0; i < Path.NumSegments; i++)
                {
                    Vector3[] points = Path.GetPointsInSegment(i);
                    float dst = HandleUtility.DistancePointBezier(mousePos, points[0], points[3], points[1], points[2]);
                    if (dst < minDstToSegment)
                    {
                        minDstToSegment = dst;
                        newSelectedSegmentIndex = i;
                    }
                }

                if (newSelectedSegmentIndex != selectedSegmentIndex)
                {
                    selectedSegmentIndex = newSelectedSegmentIndex;
                    HandleUtility.Repaint();
                }
            }

        }

        void Draw()
        {

            // Draw 2 Control lines and the Bezier Curve
            for (int i = 0; i < Path.NumSegments; i++)
            {
                Vector3[] points = Path.GetPointsInSegment(i);
                if (creator.displayControlPoints)
                {
                    Handles.color = Color.black;
                    Handles.DrawLine(points[1], points[0]);
                    Handles.DrawLine(points[2], points[3]);
                }
                Color segmentCol = (i == selectedSegmentIndex && Event.current.shift) ? creator.selectedSegmentCol : creator.segmentCol;
                Handles.DrawBezier(points[0], points[3], points[1], points[2], segmentCol, null, 2);
            }

            // Draw the 4 Control Points
            for (int i = 0; i < Path.NumPoints; i++)
            {
                if (i % 3 == 0 || creator.displayControlPoints)
                {
                    Handles.color = (i % 3 == 0) ? creator.anchorCol : creator.controlCol;
                    float handleSize = (i % 3 == 0) ? creator.anchorDiameter : creator.controlDiameter;
                    Vector3 newPos = Handles.FreeMoveHandle(Path[i], Quaternion.identity, handleSize, Vector3.zero, Handles.CylinderHandleCap);
                    if (Path[i] != newPos)
                    {
                        Undo.RecordObject(creator, "Move point");
                        Path.MovePoint(i, newPos);
                    }
                }
            }
        }

        private void OnEnable()
        {
            creator = (SplineTool)target;
            if (creator.path == null)
            {
                creator.CreatePath();
            }
        }
    
    }
}
