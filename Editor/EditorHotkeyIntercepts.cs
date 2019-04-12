using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEditor;
using Debug = UnityEngine.Debug;
using Application = UnityEngine.Application;
using System.Threading.Tasks;
using UnityEngine;

[InitializeOnLoad]
public class EditorHotkeyIntercepts
{
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private const int WM_KEYUP = 0x0101;
    private static readonly LowLevelKeyboardProc _proc = HookCallback;
    private static IntPtr _hookID = IntPtr.Zero;
    private static List<int> keysPressed = new List<int>();

    static EditorHotkeyIntercepts()
    {
        string key = SystemInfo.deviceUniqueIdentifier + "." + PlayerSettings.productGUID + ".Interception.HookID";
        IntPtr oldHookId = new IntPtr(EditorPrefs.GetInt(key));
        UnhookWindowsHookEx(oldHookId);

        _hookID = SetHook(_proc);
        EditorPrefs.SetInt(key, _hookID.ToInt32());
    }

    private static IntPtr SetHook(LowLevelKeyboardProc proc)
    {
        using (Process curProcess = Process.GetCurrentProcess())
        using (ProcessModule curModule = curProcess.MainModule)
        {
            IntPtr moduleHandle = GetModuleHandle(curModule.ModuleName);
            return SetWindowsHookEx(WH_KEYBOARD_LL, proc, moduleHandle, 0);
        }
    }

    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

    private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0)
        {
            if (ApplicationIsActivated)
            {
                int vkcode = Marshal.ReadInt32(lParam);
                if (wParam == (IntPtr)WM_KEYDOWN)
                {
                    bool intercept = false;
                    if (!keysPressed.Contains(vkcode))
                    {
                        keysPressed.Add(vkcode);
                        Pressed(vkcode);

                        //supress any ctrl + _ shortcuts in play mode
                        if (keysPressed.Contains(162) || keysPressed.Contains(163))
                        {
                            if (Application.isPlaying)
                            {
                                intercept = true;
                            }
                        }
                    }

                    if (intercept)
                    {
                        return (IntPtr)(-1);
                    }
                }
                else if (wParam == (IntPtr)WM_KEYUP)
                {
                    if (keysPressed.Contains(vkcode))
                    {
                        keysPressed.Remove(vkcode);
                        Released(vkcode);
                    }
                }
            }
        }
        return CallNextHookEx(_hookID, nCode, wParam, lParam);
    }

    private static KeyCode ToKeyCode(int vkcode)
    {
        if (vkcode == 7) return KeyCode.None;
        if (vkcode == 8) return KeyCode.Backspace;
        if (vkcode == 9) return KeyCode.Tab;
        if (vkcode == 13) return KeyCode.Return;
        if (vkcode == 20) return KeyCode.CapsLock;
        if (vkcode == 27) return KeyCode.Escape;

        if (vkcode == 32) return KeyCode.Space;
        if (vkcode == 33) return KeyCode.PageUp;
        if (vkcode == 34) return KeyCode.PageDown;
        if (vkcode == 35) return KeyCode.End;
        if (vkcode == 36) return KeyCode.Home;
        if (vkcode == 37) return KeyCode.LeftArrow;
        if (vkcode == 38) return KeyCode.UpArrow;
        if (vkcode == 39) return KeyCode.RightArrow;
        if (vkcode == 40) return KeyCode.DownArrow;
        if (vkcode == 45) return KeyCode.Insert;
        if (vkcode == 46) return KeyCode.Delete;
        if (vkcode == 47) return KeyCode.Help;

        if (vkcode == 48) return KeyCode.Alpha0;
        if (vkcode == 49) return KeyCode.Alpha1;
        if (vkcode == 50) return KeyCode.Alpha2;
        if (vkcode == 51) return KeyCode.Alpha3;
        if (vkcode == 52) return KeyCode.Alpha4;
        if (vkcode == 53) return KeyCode.Alpha5;
        if (vkcode == 54) return KeyCode.Alpha6;
        if (vkcode == 55) return KeyCode.Alpha7;
        if (vkcode == 56) return KeyCode.Alpha8;
        if (vkcode == 57) return KeyCode.Alpha9;
        if (vkcode == 59) return KeyCode.Alpha1;

        if (vkcode == 65) return KeyCode.A;
        if (vkcode == 66) return KeyCode.B;
        if (vkcode == 67) return KeyCode.C;
        if (vkcode == 68) return KeyCode.D;
        if (vkcode == 69) return KeyCode.E;
        if (vkcode == 70) return KeyCode.F;
        if (vkcode == 71) return KeyCode.G;
        if (vkcode == 72) return KeyCode.H;
        if (vkcode == 73) return KeyCode.I;
        if (vkcode == 74) return KeyCode.J;
        if (vkcode == 75) return KeyCode.K;
        if (vkcode == 76) return KeyCode.L;
        if (vkcode == 77) return KeyCode.M;
        if (vkcode == 78) return KeyCode.N;
        if (vkcode == 79) return KeyCode.O;
        if (vkcode == 80) return KeyCode.P;
        if (vkcode == 81) return KeyCode.Q;
        if (vkcode == 82) return KeyCode.R;
        if (vkcode == 83) return KeyCode.S;
        if (vkcode == 84) return KeyCode.T;
        if (vkcode == 85) return KeyCode.U;
        if (vkcode == 86) return KeyCode.V;
        if (vkcode == 87) return KeyCode.W;
        if (vkcode == 88) return KeyCode.X;
        if (vkcode == 89) return KeyCode.Y;
        if (vkcode == 90) return KeyCode.Z;
        if (vkcode == 91) return KeyCode.LeftWindows;
        if (vkcode == 93) return KeyCode.AltGr;

        if (vkcode == 96) return KeyCode.Keypad0;
        if (vkcode == 97) return KeyCode.Keypad1;
        if (vkcode == 98) return KeyCode.Keypad2;
        if (vkcode == 99) return KeyCode.Keypad3;
        if (vkcode == 100) return KeyCode.Keypad4;
        if (vkcode == 101) return KeyCode.Keypad5;
        if (vkcode == 102) return KeyCode.Keypad6;
        if (vkcode == 103) return KeyCode.Keypad7;
        if (vkcode == 104) return KeyCode.Keypad8;
        if (vkcode == 105) return KeyCode.Keypad9;

        if (vkcode == 112) return KeyCode.F1;
        if (vkcode == 113) return KeyCode.F2;
        if (vkcode == 114) return KeyCode.F3;
        if (vkcode == 115) return KeyCode.F4;
        if (vkcode == 116) return KeyCode.F5;
        if (vkcode == 117) return KeyCode.F6;
        if (vkcode == 118) return KeyCode.F7;
        if (vkcode == 119) return KeyCode.F8;
        if (vkcode == 120) return KeyCode.F9;
        if (vkcode == 121) return KeyCode.F10;
        if (vkcode == 122) return KeyCode.F11;
        if (vkcode == 123) return KeyCode.F12;
        if (vkcode == 124) return KeyCode.F13;
        if (vkcode == 125) return KeyCode.F14;
        if (vkcode == 126) return KeyCode.F15;

        if (vkcode == 144) return KeyCode.Numlock;
        if (vkcode == 145) return KeyCode.ScrollLock;

        if (vkcode == 160) return KeyCode.LeftShift;
        if (vkcode == 161) return KeyCode.RightShift;

        if (vkcode == 162) return KeyCode.LeftControl;
        if (vkcode == 163) return KeyCode.RightControl;

        if (vkcode == 179) return KeyCode.None;

        if (vkcode == 186) return KeyCode.Semicolon;
        if (vkcode == 188) return KeyCode.Less;
        if (vkcode == 187) return KeyCode.Plus;
        if (vkcode == 189) return KeyCode.Minus;
        if (vkcode == 190) return KeyCode.Greater;
        if (vkcode == 191) return KeyCode.Slash;
        if (vkcode == 192) return KeyCode.BackQuote;
        if (vkcode == 219) return KeyCode.LeftBracket;
        if (vkcode == 220) return KeyCode.Backslash;
        if (vkcode == 221) return KeyCode.RightBracket;
        if (vkcode == 222) return KeyCode.Quote;

        throw new Exception("Converter for Virtual Key Code " + vkcode + " not implemented.");
        //consult http://cherrytree.at/misc/vk.htm for the vkcode table
    }

    private static async void Pressed(int vkcode)
    {
        KeyCode keyCode = ToKeyCode(vkcode);

        //press this key on the new frame
        Input.keysPressed.Add(keyCode);
        Input.keys.Add(keyCode);

        int thisFrame = Time.frameCount;
        while (thisFrame == Time.frameCount)
        {
            await Task.Delay(1);
        }

        //frame later, remove the pressed key
        Input.keysPressed.Remove(keyCode);
    }

    private static async void Released(int vkcode)
    {
        KeyCode keyCode = ToKeyCode(vkcode);

        //release this key, for this frame
        Input.keysReleased.Add(keyCode);
        Input.keys.Remove(keyCode);

        //wait until its the next frame
        int thisFrame = Time.frameCount;
        while (thisFrame == Time.frameCount)
        {
            await Task.Delay(1);
        }

        //frame later, remove the pressed key
        Input.keysReleased.Remove(keyCode);
    }

    public static bool ApplicationIsActivated
    {
        get
        {
            var activatedHandle = GetForegroundWindow();
            if (activatedHandle == IntPtr.Zero)
            {
                return false;       // No window is currently activated
            }

            var procId = Process.GetCurrentProcess().Id;
            int activeProcId;
            GetWindowThreadProcessId(activatedHandle, out activeProcId);

            return activeProcId == procId;
        }
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);
}