using System.Collections.Generic;

namespace CoreLoader.OpenGL
{
    public interface IOpenGLWindowExtensions : IWindowExtensions
    {
        IReadOnlyList<string> GetPlatformExtensions();
    }
}