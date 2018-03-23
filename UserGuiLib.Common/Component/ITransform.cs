using System.Collections.Generic;
using UserGuiLib.Common.Algebra;

namespace UserGuiLib.Common.Component
{
    public interface ITransform
    {
        ITransform Parent { get; set; }
        
        Vector2 TopLeftPoint { get; }
        Vector2 BottomRightPoint { get; }
        Vector2 Location { get; set; }
        Vector2 Size { get; set; }
        Vector2 Anchor { get; set; }
        Vector2 Pivot { get; set; }
        Vector2 Scale { get; set; }
        Rect Bounds { get; set; }

        Vector2 WorldLocation { get; }

        IComponent Object { get; }

        void AddChild(IComponent component);
        void RemoveChild(IComponent component);

        IEnumerable<IComponent> Children { get; }

        IEnumerable<IComponent> ChildrenInRegion(Vector2 p1, Vector2 p2);
        IEnumerable<IComponent> ChildrenInRegionRelative(Vector2 p1, Vector2 p2);
    }
}
