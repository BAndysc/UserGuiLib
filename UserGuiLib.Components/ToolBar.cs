using UserGuiLib.Common.Impl;
using UserGuiLib.Common.Services.Layout;
using UserGuiLib.Common.Services.MouseEvents;

namespace UserGuiLib.Components
{
    public class ToolBar : Component
    {
        public ToolBar()
        {
            Transform.Size = new Common.Algebra.Vector2(0, 40);

            var layout = new HorizontalGroupLayout
            {
                VerticalStrech = true,
                Spacing = 5
            };

            RegisterService<ILayout>(layout);
            RegisterService<IMouseEvents>(new MouseHandler());
        }
    }
}
