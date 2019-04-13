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
        private ControllerType _type;

        [SerializeField]
        private int _joyStick;

        private List<string> pressedAxes = new List<string>();
        private List<string> releasedAxes = new List<string>();

        public int JoyStick => _joyStick;
        public string Name => _type.name;
        public ControllerType Type => _type;

        public Controller(ControllerType type, int joyStick)
        {
            _type = type;
            _joyStick = joyStick;
        }

        public Vector2 LeftThumb
        {
            get
            {
                string horizontalAxisName = Controls.GetAxisName(JoyStick, Type.leftThumb.horizontalAxis);
                string verticalAxisName = Controls.GetAxisName(JoyStick, Type.leftThumb.verticalAxis);

                float horizontal = Input.GetAxisRaw(horizontalAxisName);
                float vertical = Input.GetAxisRaw(verticalAxisName);

                return new Vector2(horizontal * (Type.leftThumb.invertHorizontal ? -1f : 1f), vertical * (Type.leftThumb.invertVertical ? -1f : 1f));
            }
        }

        public Vector2 RightThumb
        {
            get
            {
                string horizontalAxisName = Controls.GetAxisName(JoyStick, Type.rightThumb.horizontalAxis);
                string verticalAxisName = Controls.GetAxisName(JoyStick, Type.rightThumb.verticalAxis);

                float horizontal = Input.GetAxisRaw(horizontalAxisName);
                float vertical = Input.GetAxisRaw(verticalAxisName);

                return new Vector2(horizontal * (Type.rightThumb.invertHorizontal ? -1f : 1f), vertical * (Type.rightThumb.invertVertical ? -1f : 1f));
            }
        }

        public bool LeftThumbDown
        {
            get
            {
                string buttonName = Controls.GetButtonName(JoyStick, Type.leftThumb.buttonNumber);
                return Input.GetKey(buttonName);
            }
        }

        public bool RightThumbDown
        {
            get
            {
                string buttonName = Controls.GetButtonName(JoyStick, Type.rightThumb.buttonNumber);
                return Input.GetKey(buttonName);
            }
        }

        public bool Start
        {
            get
            {
                string buttonName = Controls.GetButtonName(JoyStick, Type.startButton);
                return Input.GetKey(buttonName);
            }
        }

        public bool StartDown
        {
            get
            {
                string buttonName = Controls.GetButtonName(JoyStick, Type.startButton);
                return Input.GetKeyDown(buttonName);
            }
        }

        public bool StartUp
        {
            get
            {
                string buttonName = Controls.GetButtonName(JoyStick, Type.startButton);
                return Input.GetKeyUp(buttonName);
            }
        }

        public bool Select
        {
            get
            {
                string buttonName = Controls.GetButtonName(JoyStick, Type.selectButton);
                return Input.GetKey(buttonName);
            }
        }

        public bool SelectDown
        {
            get
            {
                string buttonName = Controls.GetButtonName(JoyStick, Type.selectButton);
                return Input.GetKeyDown(buttonName);
            }
        }

        public bool SelectUp
        {
            get
            {
                string buttonName = Controls.GetButtonName(JoyStick, Type.selectButton);
                return Input.GetKeyUp(buttonName);
            }
        }

        public bool LeftBumper
        {
            get
            {
                if (!Type.leftBumper.isKey)
                {
                    return Type.leftBumper.Evaluate(JoyStick);
                }
                else
                {
                    string buttonName = Controls.GetButtonName(JoyStick, Type.leftBumper.buttonNumber);
                    return Input.GetKey(buttonName);
                }
            }
        }

        public bool RightBumper
        {
            get
            {
                if (!Type.rightBumper.isKey)
                {
                    return Type.rightBumper.Evaluate(JoyStick);
                }
                else
                {
                    string buttonName = Controls.GetButtonName(JoyStick, Type.rightBumper.buttonNumber);
                    return Input.GetKey(buttonName);
                }
            }
        }

        public float LeftTrigger
        {
            get
            {
                string axisName = Controls.GetAxisName(JoyStick, Type.leftTriggerAxis);
                return Input.GetAxisRaw(axisName);
            }
        }

        public float RightTrigger
        {
            get
            {
                string axisName = Controls.GetAxisName(JoyStick, Type.rightTriggerAxis);
                return Input.GetAxisRaw(axisName);
            }
        }

        public ControllerBind GetBind(string name)
        {
            return Type.GetBind(name);
        }

        public bool GetButton(string name)
        {
            ControllerBind bind = GetBind(name);
            return Type.GetButton(bind, JoyStick);
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
                string axisName = Controls.GetAxisName(JoyStick, bind.axisNumber);
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
                string buttonName = Controls.GetButtonName(JoyStick, bind.buttonNumber);
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
                string axisName = Controls.GetAxisName(JoyStick, bind.axisNumber);
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
                string buttonName = Controls.GetButtonName(JoyStick, bind.buttonNumber);
                return Input.GetKeyUp(buttonName);
            }
        }

        /// <summary>
        /// Returns wether this control name exists on this controller
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Contains(string name)
        {
            for (int i = 0; i < Type.buttons.Count; i++)
            {
                if (Type.buttons[i].name == name)
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
