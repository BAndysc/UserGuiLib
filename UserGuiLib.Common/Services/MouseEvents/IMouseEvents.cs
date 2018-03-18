using System;
using UserGuiLib.Common.Algebra;

namespace UserGuiLib.Common.Services.MouseEvents
{
    [Flags]
    public enum MouseButtons
    {
        None = 0,
        Left = 1,
        Right = 2,
        Middle = 4,
        Any = Left | Right | Middle
    }

    public interface IMouseEvents : IService
    {
        void MouseDown(MouseButtons buttons, Vector2 cursor);
        void MouseUp(MouseButtons buttons, Vector2 cursor);
        void MouseMove(MouseButtons buttons, Vector2 cursor);
        void MouseEnter();
        void MouseExit();
    }
}
