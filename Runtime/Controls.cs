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
        public static List<ControllerType> Types { get; set; } = new List<ControllerType>();

        public static string GetAxisName(int joyStick, int axisNumber)
        {
            return "joystick " + (joyStick + 1) + " axis " + axisNumber;
        }

        public static string GetButtonName(int joyStick, int buttonNumber)
        {
            return "joystick " + (joyStick + 1) + " button " + buttonNumber;
        }

        /// <summary>
        /// Returns whether a bind is a keyboard bind
        /// </summary>
        /// <param name="bind"></param>
        /// <returns></returns>
        public static bool IsKeyboard(string bind)
        {
            string type = GetBindType(bind);
            if (type.IndexOf("keyboard") != -1) return true;
            if (type.IndexOf("number button") != -1) return true;
            if (type.IndexOf("number button") != -1) return true;
            if (type.IndexOf("mouse button") != -1) return true;
            if (type.IndexOf("modifier") != -1) return true;
            if (type.IndexOf("special") != -1) return true;
            if (type.IndexOf("arrow key") != -1) return true;
            if (type.IndexOf("function key") != -1) return true;

            return false;
        }

        /// <summary>
        /// Returns the type of bind (keyboard, controller, mouse, etc)
        /// </summary>
        /// <param name="bind"></param>
        /// <returns></returns>
        public static string GetBindType(string bind)
        {
            //check if its a hard wired button
            if (bind == "Left Bumper" || bind == "Right Bumper" || bind == "Start" || bind == "Select")
            {
                return "<color=blue>" + bind + "</color>";
            }

            //check if its this is a bind on a controller
            foreach (ControllerType type in Types)
            {
                foreach (ControllerBind button in type.buttons)
                {
                    if (button.name == bind)
                    {
                        return "<color=grey>ok</color>";
                    }
                }
            }

            //keyboard
            bool r = false;
            if (bind == "a") r = true;
            if (bind == "b") r = true;
            if (bind == "c") r = true;
            if (bind == "d") r = true;
            if (bind == "e") r = true;
            if (bind == "f") r = true;
            if (bind == "g") r = true;
            if (bind == "h") r = true;
            if (bind == "i") r = true;
            if (bind == "j") r = true;
            if (bind == "k") r = true;
            if (bind == "l") r = true;
            if (bind == "m") r = true;
            if (bind == "n") r = true;
            if (bind == "o") r = true;
            if (bind == "p") r = true;
            if (bind == "q") r = true;
            if (bind == "r") r = true;
            if (bind == "s") r = true;
            if (bind == "t") r = true;
            if (bind == "u") r = true;
            if (bind == "v") r = true;
            if (bind == "w") r = true;
            if (bind == "x") r = true;
            if (bind == "y") r = true;
            if (bind == "z") r = true;
            if (r)
            {
                return "<color=green>keyboard</color>";
            }

            //number keys
            if (bind == "0") r = true;
            if (bind == "1") r = true;
            if (bind == "2") r = true;
            if (bind == "3") r = true;
            if (bind == "4") r = true;
            if (bind == "5") r = true;
            if (bind == "6") r = true;
            if (bind == "7") r = true;
            if (bind == "8") r = true;
            if (bind == "9") r = true;
            if (r)
            {
                return "<color=green>number button</color>";
            }

            //mouse buttons
            if (bind == "mouse 0") r = true;
            if (bind == "mouse 1") r = true;
            if (bind == "mouse 2") r = true;
            if (bind == "mouse 3") r = true;
            if (bind == "mouse 4") r = true;
            if (bind == "mouse 5") r = true;
            if (bind == "mouse 6") r = true;
            if (r)
            {
                return "<color=green>mouse button</color>";
            }

            //modifier keys
            if (bind == "left shift") r = true;
            if (bind == "right shift") r = true;
            if (bind == "left ctrl") r = true;
            if (bind == "right ctrl") r = true;
            if (bind == "right alt") r = true;
            if (bind == "left alt") r = true;
            if (bind == "right cmd") r = true;
            if (bind == "left cmd") r = true;
            if (r)
            {
                return "<color=green>modifier</color>";
            }

            //special keys
            if (bind == "backspace") r = true;
            if (bind == "tab") r = true;
            if (bind == "return") r = true;
            if (bind == "escape") r = true;
            if (bind == "space") r = true;
            if (bind == "delete") r = true;
            if (bind == "enter") r = true;
            if (bind == "insert") r = true;
            if (bind == "home") r = true;
            if (bind == "end") r = true;
            if (bind == "page up") r = true;
            if (bind == "page down") r = true;
            if (r)
            {
                return "<color=green>special</color>";
            }

            //arrow keys
            if (bind == "up") r = true;
            if (bind == "down") r = true;
            if (bind == "left") r = true;
            if (bind == "right") r = true;
            if (r)
            {
                return "<color=green>arrow key</color>";
            }

            //function keys
            if (bind.Length == 2 && bind[0] == 'f') r = true;
            if (r)
            {
                return "<color=green>function key</color>";
            }

            return "<color=orange>unknown</color>";
        }

        public static Vector2 GetLeftThumb(int joyStick = 0)
        {
            if (joyStick == -1 || joyStick >= MaxControllers)
            {
                throw new Exception("JoyStick number " + joyStick + " is out of range.");
            }
            if (Controllers.Count <= joyStick) return Vector2.zero;

            return Controllers[joyStick].LeftThumb;
        }

        public static Vector2 GetRightThumb(int joyStick = 0)
        {
            if (joyStick == -1 || joyStick >= MaxControllers)
            {
                throw new Exception("JoyStick number " + joyStick + " is out of range.");
            }
            if (Controllers.Count <= joyStick) return Vector2.zero;

            return Controllers[joyStick].RightThumb;
        }

        public static bool GetLeftThumbDown(int joyStick = 0)
        {
            if (joyStick == -1 || joyStick >= MaxControllers)
            {
                throw new Exception("JoyStick number " + joyStick + " is out of range.");
            }
            if (Controllers.Count <= joyStick) return false;

            return Controllers[joyStick].LeftThumbDown;
        }

        public static bool GetRightThumbDown(int joyStick = 0)
        {
            if (joyStick == -1 || joyStick >= MaxControllers)
            {
                throw new Exception("JoyStick number " + joyStick + " is out of range.");
            }
            if (Controllers.Count <= joyStick) return false;

            return Controllers[joyStick].RightThumbDown;
        }

        public static bool GetStart(int joyStick = 0)
        {
            if (joyStick == -1 || joyStick >= MaxControllers)
            {
                throw new Exception("JoyStick number " + joyStick + " is out of range.");
            }
            if (Controllers.Count <= joyStick) return false;

            return Controllers[joyStick].Start;
        }

        public static bool GetSelect(int joyStick = 0)
        {
            if (joyStick == -1 || joyStick >= MaxControllers)
            {
                throw new Exception("JoyStick number " + joyStick + " is out of range.");
            }
            if (Controllers.Count <= joyStick) return false;

            return Controllers[joyStick].Select;
        }

        public static bool GetLeftBumper(int joyStick = 0)
        {
            if (joyStick == -1 || joyStick >= MaxControllers)
            {
                throw new Exception("JoyStick number " + joyStick + " is out of range.");
            }
            if (Controllers.Count <= joyStick) return false;

            return Controllers[joyStick].LeftBumper;
        }

        public static bool GetRightBumper(int joyStick = 0)
        {
            if (joyStick == -1 || joyStick >= MaxControllers)
            {
                throw new Exception("JoyStick number " + joyStick + " is out of range.");
            }
            if (Controllers.Count <= joyStick) return false;

            return Controllers[joyStick].RightBumper;
        }

        public static float GetLeftTrigger(int joyStick = 0)
        {
            if (joyStick == -1 || joyStick >= MaxControllers)
            {
                throw new Exception("JoyStick number " + joyStick + " is out of range.");
            }
            if (Controllers.Count <= joyStick) return 0f;

            return Controllers[joyStick].LeftTrigger;
        }

        public static float GetRightTrigger(int joyStick = 0)
        {
            if (joyStick == -1 || joyStick >= MaxControllers)
            {
                throw new Exception("JoyStick number " + joyStick + " is out of range.");
            }
            if (Controllers.Count <= joyStick) return 0f;

            return Controllers[joyStick].RightTrigger;
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