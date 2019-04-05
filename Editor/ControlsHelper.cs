using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Popcron.Input
{
    public class ControlsHelper
    {
        private static SerializedProperty axesProperty;

        public static int AxesCount
        {
            get
            {
                if (axesProperty == null)
                {
                    SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
                    axesProperty = serializedObject.FindProperty("m_Axes");
                }

                return axesProperty.arraySize;
            }
        }

        private static SerializedProperty GetChildProperty(SerializedProperty item, string name)
        {
            //copy so we don't iterate original
            SerializedProperty copiedProperty = item.Copy();

            bool moreChildren = true;

            //step one level into child
            copiedProperty.Next(true);

            //iterate on all properties one level deep
            while (moreChildren)
            {
                //found the child we were looking for
                if (copiedProperty.name.Equals(name))
                    return copiedProperty;

                //move to the next property
                moreChildren = copiedProperty.Next(false);
            }

            //if we get here we didn't find it
            return null;
        }

        public static void LoadAllControllers()
        {
            //find a controls manager in the scene
            ControlsManager controlsManager = Object.FindObjectOfType<ControlsManager>();
            if (!controlsManager)
            {
                controlsManager = new GameObject("ControlsManager").AddComponent<ControlsManager>();

                //mark scene dirty
                Scene activeScene = SceneManager.GetActiveScene();
                EditorSceneManager.MarkSceneDirty(activeScene);
            }

            controlsManager.Controllers.Clear();

            string[] guids = AssetDatabase.FindAssets("t:Popcron.Input.ControllerType");
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                ControllerType controller = AssetDatabase.LoadAssetAtPath<ControllerType>(path);
                controlsManager.Controllers.Add(controller);
            }

            EditorUtility.SetDirty(controlsManager);
        }

        public static void AddController(ControllerType controller)
        {
            //find a controls manager in the scene
            ControlsManager controlsManager = Object.FindObjectOfType<ControlsManager>();
            if (!controlsManager)
            {
                LoadAllControllers();
                return;
            }

            controlsManager.Controllers.Add(controller);
            EditorUtility.SetDirty(controlsManager);
        }

        private static bool Exists(int axisNumber, int joyStick)
        {
            string name = Controls.GetAxisName(joyStick, axisNumber);
            for (int i = 0; i < axesProperty.arraySize; i++)
            {
                SerializedProperty item = axesProperty.GetArrayElementAtIndex(i);
                if (GetChildProperty(item, "m_Name").stringValue.Equals(name))
                {
                    return true;
                }
            }

            return false;
        }

        private static void Add(int axisNumber, int joyStick)
        {
            axesProperty.arraySize++;
            SerializedProperty item = axesProperty.GetArrayElementAtIndex(axesProperty.arraySize - 1);

            SerializedProperty name = GetChildProperty(item, "m_Name");
            name.stringValue = Controls.GetAxisName(joyStick, axisNumber);

            GetChildProperty(item, "descriptiveName").stringValue = "";
            GetChildProperty(item, "descriptiveNegativeName").stringValue = "";
            GetChildProperty(item, "positiveButton").stringValue = "";
            GetChildProperty(item, "negativeButton").stringValue = "";
            GetChildProperty(item, "altNegativeButton").stringValue = "";
            GetChildProperty(item, "altPositiveButton").stringValue = "";
            GetChildProperty(item, "gravity").floatValue = 0f;
            GetChildProperty(item, "dead").floatValue = 0.1f;
            GetChildProperty(item, "sensitivity").floatValue = 1f;
            GetChildProperty(item, "snap").boolValue = false;
            GetChildProperty(item, "invert").boolValue = false;
            GetChildProperty(item, "type").intValue = 2;
            GetChildProperty(item, "axis").intValue = axisNumber;
            GetChildProperty(item, "joyNum").intValue = joyStick + 1;
        }

        [MenuItem("Popcron/Input/Reset input entries")]
        public static void ResetUnityInputs()
        {
            //add our example axes (if not already present when we read the file below)
            SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
            axesProperty = serializedObject.FindProperty("m_Axes");

            //check how many inputs there are, if it changed then reset the inputs

            //delete all
            axesProperty.arraySize = 0;

            //create all
            for (int j = 0; j < Controls.MaxControllers; j++)
            {
                for (int a = 0; a < Controls.MaxJoystickAxes; a++)
                {
                    if (!Exists(a, j))
                    {
                        Add(a, j);
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
