using System;
using UI;
using UnityEditor;

namespace Editor
{
    [CustomEditor(typeof(InputFieldHandlerWithEnum))]
    public class InputFieldHandlerWithEnumEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var script = (InputFieldHandlerWithEnum)target;

            DrawDefaultInspector();

            if (script.scriptCible != null)
            {
                string[] variables = script.GetVariableNames();

                if (variables != null && variables.Length > 0)
                {
                    int selectedIndex = Array.IndexOf(variables, script.selectedVariable);
                    selectedIndex = EditorGUILayout.Popup("Selected Variable", selectedIndex, variables);

                    if (selectedIndex >= 0 && selectedIndex < variables.Length)
                    {
                        script.selectedVariable = variables[selectedIndex];
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("Aucune variable publique détectée dans le script cible.", MessageType.Warning);
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Assignez un script cible pour détecter ses variables publiques.", MessageType.Info);
            }
        }
    }
}