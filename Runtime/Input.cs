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

	/// <summary>
	/// Disable or enable input commands
	/// <summary>
	public static bool Enabled { get; set; } = true;
		
    public static new float GetAxis(string axisName)
    {
		if (!Enabled) return 0f;
		
        return UnityEngine.Input.GetAxis(axisName);
    }

    public static new float GetAxisRaw(string axisName)
    {
		if (!Enabled) return 0f;
		
        return UnityEngine.Input.GetAxisRaw(axisName);
    }

    public static new bool GetButton(string buttonName)
    {
		if (!Enabled) return false;
		
        return UnityEngine.Input.GetButton(buttonName);
    }

    public static new bool GetButtonDown(string buttonName)
    {
		if (!Enabled) return false;
		
        return UnityEngine.Input.GetButtonDown(buttonName);
    }

    public static new bool GetButtonUp(string buttonName)
    {
		if (!Enabled) return false;
		
        return UnityEngine.Input.GetButtonUp(buttonName);
    }

    public static new bool GetKey(KeyCode keyCode)
    {
		if (!Enabled) return false;
		
#if UNITY_EDITOR
		int platform = (int)System.Environment.OSVersion.Platform;
        if (platform != 4 && platform != 6 && platform != 128)
		{
			return keys.Contains(keyCode);
		}
#endif
		return UnityEngine.Input.GetKey(keyCode);
    }

    public static new bool GetKeyDown(KeyCode keyCode)
    {
		if (!Enabled) return false;
		
#if UNITY_EDITOR
        int platform = (int)System.Environment.OSVersion.Platform;
        if (platform != 4 && platform != 6 && platform != 128)
		{
			if (keysPressed.Contains(keyCode))
			{
				keysPressed.Remove(keyCode);
				return true;
			}
			else
			{
				return false;
			}
		}
#endif
        return UnityEngine.Input.GetKeyDown(keyCode);
    }

    public static new bool GetKeyUp(KeyCode keyCode)
    {
		if (!Enabled) return false;
		
#if UNITY_EDITOR
        int platform = (int)System.Environment.OSVersion.Platform;
        if (platform != 4 && platform != 6 && platform != 128)
		{
			if (keysReleased.Contains(keyCode))
			{
				keysReleased.Remove(keyCode);
				return true;
			}
			else
			{
				return false;
			}
		}
#endif
        return UnityEngine.Input.GetKeyUp(keyCode);
    }
}