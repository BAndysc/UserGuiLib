using UserGuiLib.Common.Algebra;
using UserGuiLib.Common.Component;
using UserGuiLib.Common.Drawing;
using UserGuiLib.Common.Impl;
using UserGuiLib.Common.Services.Layout;
using UserGuiLib.Common.Services.Render;
using UserGuiLib.Components;

namespace UserGuiLib.ExampleCommon.TreeExample
{
    public class MessagePopup : Component, IRenderer, ILayout
    {
        private Label label;
        
        private System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

        private readonly float time;

        private Vector2 margin = new Vector2(30, 20);

        public event System.Action TimePassed = delegate { };

        public MessagePopup(string text, float time)
        {
            RegisterService<IRenderer>(this);
            RegisterService<ILayout>(this);

            label = new Label();
            label.Text = text;

            sw.Start();

            Transform.AddChild(label);
            this.time = time;
        }

        public void Relayout(IGraphics g)
        {
            Transform.Size = Transform.Parent.Size;

            label.GetService<ILayout>().Relayout(g);

            label.Transform.Location = Transform.Size / 2 - label.Transform.Size;

            if (sw.ElapsedMilliseconds > time * 1000)
            {
                TimePassed();
                sw.Stop();
                sw.Reset();
            }
        }

        public void Render(IGraphics graphics)
        {
            graphics.FillRectangle(new AnyPen(200, 0, 0, 0, 1), Vector2.Zero, Transform.Size);


            graphics.FillRectangle(new AnyPen(255, 255, 255, 255, 1), label.Transform.Location - margin, label.Transform.Location + label.Transform.Size + margin);
        }
    }
}
