using System.Collections.Generic;

namespace CoreLoader.OpenGL
{
    internal interface IOpenGLWindowExtensions : IWindowExtensions
    {
        IReadOnlyList<string> GetPlatformExtensions();
    }
}