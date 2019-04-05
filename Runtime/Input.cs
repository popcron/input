using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Input : UnityEngine.Input
{
    public static List<KeyCode> keys = new List<KeyCode>();
    public static List<KeyCode> keysPressed = new List<KeyCode>();
    public static List<KeyCode> keysReleased = new List<KeyCode>();

    public static new float GetAxis(string axisName)
    {
        return UnityEngine.Input.GetAxis(axisName);
    }

    public static new float GetAxisRaw(string axisName)
    {
        return UnityEngine.Input.GetAxisRaw(axisName);
    }

    public static new bool GetButton(string buttonName)
    {
        return UnityEngine.Input.GetButton(buttonName);
    }

    public static new bool GetButtonDown(string buttonName)
    {
        return UnityEngine.Input.GetButtonDown(buttonName);
    }

    public static new bool GetButtonUp(string buttonName)
    {
        return UnityEngine.Input.GetButtonUp(buttonName);
    }

    public static new bool GetKey(KeyCode keyCode)
    {
#if UNITY_EDITOR
        return keys.Contains(keyCode);
#else
        return UnityEngine.Input.GetKey(keyCode);
#endif
    }

    public static new bool GetKeyDown(KeyCode keyCode)
    {
#if UNITY_EDITOR
        if (keysPressed.Contains(keyCode))
        {
            keysPressed.Remove(keyCode);
            return true;
        }

        return false;
#else
        return UnityEngine.Input.GetKeyDown(keyCode);
#endif
    }

    public static new bool GetKeyUp(KeyCode keyCode)
    {
#if UNITY_EDITOR
        if (keysReleased.Contains(keyCode))
        {
            keysReleased.Remove(keyCode);
            return true;
        }

        return false;
#else
        return UnityEngine.Input.GetKeyUp(keyCode);
#endif
    }
}