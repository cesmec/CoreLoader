using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using CoreLoader.OpenGL.Attributes;
using CoreLoader.OpenGL.Unix;
using CoreLoader.OpenGL.Windows;

namespace CoreLoader.OpenGL
{
    public static class WindowExtensions
    {
        private static readonly List<string> MissingOpenGLFunctions = new List<string>();
        private static INativeHelper Helper;
        private static INativeHelper NativeHelper => Helper ??= GetNativeHelper();

        public static void UseOpenGL(this IWindow window)
        {
            var extensions = NativeHelper.GetWindowExtensions(window.NativeWindow);
            window.SetWindowExtensions(extensions);
        }

        public static IReadOnlyList<string> GetMissingOpenGLFunctionNames(this IWindow _) => MissingOpenGLFunctions;

        public static void LoadOpenGLFunctions<T>(this IWindow _)
        {
            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var field in fields)
            {
                var functionName = GetFunctionName(field);

                var handle = NativeHelper.GetFunctionPtr(functionName);
                if (handle == IntPtr.Zero)
                {
                    MissingOpenGLFunctions.Add(functionName);
                }
                else
                {
                    var function = Marshal.GetDelegateForFunctionPointer(handle, field.FieldType);
                    field.SetValue(null, function);
                }
            }
        }

        internal static void LoadDefaultOpenGLFunctions() => LoadOpenGLFunctions<GlNative>(null);

        internal static void Cleanup()
        {
            Helper?.Dispose();
        }

        private static string GetFunctionName(FieldInfo field)
        {
            var functionNameAttribute = field.GetCustomAttribute<OpenGLFunctionAttribute>();
            return functionNameAttribute?.Name ?? $"gl{field.Name}";
        }

        private static INativeHelper GetNativeHelper()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                    if (!Environment.Is64BitOperatingSystem)
                        throw new PlatformNotSupportedException($"Only 64-bit unix systems are supported");
                    return new UnixNativeHelper();

                case PlatformID.Win32NT:
                    return new WindowsNativeHelper();

                default:
                    throw new PlatformNotSupportedException();
            }
        }
    }
}