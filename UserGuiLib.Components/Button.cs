using UserGuiLib.Common.Algebra;
using UserGuiLib.Common.Component;
using UserGuiLib.Common.Drawing;
using UserGuiLib.Common.Impl;
using UserGuiLib.Common.Services.Layout;
using UserGuiLib.Common.Services.MouseEvents;
using UserGuiLib.Common.Services.Render;

namespace UserGuiLib.Components
{
    public class Button : Component, IRenderer, ILayout
    {
        public string Text { get; set; }

        public event System.Action OnClick = delegate { };

        private MouseHandler mouse;

        private AnyPen borderPenNormal = new AnyPen(255, 104, 111, 122, 1);
        private AnyPen borderPenHover = new AnyPen(255, 104, 111, 122, 1);
        private AnyPen borderPenDown = new AnyPen(255, 104, 111, 122, 1);

        private AnyPen backgroundPenNormal = new AnyPen(255, 212, 216, 221, 1);
        private AnyPen backgroundPenHover = new AnyPen(255, 203, 208, 214, 1);
        private AnyPen backgroundPenDown = new AnyPen(255, 174, 180, 186, 1);

        private AnyPen textPenNormal = new AnyPen(255, 0, 0, 0, 1);
        private AnyPen textPenHover = new AnyPen(255, 0, 0, 0, 1);
        private AnyPen textPenDown = new AnyPen(255, 0, 0, 0, 1);

        private AnyFont textFont = new AnyFont("Calibri", 12);

        private AnyPen borderPen
        {
            get
            {
                if (mouse.IsLeftDown)
                    return borderPenDown;
                if (mouse.IsOver)
                    return borderPenHover;
                return borderPenNormal;
            }
        }

        private AnyPen backgroundPen
        {
            get
            {
                if (mouse.IsLeftDown)
                    return backgroundPenDown;
                if (mouse.IsOver)
                    return backgroundPenHover;
                return backgroundPenNormal;
            }
        }

        private AnyPen textPen
        {
            get
            {
                if (mouse.IsLeftDown)
                    return textPenDown;
                if (mouse.IsOver)
                    return textPenHover;
                return textPenNormal;
            }
        }

        public Button()
        {
            mouse = new MouseHandler();
            RegisterService<IMouseEvents>(mouse);
            RegisterService<IRenderer>(this);
            RegisterService<ILayout>(this);

            mouse.OnMouseDown += Mouse_OnMouseDown;
        }

        private void Mouse_OnMouseDown(MouseButtons arg1, Vector2 arg2)
        {
            OnClick();
        }

        public Button(string text) : this()
        {
            Text = text;
        }

        public void Render(IGraphics graphics)
        {
            graphics.FillRectangle(backgroundPen, Vector2.Zero, Transform.Size);
            graphics.DrawRectangle(borderPen, Vector2.Zero, Transform.Size);
            graphics.DrawString(Text, textPen, textFont, Transform.Size / 2, Vector2.Half);
        }

        public void Relayout(IGraphics g)
        {
            var measure = g.MeasureString(Text, textFont);

            Transform.Size = measure + new Vector2(20, 5);
        }
    }
}
