using ObservableObjects;
using UserGuiLib.Common.Impl;
using UserGuiLib.Common.Services.Render;

namespace UserGuiLib.ExampleCommon.ColorWheel
{
    public class ColorBox : Component, IPixelShader
    {
        public ObservableProperty<float> Hue;

        public ColorBox()
        {
            Hue = new ObservableProperty<float>(0);
            RegisterService<IPixelShader>(this);
        }
        
        public int PixelShader(uint screenX, uint screenY, float x, float y)
        {
            return ColorScale.HsvToRgb(Hue.get() * 360, x / Transform.Size.x, y / Transform.Size.y);
        }
    }
}
