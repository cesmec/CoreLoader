using System;
using System.Runtime.InteropServices;

namespace CoreLoader.Unix.Native
{
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct XEvent
    {
        [FieldOffset(0)]
        public int type;        /* must not be changed; first element */
        [FieldOffset(0)]
        public fixed long pad[24];
    }

    public struct XKeyEvent
    {
        public int type;        /* of event */
        public ulong serial;    /* # of last request processed by server */
        public bool send_event;    /* true if this came from a SendEvent request */
        public IntPtr display;    /* Display the event was read from */
        public ulong window;            /* "event" window it is reported relative to */
        public ulong root;            /* root window that the event occurred on */
        public ulong subwindow;    /* child window */
        public ulong time;        /* milliseconds */
        public int x, y;        /* pointer x, y coordinates in event window */
        public int x_root, y_root;    /* coordinates relative to root */
        public uint state;    /* key or button mask */
        public uint keycode;    /* detail */
        public bool same_screen;    /* same screen flag */
    }

    public struct XButtonEvent
    {
        public int type;        /* of event */
        public ulong serial;    /* # of last request processed by server */
        public bool send_event;    /* true if this came from a SendEvent request */
        public IntPtr display;    /* Display the event was read from */
        public ulong window;            /* "event" window it is reported relative to */
        public ulong root;            /* root window that the event occurred on */
        public ulong subwindow;    /* child window */
        public ulong time;        /* milliseconds */
        public int x, y;        /* pointer x, y coordinates in event window */
        public int x_root, y_root;    /* coordinates relative to root */
        public uint state;    /* key or button mask */
        public uint button;    /* detail */
        public bool same_screen;    /* same screen flag */
    }

    public struct XMotionEvent
    {
        public int type;        /* of event */
        public ulong serial;    /* # of last request processed by server */
        public bool send_event;    /* true if this came from a SendEvent request */
        public IntPtr display;    /* Display the event was read from */
        public ulong window;            /* "event" window reported relative to */
        public ulong root;            /* root window that the event occurred on */
        public ulong subwindow;    /* child window */
        public ulong time;        /* milliseconds */
        public int x, y;        /* pointer x, y coordinates in event window */
        public int x_root, y_root;    /* coordinates relative to root */
        public uint state;    /* key or button mask */
        public char is_hint;        /* detail */
        public bool same_screen;    /* same screen flag */
    }

    public struct XCrossingEvent
    {
        public int type;        /* of event */
        public ulong serial;    /* # of last request processed by server */
        public bool send_event;    /* true if this came from a SendEvent request */
        public IntPtr display;    /* Display the event was read from */
        public ulong window;            /* "event" window reported relative to */
        public ulong root;            /* root window that the event occurred on */
        public ulong subwindow;    /* child window */
        public ulong time;        /* milliseconds */
        public int x, y;        /* pointer x, y coordinates in event window */
        public int x_root, y_root;    /* coordinates relative to root */
        public int mode;        /* NotifyNormal, NotifyGrab, NotifyUngrab */
        public int detail;
        /*
        * NotifyAncestor, NotifyVirtual, NotifyInferior,
        * NotifyNonlinear,NotifyNonlinearVirtual
        */
        public bool same_screen;    /* same screen flag */
        public bool focus;        /* boolean focus */
        public uint state;    /* key or button mask */
    }

    public struct XFocusChangeEvent
    {
        public int type;        /* FocusIn or FocusOut */
        public ulong serial;    /* # of last request processed by server */
        public bool send_event;    /* true if this came from a SendEvent request */
        public IntPtr display;    /* Display the event was read from */
        public ulong window;        /* window of event */
        public int mode;        /* NotifyNormal, NotifyWhileGrabbed, NotifyGrab, NotifyUngrab */
        public int detail;
        /*
        * NotifyAncestor, NotifyVirtual, NotifyInferior,
        * NotifyNonlinear,NotifyNonlinearVirtual, NotifyPointer,
        * NotifyPointerRoot, NotifyDetailNone
        */
    }

    /* generated on Enterulong and FocusIn  when KeyMapState selected */

    public struct XKeymapEvent
    {
        public int type;
        public ulong serial;    /* # of last request processed by server */
        public bool send_event;    /* true if this came from a SendEvent request */
        public IntPtr display;    /* Display the event was read from */
        public ulong window;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string key_vector;
    }

    public struct XExposeEvent
    {
        public int type;
        public ulong serial;    /* # of last request processed by server */
        public bool send_event;    /* true if this came from a SendEvent request */
        public IntPtr display;    /* Display the event was read from */
        public ulong window;
        public int x, y;
        public int width, height;
        public int count;        /* if non-zero, at least this many more */
    }

    public struct XGraphicsExposeEvent
    {
        public int type;
        public ulong serial;    /* # of last request processed by server */
        public bool send_event;    /* true if this came from a SendEvent request */
        public IntPtr display;    /* Display the event was read from */
        public ulong drawable;
        public int x, y;
        public int width, height;
        public int count;        /* if non-zero, at least this many more */
        public int major_code;        /* core is CopyArea or CopyPlane */
        public int minor_code;        /* not defined in the core */
    }

    public struct XNoExposeEvent
    {
        public int type;
        public ulong serial;    /* # of last request processed by server */
        public bool send_event;    /* true if this came from a SendEvent request */
        public IntPtr display;    /* Display the event was read from */
        public ulong drawable;
        public int major_code;        /* core is CopyArea or CopyPlane */
        public int minor_code;        /* not defined in the core */
    }

    public struct XVisibilityEvent
    {
        public int type;
        public ulong serial;    /* # of last request processed by server */
        public bool send_event;    /* true if this came from a SendEvent request */
        public IntPtr display;    /* Display the event was read from */
        public ulong window;
        public int state;        /* Visibility state */
    }

    public struct XCreateWindowEvent
    {
        public int type;
        public ulong serial;    /* # of last request processed by server */
        public bool send_event;    /* true if this came from a SendEvent request */
        public IntPtr display;    /* Display the event was read from */
        public ulong parent;        /* parent of the window */
        public ulong window;        /* window id of window created */
        public int x, y;        /* window location */
        public int width, height;    /* size of window */
        public int border_width;    /* border width */
        public bool override_redirect;    /* creation should be overridden */
    }

    public struct XDestroyWindowEvent
    {
        public int type;
        public ulong serial;    /* # of last request processed by server */
        public bool send_event;    /* true if this came from a SendEvent request */
        public IntPtr display;    /* Display the event was read from */
        public ulong @event;
        public ulong window;
    }

    public struct XUnmapEvent
    {
        public int type;
        public ulong serial;    /* # of last request processed by server */
        public bool send_event;    /* true if this came from a SendEvent request */
        public IntPtr display;    /* Display the event was read from */
        public ulong @event;
        public ulong window;
        public bool from_configure;
    }

    public struct XMapEvent
    {
        public int type;
        public ulong serial;    /* # of last request processed by server */
        public bool send_event;    /* true if this came from a SendEvent request */
        public IntPtr display;    /* Display the event was read from */
        public ulong @event;
        public ulong window;
        public bool override_redirect;    /* boolean, is override set... */
    }

    public struct XMapRequestEvent
    {
        public int type;
        public ulong serial;    /* # of last request processed by server */
        public bool send_event;    /* true if this came from a SendEvent request */
        public IntPtr display;    /* Display the event was read from */
        public ulong parent;
        public ulong window;
    }

    public struct XReparentEvent
    {
        public int type;
        public ulong serial;    /* # of last request processed by server */
        public bool send_event;    /* true if this came from a SendEvent request */
        public IntPtr display;    /* Display the event was read from */
        public ulong @event;
        public ulong window;
        public ulong parent;
        public int x, y;
        public bool override_redirect;
    }

    public struct XConfigureEvent
    {
        public int type;
        public ulong serial;    /* # of last request processed by server */
        public bool send_event;    /* true if this came from a SendEvent request */
        public IntPtr display;    /* Display the event was read from */
        public ulong @event;
        public ulong window;
        public int x, y;
        public int width, height;
        public int border_width;
        public ulong above;
        public bool override_redirect;
    }

    public struct XGravityEvent
    {
        public int type;
        public ulong serial;    /* # of last request processed by server */
        public bool send_event;    /* true if this came from a SendEvent request */
        public IntPtr display;    /* Display the event was read from */
        public ulong @event;
        public ulong window;
        public int x, y;
    }

    public struct XResizeRequestEvent
    {
        public int type;
        public ulong serial;    /* # of last request processed by server */
        public bool send_event;    /* true if this came from a SendEvent request */
        public IntPtr display;    /* Display the event was read from */
        public ulong window;
        public int width, height;
    }

    public struct XConfigureRequestEvent
    {
        public int type;
        public ulong serial;    /* # of last request processed by server */
        public bool send_event;    /* true if this came from a SendEvent request */
        public IntPtr display;    /* Display the event was read from */
        public ulong parent;
        public ulong window;
        public int x, y;
        public int width, height;
        public int border_width;
        public ulong above;
        public int detail;        /* Above, Below, TopIf, BottomIf, Opposite */
        public ulong value_mask;
    }

    public struct XCirculateEvent
    {
        public int type;
        public ulong serial;    /* # of last request processed by server */
        public bool send_event;    /* true if this came from a SendEvent request */
        public IntPtr display;    /* Display the event was read from */
        public ulong @event;
        public ulong window;
        public int place;        /* PlaceOnTop, PlaceOnBottom */
    }

    public struct XCirculateRequestEvent
    {
        public int type;
        public ulong serial;    /* # of last request processed by server */
        public bool send_event;    /* true if this came from a SendEvent request */
        public IntPtr display;    /* Display the event was read from */
        public ulong parent;
        public ulong window;
        public int place;        /* PlaceOnTop, PlaceOnBottom */
    }

    public struct XPropertyEvent
    {
        public int type;
        public ulong serial;    /* # of last request processed by server */
        public bool send_event;    /* true if this came from a SendEvent request */
        public IntPtr display;    /* Display the event was read from */
        public ulong window;
        public ulong atom;
        public ulong time;
        public int state;        /* NewValue, Deleted */
    }

    public struct XSelectionClearEvent
    {
        public int type;
        public ulong serial;    /* # of last request processed by server */
        public bool send_event;    /* true if this came from a SendEvent request */
        public IntPtr display;    /* Display the event was read from */
        public ulong window;
        public ulong selection;
        public ulong time;
    }

    public struct XSelectionRequestEvent
    {
        public int type;
        public ulong serial;    /* # of last request processed by server */
        public bool send_event;    /* true if this came from a SendEvent request */
        public IntPtr display;    /* Display the event was read from */
        public ulong owner;
        public ulong requestor;
        public ulong selection;
        public ulong target;
        public ulong property;
        public ulong time;
    }

    public struct XSelectionEvent
    {
        public int type;
        public ulong serial;    /* # of last request processed by server */
        public bool send_event;    /* true if this came from a SendEvent request */
        public IntPtr display;    /* Display the event was read from */
        public ulong requestor;
        public ulong selection;
        public ulong target;
        public ulong property;        /* ATOM or None */
        public ulong time;
    }

    public struct XColormapEvent
    {
        public int type;
        public ulong serial;    /* # of last request processed by server */
        public bool send_event;    /* true if this came from a SendEvent request */
        public IntPtr display;    /* Display the event was read from */
        public ulong window;
        public ulong colormap;    /* COLORMAP or None */
        public bool @new;
        public int state;        /* ColormapInstalled, ColormapUninstalled */
    }

    public struct XClientMessageEvent
    {
        public int type;
        public ulong serial;    /* # of last request processed by server */
        public bool send_event;    /* true if this came from a SendEvent request */
        public IntPtr display;    /* Display the event was read from */
        public ulong window;
        public ulong message_type;
        public int format;
        public XClientMessageData data;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct XClientMessageData
    {
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] b;
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public short[] s;
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public long[] l;
    }

    public struct XMappingEvent
    {
        public int type;
        public ulong serial;    /* # of last request processed by server */
        public bool send_event;    /* true if this came from a SendEvent request */
        public IntPtr display;    /* Display the event was read from */
        public ulong window;        /* unused */
        public int request;        /* one of MappingModifier, MappingKeyboard, MappingPointer */
        public int first_keycode;    /* first keycode */
        public int count;        /* defines range of change w. first_keycode*/
    }

    public struct XErrorEvent
    {
        public int type;
        public IntPtr display;    /* Display the event was read from */
        public ulong resourceid;        /* resource id */
        public ulong serial;    /* serial number of failed request */
        public byte error_code;    /* error code of failed request */
        public byte request_code;    /* Major op-code of failed request */
        public byte minor_code;    /* Minor op-code of failed request */
    }

    public struct XAnyEvent
    {
        public int type;
        public ulong serial;    /* # of last request processed by server */
        public bool send_event;    /* true if this came from a SendEvent request */
        public IntPtr display;/* Display the event was read from */
        public ulong window;    /* window on which event was requested in event mask */
    }

    public struct XGenericEvent
    {
        public int type;         /* of event. Always GenericEvent */
        public ulong serial;       /* # of last request processed */
        public bool send_event;   /* true if from SendEvent request */
        public IntPtr display;     /* Display the event was read from */
        public int extension;    /* major opcode of extension that caused the event */
        public int evtype;       /* actual event type. */
    }

    public struct XGenericEventCookie
    {
        public int type;         /* of event. Always GenericEvent */
        public ulong serial;       /* # of last request processed */
        public bool send_event;   /* true if from SendEvent request */
        public IntPtr display;     /* Display the event was read from */
        public int extension;    /* major opcode of extension that caused the event */
        public int evtype;       /* actual event type. */
        public uint cookie;
        public IntPtr data;
    }
}