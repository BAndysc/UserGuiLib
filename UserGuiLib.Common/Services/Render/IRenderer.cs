using UserGuiLib.Common.Drawing;

namespace UserGuiLib.Common.Services.Render
{
    public interface IRenderer : IService
    {
        void Render(IGraphics graphics);
    }
    
    public interface IAfterChildrenRenderer : IService
    {
        void RenderAfter(IGraphics graphics);
    }
}
