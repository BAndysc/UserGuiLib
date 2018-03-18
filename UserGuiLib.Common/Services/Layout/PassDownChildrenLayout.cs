using UserGuiLib.Common.Component;
using UserGuiLib.Common.Drawing;

namespace UserGuiLib.Common.Services.Layout
{
    public class PassDownChildrenLayout : ILayout
    {
        public IComponent Owner { get; set; }

        public void Relayout(IGraphics g)
        {
            foreach (var child in Owner.Transform.Children)
            {
                var layout = child.GetService<ILayout>();
                if (layout != null)
                    layout.Relayout(g);
            }
        }
    }
}
