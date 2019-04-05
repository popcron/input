using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Popcron.Input
{
    [CustomPropertyDrawer(typeof(MapBind))]
    public class MapBindDrawer : PropertyDrawer
    {
        private const float Height = 18;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = Height;

            SerializedProperty name = property.FindPropertyRelative("name");
            SerializedProperty binds = property.FindPropertyRelative("binds");

            EditorGUI.PropertyField(position, property, label);
            if (property.isExpanded)
            {
                Rect rect = EditorGUI.IndentedRect(position);
                rect.height = Height;

                //name field
                rect.y += Height;
                EditorGUI.PropertyField(rect, name);

                //binds array field
                rect.y += Height;
                EditorGUI.PropertyField(rect, binds, true);

                if (binds.isExpanded)
                {
                    GUIStyle style = new GUIStyle(EditorStyles.miniLabel)
                    {
                        alignment = TextAnchor.UpperRight,
                        richText = true
                    };

                    //skip the size field
                    rect.y += Height;
                    rect.width -= 3;

                    for (int i = 0; i < binds.arraySize; i++)
                    {
                        rect.y += Height;
                        SerializedProperty element = binds.GetArrayElementAtIndex(i);

                        string type = Controls.GetBindType(element.stringValue);
                        EditorGUI.LabelField(rect, type, style);
                    }
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = Height;
            SerializedProperty binds = property.FindPropertyRelative("binds");

            if (property.isExpanded)
            {
                height += Height * 2;
                if (binds.isExpanded)
                {
                    height += (binds.arraySize + 1) * Height;
                }
            }

            return height;
        }
    }
}