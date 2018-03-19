using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserGuiLib.Common.Algebra;
using UserGuiLib.Common.Component;
using UserGuiLib.Common.Drawing;
using UserGuiLib.Common.Impl;
using UserGuiLib.Common.Services.Layout;
using UserGuiLib.Common.Services.MouseEvents;
using UserGuiLib.Common.Services.Render;

namespace UserGuiLib.ExampleCommon.ColorWheel
{
    public class ColorPicker : Component, ILayout
    {
        private Slider hueSlider;
        private ColorBox colorBox;

        public ColorPicker()
        {
            var mouseHandler = new MouseHandler();
            RegisterService<ILayout>(this);
            RegisterService<IMouseEvents>(mouseHandler);
            RegisterService<IMouseWheel>(mouseHandler);

            hueSlider = new Slider();

            hueSlider.Transform.Location = new Vector2(0, 0);
            hueSlider.Transform.Size = new Vector2(20, 0);
            
            colorBox = new ColorBox();
            
            hueSlider.Value.OnChange += (oldVal, newVal) => colorBox.Hue = newVal * 360;

            Transform.AddChild(hueSlider);
            Transform.AddChild(colorBox);
        }

        public void Relayout(IGraphics g)
        {
            hueSlider.Transform.Size = new Vector2(30, Transform.Size.y);

            colorBox.Transform.Location = new Vector2(hueSlider.Transform.Size.x, 0);
            colorBox.Transform.Size = new Vector2(Transform.Size.x - hueSlider.Transform.Size.x, Transform.Size.y);
        }
       
    }
}
