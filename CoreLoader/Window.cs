using System;
using CoreLoader.Events;
using CoreLoader.Input;

namespace CoreLoader
{
    public class Window : IWindow
    {
        private readonly INativeWindow _nativeWindow;
        private IWindowExtensions _extensions;

        public int Width => _nativeWindow.Width;
        public int Height => _nativeWindow.Height;
        public bool CloseRequested => _nativeWindow.CloseRequested;
        public IKeys Keys => _nativeWindow.Keys;

        INativeWindow IExtendableWindow.NativeWindow => _nativeWindow;

        public event EventHandler<KeyEventArgs> OnKeyDown
        {
            add => _nativeWindow.OnKeyDown += value;
            remove => _nativeWindow.OnKeyDown -= value;
        }
        public event EventHandler<KeyEventArgs> OnKeyUp
        {
            add => _nativeWindow.OnKeyUp += value;
            remove => _nativeWindow.OnKeyUp -= value;
        }
        public event EventHandler<ClickEventArgs> OnClick
        {
            add => _nativeWindow.OnClick += value;
            remove => _nativeWindow.OnClick -= value;
        }
        public event EventHandler<MouseMoveEventArgs> OnMouseMove
        {
            add => _nativeWindow.OnMouseMove += value;
            remove => _nativeWindow.OnMouseMove -= value;
        }
        public event EventHandler<ScrollEventArgs> OnScroll
        {
            add => _nativeWindow.OnScroll += value;
            remove => _nativeWindow.OnScroll -= value;
        }
        public event EventHandler<ResizeEventArgs> OnResize
        {
            add => _nativeWindow.OnResize += value;
            remove => _nativeWindow.OnResize -= value;
        }
        public event EventHandler<FocusChangeEventArgs> OnFocusChange
        {
            add => _nativeWindow.OnFocusChange += value;
            remove => _nativeWindow.OnFocusChange -= value;
        }

        public Window(string title, int width, int height)
        {
            _nativeWindow = NativeHelper.CreateWindow(title, width, height);
        }

        public bool GetCursorPosition(out Point position) => _nativeWindow.GetCursorPosition(out position);
        public KeyState GetKeyState(uint key) => _nativeWindow.GetKeyState(key);
        public void PollEvents() => _nativeWindow.PollEvents();
        public void SetCloseRequested() => _nativeWindow.SetCloseRequested();
        public void SetCursorPosition(in Point position) => _nativeWindow.SetCursorPosition(position);
        public void SetCursorVisible(bool visible) => _nativeWindow.SetCursorVisible(visible);
        public void SetTitle(string title) => _nativeWindow.SetTitle(title);

        public void Show()
        {
            _nativeWindow.Show();
            _extensions?.OnShow();
        }

        public void Close()
        {
            _extensions?.Cleanup();
            _nativeWindow.Close();
        }

        public void SwapBuffers() => _extensions?.SwapBuffers();

        void IExtendableWindow.SetWindowExtensions(IWindowExtensions extensions)
        {
            if (_extensions != null)
                throw new InvalidOperationException("Extensions have already been set");

            _extensions = extensions;
            _nativeWindow.SetWindowExtensions(extensions);
        }
    }
}