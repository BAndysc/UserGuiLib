using ObservableObjects;
using UserGuiLib.Common.Algebra;
using UserGuiLib.Common.Component;
using UserGuiLib.Common.Drawing;
using UserGuiLib.Common.Impl;
using UserGuiLib.Common.Services.MouseEvents;
using UserGuiLib.Common.Services.Render;

namespace UserGuiLib.ExampleCommon.ColorWheel
{
    public class Slider : Component, IRenderer, IMouseEvents, IMouseWheel, IPixelShader
    {
        private Rect slider;

        public ObservableProperty<float> Value;

        public Slider()
        {
            RegisterService<IRenderer>(this);
            RegisterService<IMouseEvents>(this);
            RegisterService<IMouseWheel>(this);
            RegisterService<IPixelShader>(this);

            Value = new ObservableProperty<float>(0, (v) => v.Clamp(0, 1));
        }

        public void Render(IGraphics graphics)
        {
            var p1 = new Vector2(Transform.Size.x/2, System.Math.Min(Value.get() * Transform.Size.y + 6, Transform.Size.y - 10));

            slider = new Rect(p1, new Vector2(Transform.Size.x, 10));

            graphics.DrawCircle(new AnyPen(255, 255, 255, 255, 1), p1, Vector2.Half, 5);
            graphics.DrawCircle(new AnyPen(255, 0,0,0, 1), p1, Vector2.Half, 6);
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

        public int PixelShader(uint screenX, uint screenY, float x, float y)
        {
            return ColorScale.HsvToRgb(y / Transform.Size.y * 360, 1, 1);
        }
    }
}
