using UserGuiLib.Common.Component;
using UserGuiLib.Common.Impl;
using UserGuiLib.Common.Services.Render;

namespace UserGuiLib.ExampleCommon.ColorWheel
{
    public class ColorBox : Component, IPixelShader
    {
        public float Hue;

        public ColorBox()
        {
            RegisterService<IPixelShader>(this);
        }
        
        public int PixelShader(uint screenX, uint screenY, float x, float y)
        {
            return ColorScale.HsvToRgb(Hue, x / Transform.Size.x, y / Transform.Size.y);
        }
    }
}
