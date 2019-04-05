using System;
using UnityEngine;

namespace Popcron.Input
{
    using Input = UnityEngine.Input;

    [Serializable]
    public class ControllerBind
    {
        public string name;
        public bool isKey;
        public int buttonNumber;
        public int axisNumber;
        public ControllerAxisDirection axisDirection;

        public ControllerBind(string name, KeyCode key)
        {
            this.isKey = true;
            this.name = name;

            if (key == KeyCode.JoystickButton0) buttonNumber = 0;
            if (key == KeyCode.JoystickButton1) buttonNumber = 1;
            if (key == KeyCode.JoystickButton2) buttonNumber = 2;
            if (key == KeyCode.JoystickButton3) buttonNumber = 3;
            if (key == KeyCode.JoystickButton4) buttonNumber = 4;
            if (key == KeyCode.JoystickButton5) buttonNumber = 5;
            if (key == KeyCode.JoystickButton6) buttonNumber = 6;
            if (key == KeyCode.JoystickButton7) buttonNumber = 7;
            if (key == KeyCode.JoystickButton8) buttonNumber = 8;
            if (key == KeyCode.JoystickButton9) buttonNumber = 9;
            if (key == KeyCode.JoystickButton10) buttonNumber = 10;
            if (key == KeyCode.JoystickButton11) buttonNumber = 11;
            if (key == KeyCode.JoystickButton12) buttonNumber = 12;
            if (key == KeyCode.JoystickButton13) buttonNumber = 13;
            if (key == KeyCode.JoystickButton14) buttonNumber = 14;
            if (key == KeyCode.JoystickButton15) buttonNumber = 15;
            if (key == KeyCode.JoystickButton16) buttonNumber = 16;
            if (key == KeyCode.JoystickButton17) buttonNumber = 17;
            if (key == KeyCode.JoystickButton18) buttonNumber = 18;
            if (key == KeyCode.JoystickButton19) buttonNumber = 19;
        }

        public ControllerBind(string name, int axisNumber, ControllerAxisDirection axisDirection)
        {
            this.isKey = false;
            this.name = name;

            this.axisNumber = axisNumber;
            this.axisDirection = axisDirection;
        }

        public bool Evaluate(int joyStick)
        {
            string axisName = Controls.GetAxisName(joyStick, axisNumber);
            float axisValue = Input.GetAxisRaw(axisName);
            switch (axisDirection)
            {
                case ControllerAxisDirection.Both:
                    return Mathf.Abs(axisValue) < 0.2f;
                case ControllerAxisDirection.Negative:
                    return axisValue < -0.2f;
                case ControllerAxisDirection.Positive:
                    return axisValue > 0.2f;
            }

            return false;
        }
    }
}