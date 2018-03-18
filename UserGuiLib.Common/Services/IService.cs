using UserGuiLib.Common.Component;

namespace UserGuiLib.Common.Services
{
    public interface IService
    {
        IComponent Owner { get; set; }
    }
}
