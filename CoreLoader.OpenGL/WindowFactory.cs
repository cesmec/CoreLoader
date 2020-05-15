using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using CoreLoader.OpenGL.Attributes;
using CoreLoader.OpenGL.Unix;
using CoreLoader.OpenGL.Windows;

namespace CoreLoader.OpenGL
{
    public static class WindowFactory
    {
        private static readonly INativeHelper _nativeHelper = GetNativeHelper();

        public static event EventHandler<ICollection<FunctionLoadError>> OnLoadError;

        public static IWindow CreateWindow(string title, int width, int height)
        {
            var window = _nativeHelper.CreateWindow(title, width, height);

            var loadErrors = LoadFunctions<GlNative>();
            if (loadErrors.Any())
            {
                OnLoadError?.Invoke(null, loadErrors);
            }

            return window;
        }

        public static ICollection<FunctionLoadError> LoadFunctions<T>() => LoadFunctions(typeof(T));

        public static ICollection<FunctionLoadError> LoadFunctions(Type type)
        {
            var loadErrors = new List<FunctionLoadError>();

            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var field in fields)
            {
                var functionName = GetFunctionName(field);
                try
                {
                    var handle = _nativeHelper.GetFunctionPtr(functionName);
                    var function = Marshal.GetDelegateForFunctionPointer(handle, field.FieldType);
                    field.SetValue(null, function);
                }
                catch (Exception e)
                {
                    loadErrors.Add(new FunctionLoadError(e, functionName));
                }
            }

            return loadErrors;
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