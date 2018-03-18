using UserGuiLib.Common.Services.Layout;
using UserGuiLib.Common.Services.MouseEvents;

namespace UserGuiLib.Common.Impl
{
    public class Root : Component
    {
        public Root()
        {
            RegisterService<IMouseEvents>(new MouseHandler());
            RegisterService<ILayout>(new PassDownChildrenLayout());
        }
    }
}
