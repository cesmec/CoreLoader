using System;
using CoreLoader.Events;
using CoreLoader.Input;

namespace CoreLoader
{
    public interface IWindow : IExtendableWindow
    {
        int Width { get; }
        int Height { get; }
        bool CloseRequested { get; }
        IKeys Keys { get; }

        event EventHandler<KeyEventArgs> OnKeyDown;
        event EventHandler<KeyEventArgs> OnKeyUp;
        event EventHandler<ClickEventArgs> OnClick;
        event EventHandler<MouseMoveEventArgs> OnMouseMove;
        event EventHandler<ScrollEventArgs> OnScroll;
        event EventHandler<ResizeEventArgs> OnResize;
        event EventHandler<FocusChangeEventArgs> OnFocusChange;

        bool IsKeyPressed(uint key);
        bool GetCursorPosition(out Point position);
        void SetCursorPosition(in Point position);
        void SetCursorVisible(bool visible);

        void SetTitle(string title);
        void PollEvents();
        void SwapBuffers();
        void Show();
        void Close();
        void SetCloseRequested();
    }
}