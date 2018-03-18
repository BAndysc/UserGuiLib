using UserGuiLib.Common.Algebra;
using UserGuiLib.Common.Component;
using UserGuiLib.Common.Drawing;

namespace UserGuiLib.Common.Services.Layout
{
    public class VerticalGroupLayout : ILayout
    {
        public bool HorizontalStrech = false;
        public float Spacing = 5;

        public IComponent Owner { get; set; }

        public void Relayout(IGraphics g)
        {
            bool anyChild = false;
            float y = 0;
            foreach (var child in Owner.Transform.Children)
            {
                var fitter = child.GetService<ILayout>();
                if (fitter != null)
                    fitter.Relayout(g);

                child.Transform.Location = new Vector2(0, y);
                if (HorizontalStrech)
                    child.Transform.Size = new Vector2(Owner.Transform.Size.x, child.Transform.Size.y);

                y += child.Transform.Size.y + Spacing;
                anyChild = true;
            }
            if (anyChild)
                y -= Spacing;

            Owner.Transform.Size = new Vector2(Owner.Transform.Size.x, y);
        }
    }
}
