namespace CoreLoader
{
    public interface IExtendableWindow
    {
        INativeWindow NativeWindow { get; }
        void SetWindowExtensions(IWindowExtensions extensions);
    }
}