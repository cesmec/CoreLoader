using System;
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

        public bool CloseRequested { get; private set; }

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
            var display = Marshal.PtrToStructure<X11.XDisplay>(DisplayPtr);
            var screen = Marshal.PtrToStructure<X11.Screen>(display.screens + display.default_screen);

            var visualInfo = GetVisualInfo(display);
            var visual = Marshal.PtrToStructure<X11.Visual>(visualInfo.visual);

            var cmap = X11.XCreateColormap(DisplayPtr, screen.root, ref visual, 0 /*AllocNone*/);
            var windowAttributes = new X11.XSetWindowAttributes
            {
                colormap = cmap,
                event_mask = /*1L << 18 /*ResizeRedirectMask*/ /*|*/ 1L << 15 /*ExposureMask*/ | 1L << 0 /*KeyPressMask*/
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

        public KeyState GetKeyState(int key)
        {
            //todo implement X11 version
            /*
            bool GetKeyState(KeySym keySym)
            {
                if(g_pDisplay == NULL)
                {
                    return false;
                }
             
                char szKey[32];
                int iKeyCodeToFind = XKeysymToKeycode(g_pDisplay, keySym);
             
                XQueryKeymap(g_pDisplay, szKey);
             
                return szKey[iKeyCodeToFind / 8] & (1 << (iKeyCodeToFind % 8));
            }
            */
            throw new NotImplementedException();
        }

        public bool GetCursorPosition(out Point position)
        {
            //todo implement
            position = new Point();
            return false;
        }

        public void SetCursorPosition(in Point position)
        {
            //todo implement
        }

        public void SetCursorVisible(bool visible)
        {
            //todo implement
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
                    case 12: /*Expose*/
                        var exposeEvent = Marshal.PtrToStructure<XExposeEvent>(EventPtr);
                        OnResize?.Invoke(this, new ResizeEventArgs(exposeEvent.width, exposeEvent.height));
                        break;
                    case 25: /*ResizeRequest*/
                        var resizeEvent = Marshal.PtrToStructure<XResizeRequestEvent>(EventPtr);
                        OnResize?.Invoke(this, new ResizeEventArgs(resizeEvent.width, resizeEvent.height));
                        break;
                    case 33: /*ClientMessage*/
                        var clientMessage = Marshal.PtrToStructure<XClientMessageEvent>(EventPtr);
                        if (clientMessage.message_type == 313)
                        {
                            CloseRequested = true;
                        }
                        break;
                }
            }
        }

        public abstract void SwapBuffers();

        protected abstract X11.XVisualInfo GetVisualInfo(X11.XDisplay display);
        protected abstract void Cleanup();
    }
}