namespace CoreLoader
{
    public interface IExtendableWindow
    {
        INativeWindow NativeWindow { get; }
        IWindowExtensions WindowExtensions { get; set; }
    }
}