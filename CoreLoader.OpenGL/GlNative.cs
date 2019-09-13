namespace CoreLoader.OpenGL
{
    public unsafe partial class GlNative
    {
        public delegate void DebugProc(uint source, uint type, uint id, uint severity, int length, string message, void* userParam);
        public delegate void DebugMessageCallbackFunc(DebugProc callback, void* userParam);

        public static DebugMessageCallbackFunc DebugMessageCallback;
    }
}