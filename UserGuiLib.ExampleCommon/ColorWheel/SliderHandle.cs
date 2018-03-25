using ObservableObjects;
using UserGuiLib.Common.Algebra;
using UserGuiLib.Common.Drawing;
using UserGuiLib.Common.Impl;
using UserGuiLib.Common.Services.Layout;
using UserGuiLib.Common.Services.MouseEvents;
using UserGuiLib.Common.Services.Render;

namespace UserGuiLib.ExampleCommon.ColorWheel
{
    public class SliderHandle : Component, IMouseEvents, IRenderer, ILayout
    {
        private readonly ObservableProperty<float> value;

        public SliderHandle(ObservableProperty<float> value)
        {
            RegisterService<IRenderer>(this);
            RegisterService<IMouseEvents>(this);
            RegisterService<ILayout>(this);

            Transform.Size = Vector2.One * 10;
            Transform.Pivot = Vector2.Half;
            Transform.Anchor = new Vector2(0.5f, 0);
            this.value = value;
        }

        public void Render(IGraphics graphics)
        {
            if (mouseHover)
                graphics.FillCircle(new AnyPen(120, 255, 255, 255, 1), Vector2.Zero, Vector2.Zero, 5);

            graphics.DrawCircle(new AnyPen(255, 255, 255, 255, 1), Vector2.Zero, Vector2.Zero, 5);
            graphics.DrawCircle(new AnyPen(255, 0, 0, 0, 1), Vector2.One*-1, Vector2.Zero, 6);
        }

        public void Relayout(IGraphics g)
        {
            Transform.Location = new Vector2(0, value.get() * Transform.Parent.Size.y);
        }

        private bool mouseDown = false;
        private bool mouseHover = false;
        private Vector2 mouseStartPosition;
        private Vector2 startLocation;

        public void MouseMove(MouseButtons buttons, Vector2 cursor)
        {
            if (mouseDown)
                value.set((startLocation + Transform.PointToParent(cursor) - mouseStartPosition).y / Transform.Parent.Size.y);
        }

        public void MouseDown(MouseButtons buttons, Vector2 cursor)
        {
            mouseDown = true;
            mouseStartPosition = Transform.PointToParent(cursor);
            startLocation = Transform.Location;
        }

        public void MouseUp(MouseButtons buttons, Vector2 cursor)
        {
            mouseDown = false;
        }

        public void MouseEnter()
        {
            mouseHover = true;
        }

        public void MouseExit()
        {
            if (!mouseDown)
                mouseHover = false;
        }
    }
}
