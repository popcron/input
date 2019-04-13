using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popcron.Input
{
    using Input = global::Input;

    [CreateAssetMenu(menuName = "Controls/Controller")]
    public class ControllerType : ScriptableObject
    {
        public List<string> controllerNames = new List<string>();
        public ControllerPlatform platform = ControllerPlatform.Windows;

        public ControllerThumb leftThumb = new ControllerThumb(7, 0, 1, false, true);
        public ControllerThumb rightThumb = new ControllerThumb(8, 3, 4, false, true);

        public ControllerBind leftBumper = new ControllerBind("Left Bumper", KeyCode.JoystickButton4);
        public ControllerBind rightBumper = new ControllerBind("Right Bumper", KeyCode.JoystickButton5);
        public int selectButton = 6;
        public int startButton = 7;
        public int leftTriggerAxis = 8;
        public int rightTriggerAxis = 9;
        public List<ControllerBind> buttons = new List<ControllerBind>
        {
            new ControllerBind("DPad Right", 5, ControllerAxisDirection.Positive),
            new ControllerBind("DPad Left", 5, ControllerAxisDirection.Negative),
            new ControllerBind("DPad Down", 6, ControllerAxisDirection.Negative),
            new ControllerBind("DPad Up", 6, ControllerAxisDirection.Positive),
            new ControllerBind("Button A", KeyCode.JoystickButton0),
            new ControllerBind("Button B", KeyCode.JoystickButton1),
            new ControllerBind("Button X", KeyCode.JoystickButton2),
            new ControllerBind("Button Y", KeyCode.JoystickButton3)
        };

        public bool GetButton(ControllerBind bind, int joyStick)
        {
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

        public ControllerBind GetBind(string name)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                if (buttons[i].name == name)
                {
                    return buttons[i];
                }
            }

            if (name == "Left Bumper")
            {
                return leftBumper;
            }
            else if (name == "Right Bumper")
            {
                return rightBumper;
            }

            return null;
        }
    }
}