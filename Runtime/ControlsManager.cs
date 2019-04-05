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

        [SerializeField]
        private Map map;

        [SerializeField]
        private List<ControllerType> allControllers = new List<ControllerType>();

        private List<Controller> controllers = new List<Controller>();
        private float[] lastAxis = new float[5 * 28];

        /// <summary>
        /// The current input map being used
        /// </summary>
        public Map Map
        {
            get
            {
                return map;
            }
            set
            {
                map = value;
            }
        }

        /// <summary>
        /// List of all controllers available in the project
        /// </summary>
        public List<ControllerType> Controllers => allControllers;

        private void Awake()
        {
            instance = this;
        }

        private void OnEnable()
        {
            instance = this;
        }

        private ControllerType GetController(string joystickName)
        {
            for (int i = 0; i < allControllers.Count; i++)
            {
                string a = allControllers[i].controllerName.ToLower().Replace(" ", "");
                string b = joystickName.ToLower().Replace(" ", "");

                if (a.Equals(b))
                {
                    return allControllers[i];
                }
            }

            return null;
        }

        private void Update()
        {
            //set static value
            Controls.Controllers = controllers;
            string[] joysticks = Input.GetJoystickNames();
            if (joysticks.Length > 0)
            {
                //load controllers
                controllers.Clear();
                int index = 0;
                for (int j = 0; j < joysticks.Length; j++)
                {
                    ControllerType type = GetController(joysticks[j]);
                    Controller controller = new Controller(type, j);
                    controllers.Add(controller);

                    for (int b = 0; b < type.buttons.Count; b++)
                    {
                        ControllerBind bind = type.buttons[b];
                        if (!bind.isKey)
                        {
                            string axisName = Controls.GetAxisName(j, bind.axisNumber);
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
                            }
                        }
                        index++;
                    }
                }
            }
            else
            {
                controllers.Clear();
            }
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

            Controller controller = instance.controllers[joyStick];
            for (int b = 0; b < bind.binds.Count; b++)
            {
                //controller check
                if (controller.GetButton(bind.binds[b]))
                {
                    return true;
                }

                //keyboard check
                if (!controller.Exists(bind.binds[b]))
                {
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

            Controller controller = instance.controllers[joyStick];
            for (int b = 0; b < bind.binds.Count; b++)
            {
                //controller check
                if (controller.GetButtonDown(bind.binds[b]))
                {
                    return true;
                }

                //keyboard check
                if (!controller.Exists(bind.binds[b]))
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

            Controller controller = instance.controllers[joyStick];
            for (int b = 0; b < bind.binds.Count; b++)
            {
                //controller check
                if (controller.GetButtonUp(bind.binds[b]))
                {
                    return true;
                }

                //keyboard check
                if (!controller.Exists(bind.binds[b]))
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