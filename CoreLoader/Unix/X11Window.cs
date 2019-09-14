using System;
using System.Linq;
using System.Runtime.InteropServices;
using CoreLoader.Events;
using CoreLoader.Unix.Native;

namespace CoreLoader.Unix
{
    public abstract class X11Window : IWindow
    {
        protected readonly IntPtr DisplayPtr;
        protected readonly uint WindowId;
        protected readonly IntPtr EventPtr;
        private readonly byte[] _keys = new byte[32];

        public bool CloseRequested { get; private set; }
        public IKeyCodes KeyCodes { get; }

        public event EventHandler<KeyEventArgs> OnKeyDown;
        public event EventHandler<KeyEventArgs> OnKeyUp;
        public event EventHandler<ClickEventArgs> OnClick;
        public event EventHandler<MouseMoveEventArgs> OnMouseMove;
        public event EventHandler<ScrollEventArgs> OnScroll;
        public event EventHandler<ResizeEventArgs> OnResize;
        public event EventHandler<FocusChangeEventArgs> OnFocusChange;

        protected X11Window(string title, int width, int height)
        {
            DisplayPtr = X11.XOpenDisplay(null);
            KeyCodes = new UnixKeyCodes(DisplayPtr);
            var display = Marshal.PtrToStructure<X11.XDisplay>(DisplayPtr);
            var screen = Marshal.PtrToStructure<X11.Screen>(display.screens + display.default_screen);

            var visualInfo = GetVisualInfo(display);
            var visual = Marshal.PtrToStructure<X11.Visual>(visualInfo.visual);

            var cmap = X11.XCreateColormap(DisplayPtr, screen.root, ref visual, 0 /*AllocNone*/);
            var windowAttributes = new X11.XSetWindowAttributes
            {
                colormap = cmap,
                event_mask = 
                    1L << 0 /*KeyPressMask*/ |
                    1L << 1 /*KeyReleaseMask*/ |
                    1L << 2 /*ButtonPressMask*/ |
                    1L << 3 /*ButtonReleaseMask*/ |
                    1L << 6 /*PointerMotionMask*/ |
                    1L << 15 /*ExposureMask*/ |
                    1L << 21 /*FocusChangeMask*/
            };
            WindowId = X11.XCreateWindow(DisplayPtr, screen.root, 0, 0, (uint)width, (uint)height, 0, visualInfo.depth, 0, ref visual, 1L << 13/*CWColormap*/ | 1L << 11/*CWEventMask*/, ref windowAttributes);

            X11.XMapWindow(DisplayPtr, WindowId);
            X11.XStoreName(DisplayPtr, WindowId, title);

            //Attach events
            var wmResize = X11.XInternAtom(DisplayPtr, "WM_SIZE_HINTS", true);
            var wmDelete = X11.XInternAtom(DisplayPtr, "WM_DELETE_WINDOW", true);
            var events = new[] { wmResize, wmDelete };
            X11.XSetWMProtocols(DisplayPtr, WindowId, events, events.Length);

            EventPtr = Marshal.AllocHGlobal(Marshal.SizeOf<XEvent>());
        }

        public KeyState GetKeyState(uint key)
        {
            var pressed = _keys[(int)key / 8] & (1 << ((int)key % 8));

            return pressed == 0 ? KeyState.Released : KeyState.Pressed;
        }

        public bool GetCursorPosition(out Point position)
        {
            if (X11.XQueryPointer(DisplayPtr, WindowId, out var rootWindow, out var childWindow, out var rootX, out var rootY, out var winX, out var winY, out var mask))
            {
                position = new Point
                {
                    X = winX,
                    Y = winY
                };
                return true;
            }
            
            position = new Point();
            return false;
        }

        public void SetCursorPosition(in Point position)
        {
            X11.XWarpPointer(DisplayPtr, 0, WindowId, 0, 0, 0, 0, position.X, position.Y);
        }

        public void SetCursorVisible(bool visible)
        {
            if (visible)
            {
                X11.XUndefineCursor(DisplayPtr, WindowId);
            }
            else
            {
                X11.XColor black = new X11.XColor();
                var noData = new byte[8];

                var bitmapNoData = X11.XCreateBitmapFromData(DisplayPtr, WindowId, noData, 8, 8);
                var invisibleCursor = X11.XCreatePixmapCursor(DisplayPtr, bitmapNoData, bitmapNoData, ref black, ref black, 0, 0);
                X11.XDefineCursor(DisplayPtr, WindowId, invisibleCursor);
                X11.XFreeCursor(DisplayPtr, invisibleCursor);
                X11.XFreePixmap(DisplayPtr, bitmapNoData);
            }
        }

        public void SetTitle(string title)
        {
            X11.XStoreName(DisplayPtr, WindowId, title);
        }

        public void Close()
        {
            Cleanup();
            X11.XDestroyWindow(DisplayPtr, WindowId);
            X11.XCloseDisplay(DisplayPtr);
        }

        public void PollEvents()
        {
            while (X11.XPending(DisplayPtr))
            {
                X11.XNextEvent(DisplayPtr, EventPtr);
                var ev = Marshal.PtrToStructure<XEvent>(EventPtr);
                switch (ev.type)
                {
                    case 1: //KeyPress
                    {
                        var keyEvent = Marshal.PtrToStructure<XKeyEvent>(EventPtr);
                        OnKeyDown?.Invoke(this, new KeyEventArgs(keyEvent.keycode));
                        break;
                    }
                    case 2: //KeyRelease
                    {
                        var keyEvent = Marshal.PtrToStructure<XKeyEvent>(EventPtr);
                        OnKeyUp?.Invoke(this, new KeyEventArgs(keyEvent.keycode));
                        break;
                    }
                    case 4: //ButtonPress
                    {
                        var buttonEvent = Marshal.PtrToStructure<XButtonEvent>(EventPtr);
                        OnClick?.Invoke(this, new ClickEventArgs(ClickEventState.Down, buttonEvent.button));
                        break;
                    }
                    case 5: //ButtonRelease
                    {
                        var buttonEvent = Marshal.PtrToStructure<XButtonEvent>(EventPtr);
                        OnClick?.Invoke(this, new ClickEventArgs(ClickEventState.Up, buttonEvent.button));
                        break;
                    }
                    case 6: //MotionNotify
                    {
                        var motionEvent = Marshal.PtrToStructure<XMotionEvent>(EventPtr);
                        OnMouseMove?.Invoke(this, new MouseMoveEventArgs(motionEvent.x, motionEvent.y));
                        break;
                    }
                    case 9: //FocusIn
                    {
                        OnFocusChange?.Invoke(this, new FocusChangeEventArgs(true));
                        break;
                    }
                    case 10: //FocusOut
                    {
                        OnFocusChange?.Invoke(this, new FocusChangeEventArgs(false));
                        break;
                    }
                    case 12: //Expose
                    {
                        var exposeEvent = Marshal.PtrToStructure<XExposeEvent>(EventPtr);
                        OnResize?.Invoke(this, new ResizeEventArgs(exposeEvent.width, exposeEvent.height));
                        break;
                    }
                    case 33: //ClientMessage
                    {
                        var clientMessage = Marshal.PtrToStructure<XClientMessageEvent>(EventPtr);
                        if (clientMessage.message_type == 313)
                        {
                            CloseRequested = true;
                        }
                        break;
                    }
                }
            }

            X11.XQueryKeymap(DisplayPtr, _keys);
        }

        public abstract void SwapBuffers();

        protected abstract X11.XVisualInfo GetVisualInfo(X11.XDisplay display);
        protected abstract void Cleanup();
    }
}