using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(LevelScript))]
public class LevelScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();

        LevelScript myTarget = (LevelScript)target;

        myTarget.experience = EditorGUILayout.IntField("Experience", myTarget.experience);
        //myTarget.name = EditorGUILayout.TextField("Name", myTarget.name);
        EditorGUILayout.HelpBox("This is a level field", MessageType.Info);
        EditorGUILayout.LabelField("Level", myTarget.Level.ToString());

        if (GUILayout.Button("Debug Level"))
        {
            myTarget.Action();
        }
    }
}
