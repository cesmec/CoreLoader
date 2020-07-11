using System;
using System.Runtime.InteropServices;
using System.Text;

namespace CoreLoader.Windows.Native
{
    public static class User32
    {
        public delegate long WndProc(IntPtr hWnd, uint message, uint wParam, long lParam);

        public struct WndClass
        {
            public uint style;
            public WndProc lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpszMenuName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpszClassName;
        }

        public struct Rect
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        public struct WindowInfo
        {
            public uint cbSize;
            public Rect rcWindow;
            public Rect rcClient;
            public uint dwStyle;
            public uint dwExStyle;
            public uint dwWindowStatus;
            public uint cxWindowBorders;
            public uint cyWindowBorders;
            public ushort atomWindowType;
            public ushort wCreatorVersion;
        }

        [DllImport(nameof(User32), ExactSpelling = true)]
        public static extern void AdjustWindowRect(ref Rect rect, WindowStyles style, bool menu);
        [DllImport(nameof(User32), ExactSpelling = true, CharSet = CharSet.Ansi)]
        public static extern ushort RegisterClassA(ref WndClass wndClass);
        [DllImport(nameof(User32), ExactSpelling = true)]
        public static extern long DefWindowProcA(IntPtr hWnd, uint message, uint wParam, long lParam);
        [DllImport(nameof(User32), ExactSpelling = true, CharSet = CharSet.Ansi)]
        public static extern IntPtr CreateWindowExA(uint exStyle, [MarshalAs(UnmanagedType.LPStr)] string className, [MarshalAs(UnmanagedType.LPStr)] string title, WindowStyles style, int x, int y, int width, int height, IntPtr parent, IntPtr menu, IntPtr instance, IntPtr lpParam);
        [DllImport(nameof(User32), ExactSpelling = true)]
        public static extern void ShowWindow(IntPtr hWnd, int cmdShow);
        [DllImport(nameof(User32), ExactSpelling = true)]
        public static extern void UpdateWindow(IntPtr hWnd);
        [DllImport(nameof(User32), ExactSpelling = true)]
        public static extern bool CloseWindow(IntPtr hWnd);
        [DllImport(nameof(User32), ExactSpelling = true)]
        public static extern bool DestroyWindow(IntPtr hWnd);
        [DllImport(nameof(User32), ExactSpelling = true)]
        public static extern bool PeekMessageA(IntPtr msg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax, uint wRemoveMsg);
        [DllImport(nameof(User32), ExactSpelling = true)]
        public static extern void TranslateMessage(IntPtr msg);
        [DllImport(nameof(User32), ExactSpelling = true)]
        public static extern void DispatchMessageA(IntPtr msg);
        [DllImport(nameof(User32), ExactSpelling = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);
        [DllImport(nameof(User32), ExactSpelling = true)]
        public static extern void SetWindowTextA(IntPtr hWnd, [MarshalAs(UnmanagedType.LPStr)] string title);

        //cursor
        [DllImport(nameof(User32), ExactSpelling = true)]
        public static extern bool GetCursorPos(out Point position);
        [DllImport(nameof(User32), ExactSpelling = true)]
        public static extern void SetCursorPos(long x, long y);
        [DllImport(nameof(User32), ExactSpelling = true)]
        public static extern bool GetWindowInfo(IntPtr hWnd, out WindowInfo windowInfo);
        [DllImport(nameof(User32), ExactSpelling = true)]
        public static extern int ShowCursor(bool show);

        //keyboard
        [DllImport(nameof(User32), ExactSpelling = true)]
        public static extern short GetAsyncKeyState(int key);
        [DllImport(nameof(User32), ExactSpelling = true)]
        public static extern int GetKeyNameTextA(long lParam, [MarshalAs(UnmanagedType.LPStr)]StringBuilder lpString, int cchSize);
    }
}