using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityInput = UnityEngine.Input;

public class Input : UnityInput
{
#if UNITY_EDITOR
    public static List<KeyCode> keys = new List<KeyCode>();
    public static List<KeyCode> keysPressed = new List<KeyCode>();
    public static List<KeyCode> keysReleased = new List<KeyCode>();
#endif

    private static List<KeyCode> consumedKeys = new List<KeyCode>();

    private static bool IsOnWindows
    {
        get
        {
            //should be the same as EditorHotkeyIntercepts.IsOnWindows
            Version version = Environment.OSVersion.Version;
            if (version.Major == 5)
            {
                if (version.Minor >= 0 && version.Minor <= 2)
                {
                    return true;
                }
            }
            else if (version.Major == 6)
            {
                if (version.Minor >= 0 && version.Minor <= 3)
                {
                    return true;
                }
            }
            else if (version.Major == 10)
            {
                return true;
            }

            return false;
        }
    }

    /// <summary>
    /// Disable or enable input commands
    /// <summary>
    public static bool Enabled { get; set; } = true;

    public static new float GetAxis(string axisName)
    {
        if (!Enabled)
        {
            return 0f;
        }

        return UnityInput.GetAxis(axisName);
    }

    public static new float GetAxisRaw(string axisName)
    {
        if (!Enabled)
        {
            return 0f;
        }

        return UnityInput.GetAxisRaw(axisName);
    }

    public static new bool GetButton(string buttonName)
    {
        if (!Enabled)
        {
            return false;
        }

        return UnityInput.GetButton(buttonName);
    }

    public static new bool GetButtonDown(string buttonName)
    {
        if (!Enabled)
        {
            return false;
        }

        return UnityInput.GetButtonDown(buttonName);
    }

    public static new bool GetButtonUp(string buttonName)
    {
        if (!Enabled)
        {
            return false;
        }

        return UnityInput.GetButtonUp(buttonName);
    }

    public static new bool GetKey(KeyCode keyCode)
    {
        if (!Enabled)
        {
            return false;
        }

#if UNITY_EDITOR
        if (IsOnWindows)
        {
            return keys.Contains(keyCode);
        }
#endif
        return UnityInput.GetKey(keyCode);
    }

    public static new bool GetKeyDown(KeyCode keyCode)
    {
        if (!Enabled)
        {
            return false;
        }

        if (consumedKeys.Contains(keyCode))
        {
            return false;
        }

#if UNITY_EDITOR
        if (IsOnWindows)
        {
            if (keysPressed.Contains(keyCode))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
#endif
        return UnityInput.GetKeyDown(keyCode);
    }

    /// <summary>
    /// Consumes this key for one frame so that GetKeyDown events will not be registered again with this key code.
    /// </summary>
    public static async void ConsumeKey(KeyCode keyCode)
    {
        if (!Enabled)
        {
            return;
        }

        consumedKeys.Add(keyCode);
        int thisFrame = Time.frameCount;
        while (thisFrame == Time.frameCount)
        {
            await Task.Delay(1);
        }
        consumedKeys.Remove(keyCode);
    }

    public static new bool GetKeyUp(KeyCode keyCode)
    {
        if (!Enabled)
        {
            return false;
        }

#if UNITY_EDITOR
        if (IsOnWindows)
        {
            if (keysReleased.Contains(keyCode))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
#endif
        return UnityInput.GetKeyUp(keyCode);
    }
}