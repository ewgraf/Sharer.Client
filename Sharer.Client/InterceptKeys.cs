using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

class InterceptKeys {
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private static LowLevelKeyboardProc _proc = HookCallback;
    private static IntPtr _hookID = IntPtr.Zero;
    private static Action<CancellationToken> CtrlShiftD2OuterCallback;   // @
    private static Action<CancellationToken> CtrlShiftD3OuterCallback;   // #
    private static Func<CancellationToken, Task> CtrlShiftD6OuterCallback; // ^

    // Specifies the type of hook procedure to be installed
    public enum HookType {
        CALLWNDPROC = 4,
        CALLWNDPROCRET = 12,
        CBT = 5,
        DEBUG = 9,
        FOREGROUNDIDLE = 11,
        GETMESSAGE = 3,
        JOURNALPLAYBACK = 1,
        JOURNALRECORD = 0,
        KEYBOARD = 2,
        KEYBOARD_LL = 13,
        MOUSE = 7,
        MOUSE_LL = 14,
        MSGFILTER = -1,
        SHELL = 10,
        SYSMSGFILTER = 6
    }


	private static CancellationToken _token;

	public static void SetHooks(Action<CancellationToken> act0, Action<CancellationToken> act1, Func<CancellationToken, Task> act2, CancellationToken token) {
        _hookID = SetHook(_proc);
        CtrlShiftD2OuterCallback = act0;
        CtrlShiftD3OuterCallback = act1;
        CtrlShiftD6OuterCallback = act2;
		_token = token;
	}

    public static void UnSetHooks() {
        UnhookWindowsHookEx(_hookID);
    }

    private static IntPtr SetHook(LowLevelKeyboardProc proc) {
        using (Process curProcess = Process.GetCurrentProcess()) {
            using (ProcessModule curModule = curProcess.MainModule) {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }
    }

    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

    private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam) {
        if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN) {
            Keys code = (Keys)Marshal.ReadInt32(lParam); // vkCode v?irtual c?ode
            if (IsControlKeyDown() && IsShiftKeyDown()) {
                if (code == Keys.D2) {
                    CtrlShiftD2OuterCallback(_token);
                } else if (code == Keys.D3) {
                    CtrlShiftD3OuterCallback(_token);
                } else if (code == Keys.D6) {
                    CtrlShiftD6OuterCallback(_token);
                }
            }            
        }
        return CallNextHookEx(_hookID, nCode, wParam, lParam);
    }

    public static bool IsControlKeyDown() {
        return IsModifierKeyDown(Keys.ControlKey, Keys.Control);
    }

    public static bool IsShiftKeyDown() {
        return IsModifierKeyDown(Keys.ShiftKey, Keys.Shift);
    }

    public static bool IsAltKeyDown() {
        return IsModifierKeyDown(Keys.Menu, Keys.Alt);
    }

    private static bool IsModifierKeyDown(Keys virtualKey, Keys key) {
        var none = Keys.None;
        if (GetKeyState(virtualKey) < 0) {
            none |= key;
        }
        return (none & key) == key;
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
    private static extern short GetKeyState(Keys key);
}
