using ObservableObjects;
using QuadTrees.QTreeRect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserGuiLib.Common.Services;

namespace UserGuiLib.Common.Component
{
    public interface IComponent
    {
        ITransform Transform { get; }

        T GetService<T>() where T : IService;
        T RegisterService<T>(T service) where T : IService;
    }
}
