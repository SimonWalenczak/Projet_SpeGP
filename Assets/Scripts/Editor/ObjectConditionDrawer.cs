using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ObjectCondition))]
public class ObjectConditionDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), GUIContent.none);

        float width = position.width / 3f;
        Rect objToPlaceRect = new Rect(position.x, position.y, width - 5, position.height);
        Rect conditionObjectRect = new Rect(position.x + width, position.y, width - 5, position.height);
        Rect objConditionRect = new Rect(position.x + 2 * width, position.y, width - 5, position.height);
        
        SerializedProperty objToPlace = property.FindPropertyRelative("ObjToPlace");
        SerializedProperty conditionObject = property.FindPropertyRelative("ConditionObject");
        SerializedProperty objCondition = property.FindPropertyRelative("ObjCondition");
        
        EditorGUI.PropertyField(objToPlaceRect, objToPlace, GUIContent.none);
        EditorGUI.PropertyField(conditionObjectRect, conditionObject, GUIContent.none);
        EditorGUI.PropertyField(objConditionRect, objCondition, GUIContent.none);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight;
    }
}