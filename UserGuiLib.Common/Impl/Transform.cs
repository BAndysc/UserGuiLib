using System.Collections.Generic;
using UserGuiLib.Common.Algebra;
using UserGuiLib.Common.Component;

namespace UserGuiLib.Common.Impl
{
    class Transform : ITransform
    {
        public Transform(IComponent owner, ITransform parent)
        {
            Object = owner;
            Parent = parent;
        }

        public ITransform Parent { get; set; }
        private List<IComponent> children = new List<IComponent>();

        public Vector2 Location { get; set; }
        public Vector2 Size { get; set; }
        public Rect Bounds { get; set; }

        public IComponent Object { get; private set; }

        public Vector2 WorldLocation
        {
            get
            {
                if (Parent == null)
                    return Location;
                return Location + Parent.Location;
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
                    child.Transform.WorldLocation.x + child.Transform.Size.y < p1.y))
                yield return child;
        }

        public IEnumerable<IComponent> ChildrenInRegionRelative(Vector2 p1, Vector2 p2)
        {
            foreach (var child in children)
                if (!(child.Transform.Location.x >= p2.x ||
                    child.Transform.Location.y >= p2.y ||
                    child.Transform.Location.x + child.Transform.Size.x < p1.x ||
                    child.Transform.Location.x + child.Transform.Size.y < p1.y))
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
