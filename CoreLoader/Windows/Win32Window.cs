using System;
using System.Threading;
using CoreLoader.Events;
using CoreLoader.Windows.Native;

namespace CoreLoader.Windows
{
    public abstract class Win32Window : IWindow
    {
        protected readonly IntPtr WindowPtr;
        private readonly User32.WndProc _wndProc;
        private readonly ManualResetEvent _loadedEvent = new ManualResetEvent(false);
        private bool _cursorVisible = true;

        public int Width { get; private set; }
        public int Height { get; private set; }
        public bool CloseRequested { get; private set; }
        public IKeys Keys { get; } = new Win32Keys();

        public event EventHandler<KeyEventArgs> OnKeyDown;
        public event EventHandler<KeyEventArgs> OnKeyUp;
        public event EventHandler<ClickEventArgs> OnClick;
        public event EventHandler<MouseMoveEventArgs> OnMouseMove;
        public event EventHandler<ScrollEventArgs> OnScroll;
        public event EventHandler<ResizeEventArgs> OnResize;
        public event EventHandler<FocusChangeEventArgs> OnFocusChange;

        protected Win32Window(string title, int width, int height)
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

            WindowPtr = User32.CreateWindowExA(0, wndClass.lpszClassName, title, style, rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);

            User32.ShowWindow(WindowPtr, 10);
            User32.UpdateWindow(WindowPtr);

            //wglGetProcAddress only works after wglMakeCurrent has been called
            _loadedEvent.WaitOne();
            _loadedEvent.Dispose();
        }

        public KeyState GetKeyState(uint key)
        {
            var state = User32.GetAsyncKeyState((int)key);
            switch (state)
            {
                case -32767: //todo check values
                case -32768:
                    return KeyState.Pressed;
                case 0:
                    return KeyState.Released;
                default:
                    return KeyState.Unknown;
            }
        }

        public bool GetCursorPosition(out Point position)
        {
            if (!User32.GetCursorPos(out var cursorPos) || !User32.GetWindowInfo(WindowPtr, out var windowInfo))
            {
                position = new Point();
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
            if (User32.GetWindowInfo(WindowPtr, out var windowInfo))
            {
                User32.SetCursorPos(position.X + windowInfo.rcClient.left, position.Y + windowInfo.rcClient.top);
            }
        }

        public void SetCursorVisible(bool visible)
        {
            if (_cursorVisible != visible)
                User32.ShowCursor(visible);
            _cursorVisible = visible;
        }

        public void SetTitle(string title)
        {
            User32.SetWindowTextA(WindowPtr, title);
        }

        public void PollEvents()
        {
            var msg = IntPtr.Zero;
            while (User32.PeekMessageA(msg, WindowPtr, 0, 0, 1))
            {
                User32.TranslateMessage(msg);
                User32.DispatchMessageA(msg);
            }
        }

        public abstract void SwapBuffers();

        public void Close()
        {
            Cleanup();
            User32.CloseWindow(WindowPtr);
            User32.DestroyWindow(WindowPtr);
        }

        public void SetCloseRequested()
        {
            CloseRequested = true;
        }

        protected abstract void Cleanup();
        protected abstract long Create(IntPtr hWnd);

        private long WndProc(IntPtr hWnd, uint message, uint wParam, long lParam)
        {
            switch (message)
            {
                case 0x0001: //Create
                    var value = Create(hWnd);
                    _loadedEvent.Set();
                    return value;
                case 0x0005: //Size
                    Width = (int)(lParam & 0xffff);
                    Height = (int)(lParam >> 16 & 0xffff);
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
                    OnMouseMove?.Invoke(this, new MouseMoveEventArgs((int)(lParam & 0xffff), (int)(lParam >> 16 & 0xffff)));
                    break;
                case 0x0100: //KeyDown
                    OnKeyDown?.Invoke(this, new KeyEventArgs(wParam));
                    break;
                case 0x0101: //KeyUp
                    OnKeyUp?.Invoke(this, new KeyEventArgs(wParam));
                    break;
                case 0x0201: //LButtonDown
                    OnClick?.Invoke(this, ClickEventArgs.Down(ClickEventArgs.Left));
                    break;
                case 0x0202: //LButtonUp
                    OnClick?.Invoke(this, ClickEventArgs.Up(ClickEventArgs.Left));
                    break;
                case 0x0204: //RButtonDown
                    OnClick?.Invoke(this, ClickEventArgs.Down(ClickEventArgs.Right));
                    break;
                case 0x0205: //RButtonUp
                    OnClick?.Invoke(this, ClickEventArgs.Up(ClickEventArgs.Right));
                    break;
                case 0x0207: //MButtonDown
                    OnClick?.Invoke(this, ClickEventArgs.Down(ClickEventArgs.Middle));
                    break;
                case 0x0208: //MButtonUp
                    OnClick?.Invoke(this, ClickEventArgs.Up(ClickEventArgs.Middle));
                    break;
                case 0x020B: //XButtonDown
                    OnClick?.Invoke(this, ClickEventArgs.Down(wParam));
                    break;
                case 0x020C: //XButtonUp
                    OnClick?.Invoke(this, ClickEventArgs.Up(wParam));
                    break;
                case 0x020A: //MouseWheel Up / Down
                    OnScroll?.Invoke(this, new ScrollEventArgs(ScrollDirection.UpDown, (int)wParam));
                    break;
                case 0x020E: //MouseWheel Left / Right
                    OnScroll?.Invoke(this, new ScrollEventArgs(ScrollDirection.LeftRight, (int)wParam));
                    break;
            }

            return User32.DefWindowProcA(hWnd, message, wParam, lParam);
        }
    }
}