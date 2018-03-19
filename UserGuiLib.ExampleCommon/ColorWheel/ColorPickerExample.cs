using UserGuiLib.Common.Algebra;
using UserGuiLib.Common.Component;
using UserGuiLib.Common.Drawing;
using UserGuiLib.Common.Impl;
using UserGuiLib.Common.Services.Layout;
using UserGuiLib.Common.Services.MouseEvents;
using UserGuiLib.Components;

namespace UserGuiLib.ExampleCommon.ColorWheel
{
    public class ColorPickerExample : Component, ILayout
    {
        private Label label;
        private ColorPicker picker;

        public ColorPickerExample()
        {
            var mouse = new MouseHandler();
            RegisterService<IMouseEvents>(mouse);
            RegisterService<IMouseWheel>(mouse);
            RegisterService<ILayout>(this);
            
            label = new Label
            {
                Text = "This is simple example showing 'fake' shader to generate color wheel. Fake, because it is rendered by CPU, so the performance will be low",
                Padding = Vector2.One * 40
            };

            picker = new ColorPicker();

            Transform.AddChild(label);
            Transform.AddChild(picker);
        }

        public void Relayout(IGraphics g)
        {
            Transform.Size = Transform.Parent.Size;

            label.GetService<ILayout>().Relayout(g);

            picker.Transform.Location = new Vector2(0, label.Transform.Size.y + 10);
            picker.Transform.Size = Transform.Size - new Vector2(0, picker.Transform.Location.y);

            picker.GetService<ILayout>().Relayout(g);
        }
    }
}
