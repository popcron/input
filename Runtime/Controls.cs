using UnityEngine;
using System;
using System.Collections.Generic;

namespace Popcron.Input
{
    public sealed partial class Controls
    {
        /// <summary>
        /// List of all the plugged in controller instances
        /// </summary>
        public static List<Controller> Controllers { get; set; } = new List<Controller>();

        public static string GetAxisName(int joyStick, int axisNumber)
        {
            return "joystick " + (joyStick + 1) + " axis " + axisNumber;
        }

        public static string GetButtonName(int joyStick, int buttonNumber)
        {
            return "joystick " + (joyStick + 1) + " button " + buttonNumber;
        }

        public static Vector2 GetLeftThumb(int joyStick = 0)
        {
            return Controllers[joyStick].GetLeftThumb();
        }

        public static Vector2 GetRightThumb(int joyStick = 0)
        {
            return Controllers[joyStick].GetRightThumb();
        }

        public static bool GetLeftThumbDown(int joyStick = 0)
        {
            return Controllers[joyStick].GetLeftThumbDown();
        }

        public static bool GetRightThumbDown(int joyStick = 0)
        {
            return Controllers[joyStick].GetRightThumbDown();
        }

        public static bool GetStart(int joyStick = 0)
        {
            return Controllers[joyStick].GetStart();
        }

        public static bool GetSelect(int joyStick = 0)
        {
            return Controllers[joyStick].GetSelect();
        }

        public static bool GetLeftBumper(int joyStick = 0)
        {
            return Controllers[joyStick].GetLeftBumper();
        }

        public static bool GetRightBumper(int joyStick = 0)
        {
            return Controllers[joyStick].GetRightBumper();
        }

        public static float GetLeftTrigger(int joyStick = 0)
        {
            return Controllers[joyStick].GetLeftTrigger();
        }

        public static float GetRightTrigger(int joyStick = 0)
        {
            return Controllers[joyStick].GetRightTrigger();
        }

        public static bool GetButton(string name, int joyStick = 0)
        {
            return ControlsManager.GetButton(name, joyStick);
        }

        public static bool GetButtonDown(string name, int joyStick = 0)
        {
            return ControlsManager.GetButtonDown(name, joyStick);
        }

        public static bool GetButtonUp(string name, int joyStick = 0)
        {
            return ControlsManager.GetButtonUp(name, joyStick);
        }
    }
}