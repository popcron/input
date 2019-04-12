using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popcron.Input
{
    using Input = UnityEngine.Input;

    [AddComponentMenu("Popcron/Input/Controls manager")]
    public class ControlsManager : MonoBehaviour
    {
        private static ControlsManager instance;

        public Map map;
        public ControllerType defaultController;

        [SerializeField]
        private List<ControllerType> allControllers = new List<ControllerType>();

        [SerializeField]
        private string[] joysticks;

        private List<Controller> controllers = new List<Controller>();
        private float[] lastAxis = new float[6 * 30];

        /// <summary>
        /// The current input map being used
        /// </summary>
        public static Map Map { get; private set; }
        public static ControllerType DefaultController { get; private set; }

        /// <summary>
        /// List of all controllers available in the project
        /// </summary>
        public List<ControllerType> Controllers => allControllers;

        private void Awake()
        {
            instance = this;
        }

        private void Reset()
        {
            CheckInstances();
        }

        private void CheckInstances()
        {
#if UNITY_EDITOR
            ControlsManager otherManager = null;
            ControlsManager[] managers = FindObjectsOfType<ControlsManager>();
            for (int i = 0; i < managers.Length; i++)
            {
                if (managers[i] != this)
                {
                    otherManager = managers[i];
                    break;
                }
            }

            if (otherManager)
            {
                Debug.Log("Another Controls manager instance already exists!");
                UnityEditor.EditorGUIUtility.PingObject(otherManager);
                DestroyImmediate(this);
            }
#endif
        }

        private void OnEnable()
        {
            Map = map;
            DefaultController = defaultController;
            instance = this;
        }

        private ControllerType GetController(string joystickName)
        {
            if (string.IsNullOrEmpty(joystickName)) return null;

            string b = joystickName.ToLower().Replace(" ", "");
            for (int i = 0; i < allControllers.Count; i++)
            {
                if (allControllers[i] == defaultController) continue;

                string a = allControllers[i].controllerName.ToLower().Replace(" ", "");
                if (a.Equals(b))
                {
                    return allControllers[i];
                }
            }

            return null;
        }

        private void Update()
        {
            Map = map;
            DefaultController = defaultController;

            //set static value
            Controls.Controllers = controllers;
            joysticks = Input.GetJoystickNames();
            if (joysticks.Length > 0)
            {
                //load controllers
                controllers.Clear();
                int index = 0;
                int joyStick = 0;
                for (int j = 0; j < joysticks.Length; j++)
                {
                    ControllerType type = GetController(joysticks[j]);
                    if (type != null)
                    {
                        Controller controller = new Controller(type, j);
                        controllers.Add(controller);
                        joyStick++;

                        for (int b = 0; b < type.buttons.Count; b++)
                        {
                            ControllerBind bind = type.buttons[b];
                            if (!bind.isKey)
                            {
                                string axisName = Controls.GetAxisName(controller.JoyStick, bind.axisNumber);
                                float axisValue = Input.GetAxisRaw(axisName);
                                if (axisValue != lastAxis[index])
                                {
                                    if (Mathf.Abs(axisValue) < 0.2f)
                                    {
                                        controller.ReleaseAxis(axisName);
                                    }
                                    else
                                    {
                                        if (bind.axisDirection == ControllerAxisDirection.Both)
                                        {
                                            if (Mathf.Abs(axisValue) > 0.2f)
                                            {
                                                axisName += ".Both";
                                                controller.PressAxis(axisName);
                                            }
                                        }
                                        else if (bind.axisDirection == ControllerAxisDirection.Negative)
                                        {
                                            if (axisValue < -0.2f)
                                            {
                                                axisName += ".Negative";
                                                controller.PressAxis(axisName);
                                            }
                                        }
                                        else if (bind.axisDirection == ControllerAxisDirection.Positive)
                                        {
                                            if (axisValue > 0.2f)
                                            {
                                                axisName += ".Positive";
                                                controller.PressAxis(axisName);
                                            }
                                        }
                                    }

                                    lastAxis[index] = axisValue;
                                    index++;
                                }
                            }
                        }
                    }
                }

                //if no controllers, add default
                if (controllers.Count == 0)
                {
                    Controller controller = new Controller(defaultController, 0);
                    controllers.Add(controller);
                }
            }
            else
            {
                controllers.Clear();
            }
        }

        private static List<Controller> GetControllers(int joyStick)
        {
            if (joyStick == -1)
            {
                return instance.controllers;
            }

            List<Controller> c = new List<Controller>();
            List<Controller> defaults = new List<Controller>();
            foreach (Controller controller in instance.controllers)
            {
                if (controller.JoyStick == joyStick)
                {
                    if (controller.Type == instance.defaultController)
                    {
                        defaults.Add(controller);
                    }
                    else
                    {
                        c.Add(controller);
                    }
                }
            }

            //add default controllers as last
            c.AddRange(defaults);

            return c;
        }

        public static bool GetButton(string name, int joyStick)
        {
            if (!instance) instance = FindObjectOfType<ControlsManager>();

            MapBind bind = instance.map.GetBind(name);
            if (bind == null)
            {
                throw new Exception("Bind for " + name + " doesn't exists in the map.");
            }

            if (instance.controllers.Count == 0) return false;
            if (joyStick < 0 || joyStick >= Controls.MaxControllers)
            {
                throw new Exception("JoyStick number " + joyStick + " is out of range.");
            }
            if (instance.controllers.Count <= joyStick) return false;

            Controller controller = instance.controllers[joyStick];
            for (int b = 0; b < bind.binds.Count; b++)
            {
                //controller check
                if (controller.GetButton(bind.binds[b]))
                {
                    return true;
                }

                //for interference checking
                if (!controller.Contains(bind.binds[b]))
                {
                    //keyboard check
                    if (Input.GetKey(bind.binds[b]))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool GetButtonDown(string name, int joyStick)
        {
            if (!instance) instance = FindObjectOfType<ControlsManager>();

            MapBind bind = instance.map.GetBind(name);
            if (bind == null)
            {
                throw new Exception("Bind for " + name + " doesn't exists in the map.");
            }

            if (instance.controllers.Count == 0) return false;
            if (joyStick < 0 || joyStick >= Controls.MaxControllers)
            {
                throw new Exception("JoyStick number " + joyStick + " is out of range.");
            }
            if (instance.controllers.Count <= joyStick) return false;

            Controller controller = instance.controllers[joyStick];
            for (int b = 0; b < bind.binds.Count; b++)
            {
                if (controller.GetButtonDown(bind.binds[b]))
                {
                    return true;
                }

                if (!controller.Contains(bind.binds[b]))
                {
                    if (Input.GetKeyDown(bind.binds[b]))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool GetButtonUp(string name, int joyStick)
        {
            if (!instance) instance = FindObjectOfType<ControlsManager>();

            MapBind bind = instance.map.GetBind(name);
            if (bind == null)
            {
                throw new Exception("Bind for " + name + " doesn't exists in the map.");
            }

            if (instance.controllers.Count == 0) return false;
            if (joyStick < 0 || joyStick >= Controls.MaxControllers)
            {
                throw new Exception("JoyStick number " + joyStick + " is out of range.");
            }
            if (instance.controllers.Count <= joyStick) return false;

            Controller controller = instance.controllers[joyStick];
            for (int b = 0; b < bind.binds.Count; b++)
            {
                if (controller.GetButtonUp(bind.binds[b]))
                {
                    return true;
                }

                if (!controller.Contains(bind.binds[b]))
                {
                    if (Input.GetKeyUp(bind.binds[b]))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
