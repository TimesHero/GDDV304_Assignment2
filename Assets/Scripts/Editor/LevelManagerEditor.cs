using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LevelManager component = (LevelManager)target;

        if (GUILayout.Button("Save Level"))
        {
            component.SaveLevel();
        }

        if (GUILayout.Button("Clear Level"))
        {
            component.ClearLevel();
        }

        if (GUILayout.Button("Load Level"))
        {
            component.LoadLevel();
        }
    }
}
