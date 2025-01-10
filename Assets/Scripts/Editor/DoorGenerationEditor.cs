using Procedural;
using UnityEditor;

namespace Editor
{
    [CustomEditor(typeof(RoomPrimalExpand))]
    public class DoorGenerationEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            // RoomExpand roomExpand = (RoomExpand)target;
            //
            // if (GUILayout.Button("Generate Doors"))
            // {
            //     roomExpand.GenerateDoorsRandomPosition();
            // }
        }
    }
}