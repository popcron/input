using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

using Popcron.Input;

[CustomPropertyDrawer(typeof(ControllerBind))]
public class ControllerBindDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.serializedObject.isEditingMultipleObjects) return;

        SerializedProperty name = property.FindPropertyRelative("name");

        EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, 16), property);
        if (property.isExpanded)
        {
            SerializedProperty isKey = property.FindPropertyRelative("isKey");

            name.stringValue = EditorGUI.TextField(new Rect(position.x + 10, position.y + 18, position.width - 10, 16), "Name", name.stringValue);
            isKey.boolValue = EditorGUI.Toggle(new Rect(position.x + 10, position.y + 18 * 2, position.width - 10, 16), "Is Key", isKey.boolValue);
            if (isKey.boolValue)
            {
                SerializedProperty buttonNumber = property.FindPropertyRelative("buttonNumber");
                string[] options = new string[]
                {
                    "Button 0",
                    "Button 1",
                    "Button 2",
                    "Button 3",
                    "Button 4",
                    "Button 5",
                    "Button 6",
                    "Button 7",
                    "Button 8",
                    "Button 9",
                    "Button 10",
                    "Button 11",
                    "Button 12",
                    "Button 13",
                    "Button 14",
                    "Button 15",
                    "Button 16",
                    "Button 17",
                    "Button 18",
                    "Button 19"
                };

                int newIndex = EditorGUI.Popup(new Rect(position.x + 20, position.y + 18 * 3, position.width - 20, 16), "Button", buttonNumber.intValue, options);
                if (newIndex != buttonNumber.intValue)
                {
                    buttonNumber.intValue = newIndex;
                }
            }
            else
            {
                SerializedProperty axisNumber = property.FindPropertyRelative("axisNumber");
                SerializedProperty axisDirection = property.FindPropertyRelative("axisDirection");

                axisNumber.intValue = EditorGUI.IntField(new Rect(position.x + 20, position.y + 18 * 3, position.width - 20, 16), "Axis", axisNumber.intValue);
                axisNumber.intValue = Mathf.Clamp(axisNumber.intValue, 0, 27);

                axisDirection.enumValueIndex = EditorGUI.Popup(new Rect(position.x + 20, position.y + 18 * 4, position.width - 20, 16), "Direction", axisDirection.enumValueIndex, axisDirection.enumDisplayNames);
            }
        }

        property.serializedObject.ApplyModifiedProperties();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!property.isExpanded)
        {
            return base.GetPropertyHeight(property, label);
        }

        SerializedProperty button = property.FindPropertyRelative("isKey");
        if (button.boolValue)
        {
            return 18 * 4;
        }

        return 18 * 5;
    }
}
