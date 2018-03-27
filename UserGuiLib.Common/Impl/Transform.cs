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
        private Vector2 _size;
        public Vector2 Size
        {
            get
            {
                if (Anchor.Min == Anchor.Max)
                {
                    return _size;
                }
                return (Anchor.Max-Anchor.Min)*Parent.Size;
            }

            set
            {
                _size = value;
            }
        }
        public Anchor Anchor { get; set; }
        public Vector2 Pivot { get; set; }
        public Vector2 Scale { get; set; }

        public Vector2 TopLeftPoint
        {
            get
            {
                if (Parent == null)
                    return Location - Size * Pivot * Scale;

                return Location - Size * Pivot * Scale + Parent.Size * Anchor.Min;
            }
        }

        public Vector2 BottomRightPoint
        {
            get
            {
                if (Parent == null)
                    return Location - Size * Pivot * Scale + Size * Scale;
                return Location - Size * Pivot*Scale + Parent.Size * Anchor.Max+Size*Scale;
            }
        }

        public Rect Bounds { get; set; }

        public IComponent Object { get; private set; }

        public Vector2 WorldTopLeftPoint
        {
            get
            {
                if (Parent == null)
                    return TopLeftPoint;
                return PointToWorld(Vector2.Zero);
            }
        }
        public Vector2 WorldBottomRightPoint
        {
            get
            {
                if (Parent == null)
                    return BottomRightPoint;
                return PointToWorld(Size);
            }
        }

        private HashSet<IComponent> componentsToRemove = new HashSet<IComponent>();

        public void AddChild(IComponent component)
        {
            component.Transform.Parent = this;
            children.Add(component);
        }

        public void RemoveChild(IComponent component)
        {
            componentsToRemove.Add(component);
        }

        private void RemovePendingComponents()
        {
            foreach (var component in componentsToRemove)
            {
                children.Remove(component);
                component.Transform.Parent = null;
            }
            componentsToRemove.Clear();
        }

        public IEnumerable<IComponent> ChildrenInRegion(Vector2 p1, Vector2 p2)
        {
            foreach (var child in children)
                if (!(child.Transform.WorldTopLeftPoint.x >= p2.x ||
                    child.Transform.WorldTopLeftPoint.y >= p2.y ||
                    child.Transform.WorldBottomRightPoint.x < p1.x ||
                    child.Transform.WorldBottomRightPoint.y < p1.y))
                    yield return child;
            RemovePendingComponents();
        }

        public IEnumerable<IComponent> ChildrenInRegionRelative(Vector2 p1, Vector2 p2)
        {
            foreach (var child in children)
                if (!(child.Transform.TopLeftPoint.x >= p2.x ||
                    child.Transform.TopLeftPoint.y >= p2.y ||
                    child.Transform.BottomRightPoint.x < p1.x ||
                    child.Transform.BottomRightPoint.y < p1.y))
                    yield return child;
            RemovePendingComponents();
        }

        public IEnumerable<IComponent> Children
        {
            get
            {
                foreach (var child in children)
                    yield return child;
                RemovePendingComponents();
            }
        }

        public Vector2 PointToParent(Vector2 point)
        {
            return point * Scale + TopLeftPoint;
        }

        public Vector2 PointToWorld(Vector2 point)
        {
            if (Parent == null)
                return point;
            return Parent.PointToWorld(PointToParent(point));
        }
    }
}
