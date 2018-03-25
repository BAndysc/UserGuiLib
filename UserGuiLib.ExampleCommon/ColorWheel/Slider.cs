using ObservableObjects;
using UserGuiLib.Common.Algebra;
using UserGuiLib.Common.Impl;
using UserGuiLib.Common.Services.Layout;
using UserGuiLib.Common.Services.MouseEvents;
using UserGuiLib.Common.Services.Render;

namespace UserGuiLib.ExampleCommon.ColorWheel
{
    public class Slider : Component, IMouseWheel, IPixelShader
    {
        public ObservableProperty<float> Value;
        
        public Slider()
        {
            RegisterService<IMouseEvents>(new MouseHandler());
            RegisterService<IMouseWheel>(this);
            RegisterService<IPixelShader>(this);
            RegisterService<ILayout>(new PassDownChildrenLayout());

            Value = new ObservableProperty<float>(0, v => v.Clamp(0, 1));
            Transform.AddChild(new SliderHandle(Value));
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
