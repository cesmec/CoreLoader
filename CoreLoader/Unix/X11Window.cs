using System;
using System.Runtime.InteropServices;
using CoreLoader.Events;
using CoreLoader.Input;
using CoreLoader.Unix.Native;

namespace CoreLoader.Unix
{
    internal sealed class X11Window : INativeWindow
    {
        private readonly string _title;
        private readonly byte[] _keys = new byte[32];
        private IX11WindowExtensions _x11Extensions;

        public int Width { get; private set; }
        public int Height { get; private set; }
        public bool CloseRequested { get; private set; }
        public IKeys Keys { get; }

        public IntPtr NativeHandle { get; }
        public uint WindowId { get; private set; }
        internal IntPtr EventPtr { get; }

        public event EventHandler<KeyEventArgs> OnKeyDown;
        public event EventHandler<KeyEventArgs> OnKeyUp;
        public event EventHandler<ClickEventArgs> OnClick;
        public event EventHandler<MouseMoveEventArgs> OnMouseMove;
        public event EventHandler<ScrollEventArgs> OnScroll;
        public event EventHandler<ResizeEventArgs> OnResize;
        public event EventHandler<FocusChangeEventArgs> OnFocusChange;

        internal X11Window(string title, int width, int height)
        {
            _title = title;
            Width = width;
            Height = height;
            NativeHandle = X11.XOpenDisplay(null);
            Keys = new UnixKeys(NativeHandle);

            EventPtr = Marshal.AllocHGlobal(Marshal.SizeOf<XEvent>());
        }

        public void Show()
        {
            if (_x11Extensions == null)
                throw new InvalidOperationException("No Graphics API implementation in use");

            var display = Marshal.PtrToStructure<X11.XDisplay>(NativeHandle);
            var screen = Marshal.PtrToStructure<X11.Screen>(display.screens + display.default_screen);

            var visualInfo = _x11Extensions.GetVisualInfo(display);
            var visual = Marshal.PtrToStructure<X11.Visual>(visualInfo.visual);

            var cmap = X11.XCreateColormap(NativeHandle, screen.root, ref visual, 0 /*AllocNone*/);
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
            WindowId = X11.XCreateWindow(NativeHandle, screen.root, 0, 0, (uint)Width, (uint)Height, 0, visualInfo.depth, 0, ref visual, 1L << 13/*CWColormap*/ | 1L << 11/*CWEventMask*/, ref windowAttributes);

            X11.XMapWindow(NativeHandle, WindowId);
            X11.XStoreName(NativeHandle, WindowId, _title);

            //Attach events
            var wmResize = X11.XInternAtom(NativeHandle, "WM_SIZE_HINTS", true);
            var wmDelete = X11.XInternAtom(NativeHandle, "WM_DELETE_WINDOW", true);
            var events = new[] { wmResize, wmDelete };
            X11.XSetWMProtocols(NativeHandle, WindowId, events, events.Length);
        }

        public KeyState GetKeyState(uint key)
        {
            var pressed = _keys[(int)key / 8] & (1 << ((int)key % 8));

            return pressed == 0 ? KeyState.Released : KeyState.Pressed;
        }

        public bool GetCursorPosition(out Point position)
        {
            if (X11.XQueryPointer(NativeHandle, WindowId, out _, out _, out _, out _, out var winX, out var winY, out _))
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
            X11.XWarpPointer(NativeHandle, 0, WindowId, 0, 0, 0, 0, position.X, position.Y);
        }

        public void SetCursorVisible(bool visible)
        {
            if (visible)
            {
                X11.XUndefineCursor(NativeHandle, WindowId);
            }
            else
            {
                var black = new X11.XColor();
                var noData = new byte[8];

                var bitmapNoData = X11.XCreateBitmapFromData(NativeHandle, WindowId, noData, 8, 8);
                var invisibleCursor = X11.XCreatePixmapCursor(NativeHandle, bitmapNoData, bitmapNoData, ref black, ref black, 0, 0);
                X11.XDefineCursor(NativeHandle, WindowId, invisibleCursor);
                X11.XFreeCursor(NativeHandle, invisibleCursor);
                X11.XFreePixmap(NativeHandle, bitmapNoData);
            }
        }

        public void SetTitle(string title)
        {
            X11.XStoreName(NativeHandle, WindowId, title);
        }

        public void Close()
        {
            X11.XDestroyWindow(NativeHandle, WindowId);
            X11.XCloseDisplay(NativeHandle);
            Marshal.FreeHGlobal(EventPtr);
        }

        public void SetCloseRequested()
        {
            CloseRequested = true;
        }

        public void PollEvents()
        {
            while (X11.XPending(NativeHandle))
            {
                X11.XNextEvent(NativeHandle, EventPtr);
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
                        Width = exposeEvent.width;
                        Height = exposeEvent.height;
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

            X11.XQueryKeymap(NativeHandle, _keys);
        }

        public void SetWindowExtensions(IWindowExtensions extensions)
        {
            if (extensions is IX11WindowExtensions x11Extensions)
                _x11Extensions = x11Extensions;
        }
    }
}