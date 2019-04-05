using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Popcron.Input
{
    [CustomPropertyDrawer(typeof(ControllerBind))]
    public class ControllerBindDrawer : PropertyDrawer
    {
        private const float Height = 18;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty name = property.FindPropertyRelative("name");

            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, 16), property);
            if (property.isExpanded)
            {
                SerializedProperty isKey = property.FindPropertyRelative("isKey");

                name.stringValue = EditorGUI.TextField(new Rect(position.x + 10, position.y + Height, position.width - 10, 16), "Name", name.stringValue);
                isKey.boolValue = EditorGUI.Toggle(new Rect(position.x + 10, position.y + Height * 2, position.width - 10, 16), "Is Key", isKey.boolValue);
                if (isKey.boolValue)
                {
                    SerializedProperty buttonNumber = property.FindPropertyRelative("buttonNumber");
                    string[] options = new string[Controls.MaxJoystickButtons];
                    for (int i = 0; i < Controls.MaxJoystickButtons; i++)
                    {
                        options[i] = "Button " + 0;
                    }

                    int newIndex = EditorGUI.Popup(new Rect(position.x + 20, position.y + Height * 3, position.width - 20, 16), "Button", buttonNumber.intValue, options);
                    if (newIndex != buttonNumber.intValue)
                    {
                        buttonNumber.intValue = newIndex;
                    }
                }
                else
                {
                    SerializedProperty axisNumber = property.FindPropertyRelative("axisNumber");
                    SerializedProperty axisDirection = property.FindPropertyRelative("axisDirection");

                    axisNumber.intValue = EditorGUI.IntField(new Rect(position.x + 20, position.y + Height * 3, position.width - 20, 16), "Axis", axisNumber.intValue);
                    axisNumber.intValue = Mathf.Clamp(axisNumber.intValue, 0, 27);

                    axisDirection.enumValueIndex = EditorGUI.Popup(new Rect(position.x + 20, position.y + Height * 4, position.width - 20, 16), "Direction", axisDirection.enumValueIndex, axisDirection.enumDisplayNames);
                }
            }

            property.serializedObject.ApplyModifiedProperties();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!property.isExpanded)
            {
                return Height;
            }

            SerializedProperty button = property.FindPropertyRelative("isKey");
            if (button.boolValue)
            {
                return Height * 4;
            }

            return Height * 5;
        }
    }
}