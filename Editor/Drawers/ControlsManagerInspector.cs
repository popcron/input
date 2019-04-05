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
        private const string ShowKey = "Popcron.Input.ShowControllers";

        private ControlsManager manager;
        private List<bool> show = new List<bool>();

        private void OnEnable()
        {
            manager = target as ControlsManager;

            string[] data = EditorPrefs.GetString(ShowKey).Split(',');
            show.Clear();
            foreach (string element in data)
            {
                show.Add(element == "true");
            }
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
                //resize the show list if the controller count changed
                if (show.Count != Controls.Controllers.Count)
                {
                    if (show.Count > Controls.Controllers.Count)
                    {
                        while (show.Count > Controls.Controllers.Count)
                        {
                            show.Add(true);
                        }
                    }
                    else
                    {
                        while (show.Count < Controls.Controllers.Count)
                        {
                            show.Add(false);
                        }
                    }
                }

                EditorGUILayout.LabelField("Connected Controllers: ");
                EditorGUI.indentLevel++;
                for (int j = 0; j < Controls.Controllers.Count; j++)
                {
                    bool newShow = EditorGUILayout.Foldout(show[j], Controls.Controllers[j].Name);
                    if (newShow != show[j])
                    {
                        show[j] = newShow;
                        EditorPrefs.SetString(ShowKey, string.Join(",", show));
                    }

                    if (newShow)
                    {
                        EditorGUI.indentLevel++;

                        //show axes
                        for (int a = 0; a < Controls.MaxJoystickAxes; a++)
                        {
                            string axisName = Controls.GetAxisName(j, a);
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
            if (!manager.Map)
            {
                EditorGUILayout.HelpBox("No input Map being used.", MessageType.Error);

                //create a new map
                if (GUILayout.Button("Create new map"))
                {
                    Map map = CreateInstance<Map>();
                    map.name = "New controller";

                    string path = Path.Combine("Assets", "New input map.asset");
                    AssetDatabase.CreateAsset(map, path);

                    manager.Map = map;
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