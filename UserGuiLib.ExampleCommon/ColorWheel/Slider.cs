using ObservableObjects;
using UserGuiLib.Common.Algebra;
using UserGuiLib.Common.Component;
using UserGuiLib.Common.Drawing;
using UserGuiLib.Common.Impl;
using UserGuiLib.Common.Services.MouseEvents;
using UserGuiLib.Common.Services.Render;

namespace UserGuiLib.ExampleCommon.ColorWheel
{
    public class Slider : Component, IRenderer, IMouseEvents, IMouseWheel
    {
        private Rect slider;

        public ObservableProperty<float> Value;

        public Slider()
        {
            RegisterService<IRenderer>(this);
            RegisterService<IMouseEvents>(this);
            RegisterService<IMouseWheel>(this);

            Value = new ObservableProperty<float>(0, (v) => v.Clamp(0, 1));
        }

        public void Render(IGraphics graphics)
        {
            graphics.DrawLine(new AnyPen(255, 0, 0, 0, 2), new Vector2(Transform.Size.x / 2, 0), new Vector2(Transform.Size.x / 2, Transform.Size.y));

            var p1 = new Vector2(0, System.Math.Min(Value.get() * Transform.Size.y, Transform.Size.y - 10));

            slider = new Rect(p1, new Vector2(Transform.Size.x, 10));

            graphics.FillRectangle(new AnyPen(255, 0, 0, 0, 1), p1, p1 + new Vector2(Transform.Size.x, 10));
        }

        private bool down = false;

        public void MouseDown(MouseButtons buttons, Vector2 cursor)
        {
            if (slider.Contains(cursor))
                down = true;
            else
            {
                if (cursor.y > slider.Bottom)
                    Value.set(Value.get() + 0.1f);
                else
                    Value.set(Value.get() - 0.1f);
            }
        }

        public void MouseUp(MouseButtons buttons, Vector2 cursor)
        {
            down = false;
        }

        public void MouseMove(MouseButtons buttons, Vector2 cursor)
        {
            if (down)
            {
                var offset = cursor.y / Transform.Size.y;
                Value.set(offset);
            }
        }

        public void MouseEnter()
        {
        }

        public void MouseExit()
        {
        }

        public void MouseWheel(float delta)
        {
            if (delta > 0)
            {
                Value.set(Value.get() - 0.1f);
            }
            else if (delta < 0)
            {
                Value.set(Value.get() + 0.1f);
            }
        }
    }
}
