namespace CoreLoader
{
    public interface IWindowExtensions
    {
        void OnShow();
        void SwapBuffers();
        void Cleanup();
    }
}