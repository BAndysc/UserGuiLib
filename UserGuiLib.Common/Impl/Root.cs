using UserGuiLib.Common.Services.Layout;
using UserGuiLib.Common.Services.MouseEvents;

namespace UserGuiLib.Common.Impl
{
    public class Root : Component
    {
        public Root()
        {
            var mouseHandler = new MouseHandler();
            RegisterService<IMouseEvents>(mouseHandler);
            RegisterService<IMouseWheel>(mouseHandler);
            RegisterService<ILayout>(new PassDownChildrenLayout());
        }
    }
}
