using System;
using System.Collections.Generic;
using System.Drawing;
using QuadTrees;
using QuadTrees.QTreeRect;
using UserGuiLib.Common.Algebra;
using UserGuiLib.Common.Component;

namespace UserGuiLib.Common.Impl
{
    class Transform : ITransform
    {
        private class TransformWrapper : IRectQuadStorable
        {
            public ITransform transform;

            public TransformWrapper(ITransform transform)
            {
                this.transform = transform;
            }

            public RectangleF Rect => new RectangleF(transform.Location.x, transform.Location.y, transform.Size.x, transform.Size.y);
        }

        public Transform(IComponent owner, ITransform parent)
        {
            Object = owner;
            Parent = parent;
            ChildrenTree = new QuadTreeRect<TransformWrapper>();
        }

        public ITransform Parent { get; set; }

        private Dictionary<ITransform, TransformWrapper> childrenWrappers = new Dictionary<ITransform, TransformWrapper>();
        private QuadTreeRect<TransformWrapper> ChildrenTree;

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
            var wrapper = new TransformWrapper(component.Transform);
            childrenWrappers.Add(component.Transform, wrapper);
            ChildrenTree.Add(wrapper);
        }

        public void RemoveChild(IComponent component)
        {
            var wrapper = childrenWrappers[component.Transform];
            childrenWrappers.Remove(component.Transform);
            ChildrenTree.Remove(wrapper);
        }

        public IEnumerable<IComponent> ChildrenInRegion(Vector2 p1, Vector2 p2)
        {
            foreach (var wrapper in ChildrenTree.GetObjects(new RectangleF(p1.x, p1.y, p2.x - p1.x, p2.y - p1.y)))
                yield return wrapper.transform.Object;
        }

        public IEnumerable<IComponent> Children
        {
            get
            {
                foreach (var wrapper in ChildrenTree.GetAllObjects())
                    yield return wrapper.transform.Object;
            }
        }
    }
}
