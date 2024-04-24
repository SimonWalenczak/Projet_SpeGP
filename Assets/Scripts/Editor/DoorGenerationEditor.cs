using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RoomExpand))]
public class DoorGenerationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RoomExpand roomExpand = (RoomExpand)target;

        if (GUILayout.Button("Generate Doors"))
        {
            roomExpand.GenerateDoorsRandomPosition();
        }
    }
}