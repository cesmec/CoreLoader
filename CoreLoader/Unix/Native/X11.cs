using System;
using System.Runtime.InteropServices;

namespace CoreLoader.Unix.Native
{
    public class X11
    {
        public delegate ulong AllocatorFunc(ref XDisplay display);
        public delegate int Private15Func(ref XDisplay display);

        public struct XDisplay
        {
            public IntPtr ext_data;	/* hook for extension to hang data */
            public IntPtr private1;
            public int fd;			/* Network socket. */
            public int private2;
            public int proto_major_version;/* major version of server's X protocol */
            public int proto_minor_version;/* minor version of servers X protocol */
            [MarshalAs(UnmanagedType.LPStr)]
            public string vendor;		/* vendor of the server hardware */
            public ulong private3;
            public ulong private4;
            public ulong private5;
            public int private6;
            public AllocatorFunc resource_alloc;
            public int byte_order;		/* screen byte order, LSBFirst, MSBFirst */
            public int bitmap_unit;	/* padding and data requirements */
            public int bitmap_pad;		/* padding requirements on bitmaps */
            public int bitmap_bit_order;	/* LeastSignificant or MostSignificant */
            public int nformats;		/* number of pixmap formats in list */
            public IntPtr pixmap_format;	/* pixmap format list */
            public int private8;
            public int release;		/* release of the server */
            public IntPtr private9, private10;
            public int qlen;		/* Length of input event queue */
            public ulong last_request_read; /* seq number of last event read */
            public ulong request;	/* sequence number of last request. */
            public IntPtr private11;
            public IntPtr private12;
            public IntPtr private13;
            public IntPtr private14;
            public uint max_request_size; /* maximum number 32 bit words in request*/
            public IntPtr db;
            public Private15Func private15;
            [MarshalAs(UnmanagedType.LPStr)]
            public string display_name;	/* "host:display" string used on this connect*/
            public int default_screen;	/* default screen for operations */
            public int nscreens;		/* number of screens on this server*/
            public IntPtr screens;	/* pointer to list of screens */
            public ulong motion_buffer;	/* size of motion buffer */
            public ulong private16;
            public int min_keycode;	/* minimum defined keycode */
            public int max_keycode;	/* maximum defined keycode */
            public IntPtr private17;
            public IntPtr private18;
            public int private19;
            public IntPtr xdefaults;	/* contents of defaults from server */
            /* there is more to this structure, but it is private to Xlib */
        }

        public struct Screen
        {
            public IntPtr ext_data;	/* hook for extension to hang data */
            public IntPtr display;/* back pointer to display structure */
            public ulong root;		/* Root window id. */
            public int width, height;	/* width and height of screen */
            public int mwidth, mheight;	/* width and height of  in millimeters */
            public int ndepths;		/* number of depths possible */
            public IntPtr depths;		/* list of allowable depths on the screen */
            public int root_depth;		/* bits per pixel */
            public IntPtr root_visual;	/* root visual */
            public IntPtr default_gc;		/* GC for the root root visual */
            public ulong cmap;		/* default color map */
            public ulong white_pixel;
            public ulong black_pixel;	/* White and Black pixel values */
            public int max_maps, min_maps;	/* max and min color maps */
            public int backing_store;	/* Never, WhenMapped, Always */
            public int save_unders;
            public long root_input_mask;	/* initial root input mask */
        }

        public struct XVisualInfo
        {
            public IntPtr visual;
            public ulong visualid;
            public int screen;
            public int depth;
            public int @class;
            public ulong red_mask;
            public ulong green_mask;
            public ulong blue_mask;
            public int colormap_size;
            public int bits_per_rgb;
        }

        public struct Visual
        {
            public IntPtr ext_data;	/* hook for extension to hang data */
            public ulong visualid;	/* visual id of this visual */
            public int @class;		/* class of screen (monochrome, etc.) */
            public ulong red_mask, green_mask, blue_mask;	/* mask values */
            public int bits_per_rgb;	/* log base 2 of distinct color values */
            public int map_entries;	/* color map entries */
        }

        public struct XSetWindowAttributes
        {
            public ulong background_pixmap;	/* background or None or ParentRelative */
            public ulong background_pixel;	/* background pixel */
            public ulong border_pixmap;	/* border of the window */
            public ulong border_pixel;	/* border pixel value */
            public int bit_gravity;		/* one of bit gravity values */
            public int win_gravity;		/* one of the window gravity values */
            public int backing_store;		/* NotUseful, WhenMapped, Always */
            public ulong backing_planes;/* planes to be preseved if possible */
            public ulong backing_pixel;/* value to use in restoring planes */
            public int save_under;		/* should bits under be saved? (popups) */
            public long event_mask;		/* set of events that should be saved */
            public long do_not_propagate_mask;	/* set of events that should not propagate */
            public int override_redirect;	/* boolean value for override-redirect */
            public ulong colormap;		/* color map to be associated with window */
            public ulong cursor;		/* cursor to be displayed (or None) */
        }

        public struct XColor
        {
            public ulong pixel;
            public ushort red, green, blue;
            public byte flags;  /* do_red, do_green, do_blue */
            public byte pad;
        }

        private const string LibName = "libX11";

        [DllImport(LibName, ExactSpelling = true)]
        public static extern IntPtr XOpenDisplay([MarshalAs(UnmanagedType.LPStr)] string displayName);
        [DllImport(LibName, ExactSpelling = true)]
        public static extern ulong XCreateColormap(IntPtr display, ulong root, ref Visual visual, int alloc);
        [DllImport(LibName, ExactSpelling = true)]
        public static extern uint XCreateWindow(IntPtr display, ulong parent, int x, int y, uint width, uint height, uint borderWidth, int depth, uint @class, ref Visual visual, ulong valueMask, ref XSetWindowAttributes attributes);
        [DllImport(LibName, ExactSpelling = true)]
        public static extern int XMapWindow(IntPtr display, ulong window);
        [DllImport(LibName, ExactSpelling = true)]
        public static extern int XStoreName(IntPtr display, ulong window, string title);
        [DllImport(LibName, ExactSpelling = true)]
        public static extern int XDestroyWindow(IntPtr display, ulong window);
        [DllImport(LibName, ExactSpelling = true)]
        public static extern int XCloseDisplay(IntPtr display);
        [DllImport(LibName, ExactSpelling = true)]
        public static extern ulong XInternAtom(IntPtr display, string atomName, bool onlyIfExists);
        [DllImport(LibName, ExactSpelling = true)]
        public static extern int XSetWMProtocols(IntPtr display, ulong window, ulong[] protocols, int count);

        //keys
        [DllImport(LibName, ExactSpelling = true)]
        public static extern uint XKeysymToKeycode(IntPtr display, uint keysym);
        [DllImport(LibName, ExactSpelling = true)]
        public static extern uint XKeycodeToKeysym(IntPtr display, uint keycode, int index);
        [DllImport(LibName, ExactSpelling = true)]
        public static extern void XQueryKeymap(IntPtr display, byte[] keys_return);
        [DllImport(LibName, ExactSpelling = true)]
        public static extern string XKeysymToString(uint keysym);
        [DllImport(LibName, ExactSpelling = true)]
        public static extern uint XStringToKeysym(string name);

        //pointer
        [DllImport(LibName, ExactSpelling = true)]
        public static extern bool XQueryPointer(IntPtr display, ulong w, out ulong root_return, out ulong child_return, out int root_x_return, out int root_y_return, out int win_x_return, out int win_y_return, out uint mask_return);
        [DllImport(LibName, ExactSpelling = true)]
        public static extern void XWarpPointer(IntPtr display, ulong src_w, ulong dest_w, int src_x, int src_y, uint src_width, uint src_height, int dest_x, int dest_y);
        [DllImport(LibName, ExactSpelling = true)]
        public static extern uint XCreateBitmapFromData(IntPtr display, ulong window, byte[] data, uint width, uint height);
        [DllImport(LibName, ExactSpelling = true)]
        public static extern uint XCreatePixmapCursor(IntPtr display, uint bitmap, uint bitmap_mask, ref XColor foreground, ref XColor background, uint x, uint y);
        [DllImport(LibName, ExactSpelling = true)]
        public static extern void XDefineCursor(IntPtr display, uint window, uint cursor);
        [DllImport(LibName, ExactSpelling = true)]
        public static extern void XFreeCursor(IntPtr display, uint cursor);
        [DllImport(LibName, ExactSpelling = true)]
        public static extern void XFreePixmap(IntPtr display, uint pixmap);
        [DllImport(LibName, ExactSpelling = true)]
        public static extern void XUndefineCursor(IntPtr display, ulong window);

        //event handling
        [DllImport(LibName, ExactSpelling = true)]
        public static extern bool XPending(IntPtr display);
        [DllImport(LibName, ExactSpelling = true)]
        public static extern int XNextEvent(IntPtr display, IntPtr @event);
    }
}