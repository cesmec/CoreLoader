using System;
using System.Reflection;
using System.Runtime.InteropServices;
using CoreLoader.OpenGL.Events;
using CoreLoader.OpenGL.Unix;
using CoreLoader.OpenGL.Windows;

namespace CoreLoader.OpenGL
{
    public static class WindowFactory
    {
        public static event EventHandler<LoadErrorEvent> OnLoadError;

        public static IWindow CreateWindow(string title, int width, int height)
        {
            var helper = GetNativeHelper();
            var window = helper.CreateWindow(title, width, height);

            InitOpenGL(helper);

            return window;
        }

        private static void InitOpenGL(INativeHelper nativeHelper)
        {
            var fields = typeof(GlNative).GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var field in fields)
            {
                var functionName = GetFunctionName(field.Name);
                try
                {
                    var handle = nativeHelper.GetFunctionPtr(functionName);
                    var function = Marshal.GetDelegateForFunctionPointer(handle, field.FieldType);
                    field.SetValue(null, function);
                }
                catch (Exception e)
                {
                    OnLoadError?.Invoke(null, new LoadErrorEvent(e, functionName));
                }
            }
        }

        private static string GetFunctionName(string fieldName) => $"gl{fieldName}";

        private static INativeHelper GetNativeHelper()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.MacOSX:
                    throw new PlatformNotSupportedException();
                case PlatformID.Unix:
                    if (!Environment.Is64BitOperatingSystem)
                        throw new PlatformNotSupportedException($"Only 64-bit unix systems are supported");
                    return new UnixNativeHelper();
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
                case PlatformID.Xbox:
                    return new WindowsNativeHelper();
                default:
                    throw new PlatformNotSupportedException();
            }
        }
    }
}