using UserGuiLib.Common.Algebra;
using UserGuiLib.Common.Component;
using UserGuiLib.Common.Drawing;

namespace UserGuiLib.Common.Services.Layout
{
    public class HorizontalGroupLayout : ILayout
    {
        public bool VerticalStrech = false;
        public float Spacing = 5;

        public IComponent Owner { get; set; }

        public void Relayout(IGraphics g)
        {
            bool anyChild = false;
            float x = 0;
            foreach (var child in Owner.Transform.Children)
            {
                var fitter = child.GetService<ILayout>();
                if (fitter != null)
                    fitter.Relayout(g);

                child.Transform.Location = new Vector2(x, 0);
                if (VerticalStrech)
                    child.Transform.Size = new Vector2(child.Transform.Size.x, Owner.Transform.Size.y);

                x += child.Transform.Size.x + Spacing;
                anyChild = true;
            }
            if (anyChild)
                x -= Spacing;

            Owner.Transform.Size = new Vector2(x, Owner.Transform.Size.y);
        }
    }
}
