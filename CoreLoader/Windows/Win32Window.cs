﻿using System;
using System.Runtime.InteropServices;
using System.Threading;
using CoreLoader.Events;
using CoreLoader.Input;
using CoreLoader.Windows.Native;

namespace CoreLoader.Windows
{
    internal sealed class Win32Window : INativeWindow
    {
        private readonly User32.WndProc _wndProc;
        private readonly IntPtr _msg;
        private readonly ManualResetEvent _loadedEvent = new ManualResetEvent(false);
        private bool _cursorVisible = true;

        public int Width { get; private set; }
        public int Height { get; private set; }
        public bool CloseRequested { get; private set; }
        public IKeys Keys { get; } = new Win32Keys();
        public IntPtr NativeHandle { get; }
        uint INativeWindow.WindowId => 0;

        public event EventHandler<KeyEventArgs> OnKeyDown;
        public event EventHandler<KeyEventArgs> OnKeyUp;
        public event EventHandler<ClickEventArgs> OnClick;
        public event EventHandler<MouseMoveEventArgs> OnMouseMove;
        public event EventHandler<ScrollEventArgs> OnScroll;
        public event EventHandler<ResizeEventArgs> OnResize;
        public event EventHandler<FocusChangeEventArgs> OnFocusChange;

        internal Win32Window(string title, int width, int height)
        {
            Width = width;
            Height = height;
            _wndProc = WndProc;
            var wndClass = new User32.WndClass
            {
                lpfnWndProc = _wndProc,
                style = 0x20,
                hbrBackground = new IntPtr(1),
                lpszClassName = "coreloader"
            };
            User32.RegisterClassA(ref wndClass);

            const int offset = 50;

            var rect = new User32.Rect
            {
                top = offset,
                left = offset,
                bottom = offset + height,
                right = offset + width
            };
            var style = WindowStyles.WS_OVERLAPPEDWINDOW;

            User32.AdjustWindowRect(ref rect, style, false);

            NativeHandle = User32.CreateWindowExA(0, wndClass.lpszClassName, title, style, rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);

            const int sizeofMsg = 48;
            _msg = Marshal.AllocHGlobal(sizeofMsg);
        }

        public void Show()
        {
            User32.ShowWindow(NativeHandle, 10);
            User32.UpdateWindow(NativeHandle);

            //wglGetProcAddress only works after wglMakeCurrent has been called
            _loadedEvent.WaitOne();
            _loadedEvent.Dispose();
        }

        public bool IsKeyPressed(uint key)
        {
            var state = User32.GetAsyncKeyState((int)key);
            return ((state >> 15) & 0x01) != 0;
        }

        public bool GetCursorPosition(out Point position)
        {
            if (!User32.GetCursorPos(out var cursorPos) || !User32.GetWindowInfo(NativeHandle, out var windowInfo))
            {
                position = default;
                return false;
            }

            position = new Point
            {
                X = cursorPos.X - windowInfo.rcClient.left,
                Y = cursorPos.Y - windowInfo.rcClient.top
            };
            return true;
        }

        public void SetCursorPosition(in Point position)
        {
            if (User32.GetWindowInfo(NativeHandle, out var windowInfo))
            {
                User32.SetCursorPos(position.X + windowInfo.rcClient.left, position.Y + windowInfo.rcClient.top);
            }
        }

        public void SetCursorVisible(bool visible)
        {
            if (_cursorVisible != visible)
                _ = User32.ShowCursor(visible);
            _cursorVisible = visible;
        }

        public void SetTitle(string title)
        {
            User32.SetWindowTextA(NativeHandle, title);
        }

        public void PollEvents()
        {
            while (User32.PeekMessageA(_msg, NativeHandle, 0, 0, 1))
            {
                User32.TranslateMessage(_msg);
                User32.DispatchMessageA(_msg);
            }
        }

        public void Close()
        {
            Marshal.FreeHGlobal(_msg);
            User32.CloseWindow(NativeHandle);
            User32.DestroyWindow(NativeHandle);
        }

        public void SetCloseRequested()
        {
            CloseRequested = true;
        }

        private static short LowWord(long value)
        {
            return (short)(value & 0xFFFF);
        }

        private static short HighWord(long value)
        {
            return (short)((value >> 16) & 0xFFFF);
        }

        private long WndProc(IntPtr hWnd, uint message, uint wParam, long lParam)
        {
            var loWord = LowWord(lParam);
            var hiWord = HighWord(lParam);

            switch (message)
            {
                case 0x0001: //Create
                    _loadedEvent.Set();
                    break;
                case 0x0005: //Size
                    Width = loWord;
                    Height = hiWord;
                    OnResize?.Invoke(this, new ResizeEventArgs(Width, Height));
                    break;
                case 0x0007: //SetFocus
                    OnFocusChange?.Invoke(this, new FocusChangeEventArgs(true));
                    break;
                case 0x0008: //KillFocus
                    OnFocusChange?.Invoke(this, new FocusChangeEventArgs(false));
                    break;
                case 0x0010: //Close
                    CloseRequested = true;
                    break;
                case 0x0200: //MouseMove
                    OnMouseMove?.Invoke(this, new MouseMoveEventArgs(loWord, hiWord));
                    break;
                case 0x0100: //KeyDown
                    OnKeyDown?.Invoke(this, new KeyEventArgs(wParam));
                    break;
                case 0x0101: //KeyUp
                    OnKeyUp?.Invoke(this, new KeyEventArgs(wParam));
                    break;
                case 0x0201: //LButtonDown
                    OnClick?.Invoke(this, ClickEventArgs.Down(new Point(loWord, hiWord), ClickEventArgs.MouseButton.Left));
                    break;
                case 0x0202: //LButtonUp
                    OnClick?.Invoke(this, ClickEventArgs.Up(new Point(loWord, hiWord), ClickEventArgs.MouseButton.Left));
                    break;
                case 0x0204: //RButtonDown
                    OnClick?.Invoke(this, ClickEventArgs.Down(new Point(loWord, hiWord), ClickEventArgs.MouseButton.Right));
                    break;
                case 0x0205: //RButtonUp
                    OnClick?.Invoke(this, ClickEventArgs.Up(new Point(loWord, hiWord), ClickEventArgs.MouseButton.Right));
                    break;
                case 0x0207: //MButtonDown
                    OnClick?.Invoke(this, ClickEventArgs.Down(new Point(loWord, hiWord), ClickEventArgs.MouseButton.Middle));
                    break;
                case 0x0208: //MButtonUp
                    OnClick?.Invoke(this, ClickEventArgs.Up(new Point(loWord, hiWord), ClickEventArgs.MouseButton.Middle));
                    break;
                case 0x020B: //XButtonDown
                    OnClick?.Invoke(this, ClickEventArgs.Down(new Point(loWord, hiWord), ClickEventArgs.MouseButton.Middle + (uint)HighWord(wParam)));
                    break;
                case 0x020C: //XButtonUp
                    OnClick?.Invoke(this, ClickEventArgs.Up(new Point(loWord, hiWord), ClickEventArgs.MouseButton.Middle + (uint)HighWord(wParam)));
                    break;
                case 0x020A: //MouseWheel Up / Down
                case 0x020E: //MouseWheel Left / Right
                    if (OnScroll != null)
                    {
                        var direction = message == 0x020A
                            ? ScrollDirection.UpDown
                            : ScrollDirection.LeftRight;
                        var delta = HighWord(wParam);

                        var mousePosition = new Point(loWord, hiWord);
                        OnScroll(this, new ScrollEventArgs(direction, mousePosition, delta));
                    }
                    break;
            }

            return User32.DefWindowProcA(hWnd, message, wParam, lParam);
        }

        public void SetWindowExtensions(IWindowExtensions extensions)
        { }
    }
}