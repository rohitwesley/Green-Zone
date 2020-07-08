using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelStateManager))]
public class LevelStateManagerEditor : Editor
{
    LevelStateManager LevelGen;

    public override void OnInspectorGUI()
    {
        using (var checkUpdate = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if (checkUpdate.changed)
            {
                //LevelGen.GenerateMap();
            }
        }

        if (GUILayout.Button("Generate Level"))
        {
            //LevelGen.GenerateMap();
        }
        if (GUILayout.Button("Clear Level"))
        {
           // LevelGen.ResetMap();
        }
    }

    private void OnEnable()
    {
        LevelGen = (LevelStateManager)target;
    }
}
