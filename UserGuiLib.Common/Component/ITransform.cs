using QuadTrees;
using QuadTrees.QTreeRect;
using System.Collections.Generic;
using UserGuiLib.Common.Algebra;

namespace UserGuiLib.Common.Component
{
    public interface ITransform
    {
        ITransform Parent { get; set; }
        
        Vector2 Location { get; set; }
        Vector2 Size { get; set; }
        Rect Bounds { get; set; }

        Vector2 WorldLocation { get; }

        IComponent Object { get; }

        void AddChild(IComponent component);
        void RemoveChild(IComponent component);

        IEnumerable<IComponent> Children { get; }

        IEnumerable<IComponent> ChildrenInRegion(Vector2 p1, Vector2 p2);
    }
}
