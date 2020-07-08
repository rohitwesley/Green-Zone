using UnityEditor;
using UnityEngine;

namespace MovementTools
{
    [CustomEditor(typeof(MapGenerator))]
    public class MapGeneratorEditor : Editor
    {
        MapGenerator mapGen;
        Editor mapEditor;
        public override void OnInspectorGUI()
        {
            using (var checkUpdate = new EditorGUI.ChangeCheckScope())
            {
                base.OnInspectorGUI();
                if (checkUpdate.changed)
                {
                    mapGen.OnObjectSettingsUpdated();
                }
            }

            if (GUILayout.Button("Generate Map"))
            {
                mapGen.GenerateMap();
            }

            // Get scriptable object setting into the inspector to use in this script
            DrawSettingsEditor(mapGen.mapSettings, mapGen.OnObjectSettingsUpdated, ref mapGen.foldout, ref mapEditor);

        }

        private void DrawSettingsEditor(MapSettings settings, System.Action onSettingsUpdated, ref bool foldout, ref Editor editor)
        {
            if (settings != null)
            {
                foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);
                // update in real time if auto update checked
                using (var checkUpdate = new EditorGUI.ChangeCheckScope())
                {
                    if (foldout)
                    {
                        CreateCachedEditor(settings, null, ref editor);
                        // Editor editor = CreateEditor(settings);
                        editor.OnInspectorGUI();
                        if (checkUpdate.changed)
                        {
                            if (onSettingsUpdated != null)
                            {
                                onSettingsUpdated();
                            }
                        }

                    }

                }

            }
        }


        private void OnEnable()
        {
            mapGen = (MapGenerator)target;
        }

    }


}
