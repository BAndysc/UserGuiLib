namespace UserGuiLib.Common.Services.Layout
{
    public interface ILayout : IService
    {
        void Relayout(Drawing.IGraphics g);
    }
}
