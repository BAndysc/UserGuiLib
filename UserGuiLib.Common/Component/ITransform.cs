using System.Collections.Generic;
using UserGuiLib.Common.Algebra;

namespace UserGuiLib.Common.Component
{
    public struct Anchor
    {
        public Vector2 Min { get; private set; }
        public Vector2 Max { get; private set; }
        
        public bool SamePoint { get; private set; }

        public Anchor(Vector2 min, Vector2 max)
        {
            Min = min;
            Max = max;
            SamePoint = false;
        }

        public Anchor(Vector2 anchor)
        {
            Min = anchor;
            Max = anchor;
            SamePoint = true;
        }
    }
    public interface ITransform
    {
        ITransform Parent { get; set; }
        
        Vector2 TopLeftPoint { get; }
        Vector2 BottomRightPoint { get; }
        Vector2 Location { get; set; }
        Vector2 Size { get; set; }
        Anchor Anchor { get; set; }
        Vector2 Pivot { get; set; }
        Vector2 Scale { get; set; }
        Rect Bounds { get; set; }

        Vector2 WorldTopLeftPoint { get; }
        Vector2 WorldBottomRightPoint { get; }

        IComponent Object { get; }

        void AddChild(IComponent component);
        void RemoveChild(IComponent component);

        IEnumerable<IComponent> Children { get; }

        IEnumerable<IComponent> ChildrenInRegion(Vector2 p1, Vector2 p2);
        IEnumerable<IComponent> ChildrenInRegionRelative(Vector2 p1, Vector2 p2);

        Vector2 PointToParent(Vector2 point);
        Vector2 PointToWorld(Vector2 point);
    }
}
