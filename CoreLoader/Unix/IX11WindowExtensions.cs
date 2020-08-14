using CoreLoader.Unix.Native;

namespace CoreLoader.Unix
{
    public interface IX11WindowExtensions : IWindowExtensions
    {
        X11.XVisualInfo GetVisualInfo(X11.XDisplay display);
    }
}