using UserGuiLib.Common.Algebra;
using UserGuiLib.Common.Component;
using UserGuiLib.Common.Drawing;
using UserGuiLib.Common.Impl;
using UserGuiLib.Common.Services.Layout;
using UserGuiLib.Common.Services.Render;

namespace UserGuiLib.Components
{
    public class Label : Component, IRenderer, ILayout
    {
        public IComponent Owner { get; set; }

        public string Text;

        public Vector2 Padding;

        public AnyPen Pen;

        public AnyFont Font;

        public Label()
        {
            RegisterService<ILayout>(this);
            RegisterService<IRenderer>(this);

            Font = DefaultFont;
            Pen = DefaultPen;
            Padding = new Vector2(5, 5);
        }

        public void Relayout(IGraphics g)
        {
            if (Text != null)
                Transform.Size = g.MeasureString(Text, Font, Transform.Parent.Size.x) + Padding*2;
        }

        public void Render(IGraphics graphics)
        {
            if (Text != null)
                graphics.DrawString(Text, Pen, Font, Padding, Vector2.Zero, Transform.Parent.Size.x - Padding.x);
        }
    }
}
