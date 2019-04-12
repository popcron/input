using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Popcron.Input
{
    using Input = UnityEngine.Input;

    [CustomEditor(typeof(ControlsManager))]
    public class ControlsManagerInspector : Editor
    {
        private ControlsManager manager;

        private void OnEnable()
        {
            manager = target as ControlsManager;
        }

        public override bool RequiresConstantRepaint()
        {
            return true;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Application.isPlaying)
            {

                EditorGUILayout.LabelField("Connected Controllers: ");
                EditorGUI.indentLevel++;
                {
                    for (int j = 0; j < Controls.Controllers.Count; j++)
                    {
                        int joyStick = Controls.Controllers[j].JoyStick;
                        EditorGUILayout.LabelField(Controls.Controllers[j].Name);
                        EditorGUI.indentLevel++;
                        {
                            //show axes
                            for (int a = 0; a < Controls.MaxJoystickAxes; a++)
                            {
                                string axisName = Controls.GetAxisName(joyStick, a);
                                float value = Input.GetAxisRaw(axisName);
                                if (value != 0)
                                {
                                    EditorGUILayout.LabelField(axisName + ": " + value, EditorStyles.miniLabel);
                                }
                                else
                                {
                                    EditorGUILayout.LabelField(axisName + ": ", EditorStyles.miniLabel);
                                }
                            }

                            //show buttons
                            for (int b = 0; b < Controls.MaxJoystickButtons; b++)
                            {
                                string buttonName = Controls.GetButtonName(j, b);
                                bool value = Input.GetKey(buttonName);
                                if (!value)
                                {
                                    EditorGUILayout.LabelField(buttonName + ": ", EditorStyles.miniLabel);
                                }
                                else
                                {
                                    EditorGUILayout.LabelField(buttonName + ": Pressed", EditorStyles.miniLabel);
                                }
                            }
                        }
                        EditorGUI.indentLevel--;
                    }
                }

                EditorGUI.indentLevel--;
            }
            else
            {
                EditorGUILayout.LabelField("Game not running, inputs wont be detected.", EditorStyles.miniLabel);
            }

            //check if map is present
            if (!manager.map)
            {
                EditorGUILayout.HelpBox("No input Map being used.", MessageType.Error);

                //create a new map
                if (GUILayout.Button("Create new map"))
                {
                    Map map = CreateInstance<Map>();
                    map.name = "New controller";

                    string path = Path.Combine("Assets", "New input map.asset");
                    AssetDatabase.CreateAsset(map, path);

                    manager.map = map;
                    EditorUtility.SetDirty(manager);
                    EditorGUIUtility.PingObject(map);
                }
            }

            bool hasControllers = false;
            for (int i = 0; i < manager.Controllers.Count; i++)
            {
                if (manager.Controllers[i])
                {
                    hasControllers = true;
                    break;
                }
            }

            //no default controller
            if (!manager.defaultController)
            {
                EditorGUILayout.HelpBox("No default controller is present.", MessageType.Error);

                //create a new controller
                if (GUILayout.Button("Create default controller"))
                {
                    ControllerType controller = CreateInstance<ControllerType>();
                    controller.name = "Default controller";

                    string path = Path.Combine("Assets", "Default controller.asset");
                    AssetDatabase.CreateAsset(controller, path);

                    manager.defaultController = controller;
                    EditorUtility.SetDirty(manager);

                }
            }

            //check how many controllers are present
            if (!hasControllers)
            {
                EditorGUILayout.HelpBox("No controllers are present.", MessageType.Warning);

                //create a new controller
                if (GUILayout.Button("Create new controller"))
                {
                    ControllerType controller = CreateInstance<ControllerType>();
                    controller.name = "New controller";

                    string path = Path.Combine("Assets", "New controller.asset");
                    AssetDatabase.CreateAsset(controller, path);

                    //populate the first empty slot
                    bool added = false;
                    for (int i = 0; i < manager.Controllers.Count; i++)
                    {
                        if (manager.Controllers[i] == null)
                        {
                            manager.Controllers[i] = controller;
                            added = true;
                            break;
                        }
                    }

                    //add to end of list then
                    if (!added)
                    {
                        manager.Controllers.Add(controller);
                        Controls.Types = manager.Controllers;
                    }

                    EditorUtility.SetDirty(manager);
                    EditorGUIUtility.PingObject(controller);
                }
            }
        }
    }
}
