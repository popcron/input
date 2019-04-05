using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Popcron.Input
{
    using Input = UnityEngine.Input;

    [Serializable]
    public class Controller
    {
        [SerializeField]
        private ControllerType type;

        [SerializeField]
        private int joyStick;

        private List<string> pressedAxes = new List<string>();
        private List<string> releasedAxes = new List<string>();

        public string Name
        {
            get
            {
                return type.controllerName;
            }
        }

        public Controller(ControllerType type, int joyStick)
        {
            this.type = type;
            this.joyStick = joyStick;
        }

        public Vector2 GetLeftThumb()
        {
            string horizontalAxisName = Controls.GetAxisName(joyStick, type.leftThumb.horizontalAxis);
            string verticalAxisName = Controls.GetAxisName(joyStick, type.leftThumb.verticalAxis);

            float horizontal = Input.GetAxisRaw(horizontalAxisName);
            float vertical = Input.GetAxisRaw(verticalAxisName);

            return new Vector2(horizontal * (type.leftThumb.invertHorizontal ? -1f : 1f), vertical * (type.leftThumb.invertVertical ? -1f : 1f));
        }

        public Vector2 GetRightThumb()
        {
            string horizontalAxisName = Controls.GetAxisName(joyStick, type.rightThumb.horizontalAxis);
            string verticalAxisName = Controls.GetAxisName(joyStick, type.rightThumb.verticalAxis);

            float horizontal = Input.GetAxisRaw(horizontalAxisName);
            float vertical = Input.GetAxisRaw(verticalAxisName);

            return new Vector2(horizontal * (type.rightThumb.invertHorizontal ? -1f : 1f), vertical * (type.rightThumb.invertVertical ? -1f : 1f));
        }

        public bool GetLeftThumbDown()
        {
            if (!type.leftThumb.button.isKey)
            {
                return type.leftThumb.button.Evaluate(joyStick);
            }
            else
            {
                string buttonName = Controls.GetButtonName(joyStick, type.leftThumb.button.buttonNumber);
                return Input.GetKey(buttonName);
            }
        }

        public bool GetRightThumbDown()
        {
            if (!type.rightThumb.button.isKey)
            {
                return type.rightThumb.button.Evaluate(joyStick);
            }
            else
            {
                string buttonName = Controls.GetButtonName(joyStick, type.rightThumb.button.buttonNumber);
                return Input.GetKey(buttonName);
            }
        }

        public bool GetStart()
        {
            if (!type.start.isKey)
            {
                return type.start.Evaluate(joyStick);
            }
            else
            {
                string buttonName = Controls.GetButtonName(joyStick, type.start.buttonNumber);
                return Input.GetKey(buttonName);
            }
        }

        public bool GetSelect()
        {
            if (!type.select.isKey)
            {
                return type.select.Evaluate(joyStick);
            }
            else
            {
                string buttonName = Controls.GetButtonName(joyStick, type.select.buttonNumber);
                return Input.GetKey(buttonName);
            }
        }

        public bool GetLeftBumper()
        {
            if (!type.leftBumper.isKey)
            {
                return type.leftBumper.Evaluate(joyStick);
            }
            else
            {
                string buttonName = Controls.GetButtonName(joyStick, type.leftBumper.buttonNumber);
                return Input.GetKey(buttonName);
            }
        }

        public bool GetRightBumper()
        {
            if (!type.rightBumper.isKey)
            {
                return type.rightBumper.Evaluate(joyStick);
            }
            else
            {
                string buttonName = Controls.GetButtonName(joyStick, type.rightBumper.buttonNumber);
                return Input.GetKey(buttonName);
            }
        }

        public float GetLeftTrigger()
        {
            string axisName = Controls.GetAxisName(joyStick, type.leftTriggerAxis);
            return Input.GetAxisRaw(axisName);
        }

        public float GetRightTrigger()
        {
            string axisName = Controls.GetAxisName(joyStick, type.rightTriggerAxis);
            return Input.GetAxisRaw(axisName);
        }

        public ControllerBind GetBind(string name)
        {
            for (int i = 0; i < type.buttons.Count; i++)
            {
                if (type.buttons[i].name == name)
                {
                    return type.buttons[i];
                }
            }

            if (name == "Left Bumper")
            {
                return type.leftBumper;
            }
            else if (name == "Right Bumper")
            {
                return type.rightBumper;
            }
            else if (name == "Start")
            {
                return type.start;
            }
            else if (name == "Select")
            {
                return type.select;
            }

            return null;
        }

        public bool GetButton(string name)
        {
            ControllerBind bind = GetBind(name);
            if (bind == null)
            {
                return false;
            }

            if (!bind.isKey)
            {
                return bind.Evaluate(joyStick);
            }
            else
            {
                string buttonName = Controls.GetButtonName(joyStick, bind.buttonNumber);
                return Input.GetKey(buttonName);
            }
        }

        public bool GetButtonDown(string name)
        {
            ControllerBind bind = GetBind(name);
            if (bind == null)
            {
                return false;
            }

            if (!bind.isKey)
            {
                string axisName = Controls.GetAxisName(joyStick, bind.axisNumber);
                for (int i = 0; i < pressedAxes.Count; i++)
                {
                    if (pressedAxes[i] == axisName + "." + bind.axisDirection)
                    {
                        return true;
                    }
                }

                return false;
            }
            else
            {
                string buttonName = Controls.GetButtonName(joyStick, bind.buttonNumber);
                return Input.GetKeyDown(buttonName);
            }
        }

        public bool GetButtonUp(string name)
        {
            ControllerBind bind = GetBind(name);
            if (bind == null)
            {
                return false;
            }

            if (!bind.isKey)
            {
                string axisName = Controls.GetAxisName(joyStick, bind.axisNumber);
                for (int i = 0; i < releasedAxes.Count; i++)
                {
                    if (releasedAxes[i] == axisName)
                    {
                        return true;
                    }
                }

                return false;
            }
            else
            {
                string buttonName = Controls.GetButtonName(joyStick, bind.buttonNumber);
                return Input.GetKeyUp(buttonName);
            }
        }

        /// <summary>
        /// Returns wether this control name exists on this controller
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Exists(string name)
        {
            for (int i = 0; i < type.buttons.Count; i++)
            {
                if (type.buttons[i].name == name)
                {
                    return true;
                }
            }

            if (name == "Left Bumper") return true;
            else if (name == "Right Bumper") return true;
            else if (name == "Start") return true;
            else if (name == "Select") return true;

            return false;
        }

        public void PressAxis(string name)
        {
            if (pressedAxes.Contains(name)) return;

            pressedAxes.Add(name);
        }

        public void ReleaseAxis(string name)
        {
            if (releasedAxes.Contains(name)) return;

            releasedAxes.Add(name);
        }

        public void ClearAxes()
        {
            pressedAxes.Clear();
            releasedAxes.Clear();
        }
    }
}