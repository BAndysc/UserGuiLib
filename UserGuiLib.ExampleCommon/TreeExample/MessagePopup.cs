using UserGuiLib.Common.Algebra;
using UserGuiLib.Common.Component;
using UserGuiLib.Common.Drawing;
using UserGuiLib.Common.Impl;
using UserGuiLib.Common.Services.Layout;
using UserGuiLib.Common.Services.Render;
using UserGuiLib.Components;

namespace UserGuiLib.ExampleCommon.TreeExample
{
    public class MessagePopup : Component, IRenderer
    {
        private Label label;
        
        private System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

        private readonly float time;

        private Vector2 margin = new Vector2(30, 20);

        public event System.Action TimePassed = delegate { };

        public MessagePopup(string text, float time)
        {
            RegisterService<IRenderer>(this);

            label = new Label
            {
                Text = text,
            };
            label.Transform.Pivot = Vector2.Half;
            label.Transform.Anchor = new Anchor(Vector2.Half, Vector2.Half);
            label.Transform.Size = new Vector2(200, 150);

            sw.Start();

            Transform.Anchor = new Anchor(Vector2.Zero, Vector2.One);

            Transform.AddChild(label);
            this.time = time;
        }

        public void Render(IGraphics graphics)
        {
            if (sw.ElapsedMilliseconds > time * 1000)
            {
                TimePassed();
                sw.Stop();
                sw.Reset();
            }

            label.GetService<ILayout>().Relayout(graphics);

            graphics.FillRectangle(new AnyPen(200, 0, 0, 0, 1), Vector2.Zero, Transform.Size);
            
            graphics.FillRectangle(new AnyPen(255, 255, 255, 255, 1), label.Transform.TopLeftPoint - margin, label.Transform.BottomRightPoint + margin);
        }
    }
}
