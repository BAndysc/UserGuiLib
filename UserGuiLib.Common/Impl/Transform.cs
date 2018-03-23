using System.Collections.Generic;
using UserGuiLib.Common.Algebra;
using UserGuiLib.Common.Component;

namespace UserGuiLib.Common.Impl
{
    class Transform : ITransform
    {
        public Transform(IComponent owner, ITransform parent)
        {
            Scale = Vector2.One;
            Object = owner;
            Parent = parent;
        }

        public ITransform Parent { get; set; }
        private List<IComponent> children = new List<IComponent>();

        public Vector2 Location { get; set; }
        public Vector2 Size { get; set; }
        public Vector2 Anchor { get; set; }
        public Vector2 Pivot { get; set; }
        public Vector2 Scale { get; set; }

        public Vector2 TopLeftPoint
        {
            get
            {
                if (Parent == null)
                    return Location - Size * Pivot * Scale;
                return Location - Size * Pivot * Scale + Parent.Size * Anchor;
            }
        }

        public Vector2 BottomRightPoint
        {
            get
            {
                return Location - Size * Pivot*Scale + Parent.Size * Anchor+Size*Scale;
            }
        }

        public Rect Bounds { get; set; }

        public IComponent Object { get; private set; }

        public Vector2 WorldLocation
        {
            get
            {
                if (Parent == null)
                    return TopLeftPoint;
                return TopLeftPoint + Parent.TopLeftPoint;
            }
        }

        public void AddChild(IComponent component)
        {
            component.Transform.Parent = this;
            children.Add(component);
        }

        public void RemoveChild(IComponent component)
        {
            component.Transform.Parent = null;
            children.Remove(component);
        }

        public IEnumerable<IComponent> ChildrenInRegion(Vector2 p1, Vector2 p2)
        {
            foreach (var child in children)
                if (!(child.Transform.WorldLocation.x >= p2.x ||
                    child.Transform.WorldLocation.y >= p2.y ||
                    child.Transform.WorldLocation.x + child.Transform.Size.x < p1.x ||
                    child.Transform.WorldLocation.y + child.Transform.Size.y < p1.y))
                yield return child;
        }

        public IEnumerable<IComponent> ChildrenInRegionRelative(Vector2 p1, Vector2 p2)
        {
            foreach (var child in children)
                if (!(child.Transform.TopLeftPoint.x >= p2.x ||
                    child.Transform.TopLeftPoint.y >= p2.y ||
                    child.Transform.BottomRightPoint.x < p1.x ||
                    child.Transform.BottomRightPoint.y < p1.y))
                    yield return child;
        }

        public IEnumerable<IComponent> Children
        {
            get
            {
                foreach (var child in children)
                    yield return child;
            }
        }
    }
}
