using UserGuiLib.Common.Algebra;
using UserGuiLib.Common.Component;
using UserGuiLib.Common.Drawing;
using UserGuiLib.Common.Impl;
using UserGuiLib.Common.Services.Layout;
using UserGuiLib.Common.Services.MouseEvents;
using UserGuiLib.Common.Services.Render;

namespace UserGuiLib.Components
{
    public class ListViewItem<T> : Component, ILayout, IRenderer
    {
        private T obj;

        private AnyFont font;
        private AnyPen normalBg;
        private AnyPen hoverBg;

        private bool over = false;
        
        public Vector2 Padding;

        public ListViewItem(T obj)
        {
            this.obj = obj;

            Padding = new Vector2(20, 50);

            font = new AnyFont("Microsoft Sans Serif", 9);
            normalBg = new AnyPen(255, 0, 0, 0, 1);
            hoverBg = new AnyPen(255, 255, 255, 255, 1);

            var mouse = new MouseHandler();
            RegisterService<IMouseEvents>(mouse);
            RegisterService<IRenderer>(this);
            RegisterService<ILayout>(this);

            mouse.OnMouseEnter += Mouse_OnMouseEnter;
            mouse.OnMouseExit += Mouse_OnMouseExit;
        }

        private void Mouse_OnMouseExit()
        {
            over = false;
        }

        private void Mouse_OnMouseEnter()
        {
            over = true;
        }

        public void Render(IGraphics g)
        {
            if (over)
                g.FillRectangle(new AnyPen(255, 0, 120, 215, 1), Vector2.Zero, Transform.Size);

            g.DrawString(obj.ToString(), over ? hoverBg : normalBg, font, new Vector2(20, Transform.Size.y / 2), new Vector2(0, 0.5f));
        }

        public void Relayout(IGraphics g)
        {
            Transform.Size = g.MeasureString(obj.ToString(), font) + Padding;
        }
    }
}
