using System;

namespace UserGuiLib.Common.Services.Render
{
    /// <summary>
    /// Fake "shader" - it is rendered by CPU, so it is a toy rather than a usable tool
    /// </summary>
    public interface IPixelShader : IService
    {
        Int32 PixelShader(UInt32 screenX, UInt32 screenY, float x, float y);
    }
}
