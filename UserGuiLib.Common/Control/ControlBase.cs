using System;
using System.Linq;
using UserGuiLib.Common.Algebra;
using UserGuiLib.Common.Component;
using UserGuiLib.Common.Drawing;
using UserGuiLib.Common.Impl;
using UserGuiLib.Common.Services.Layout;
using UserGuiLib.Common.Services.MouseEvents;
using UserGuiLib.Common.Services.Render;

namespace UserGuiLib.Common.Control
{
    public abstract class ControlBase : IControlBase
    {
        private Root root;

        protected bool IsMouseLeftDown
        {
            get { return DownButtons.HasFlag(MouseButtons.Left); }
        }
        protected bool IsMouseRightDown
        {
            get {  return DownButtons.HasFlag(MouseButtons.Right); }
        }
        protected MouseButtons DownButtons { get; private set; }
        protected MouseButtons DragOnButton { get; set; } = MouseButtons.Left;

        protected Vector2 DelayedMouseScreenPosition { get; private set; }
        protected Vector2 MouseStartZoomScreen { get; private set; }
        protected Vector2 MouseStartScreenPosition { get; private set; }
        protected Vector2 MouseStartWorldPosition { get; private set; }
        protected Vector2 MouseScreenPosition { get; private set; }

        protected Vector2 DragStartOffset { get; private set; }
        protected Vector2 Offset { get; private set; }
        protected Vector2 OffsetDirection { get; private set; }
        protected float OffsetVelocity { get; private set; } = 0;

        protected float Zoom { get; private set; } = 1;
        protected float ZoomVelocity { get; private set; } = 0;

        protected bool Dragging { get; private set; } = false;

        public bool Draggable { get; set; } = true;
        public bool Zoomable { get; set; } = true;

        protected float ScalingFactor { get; private set; } = 1;

        protected Vector2 MouseWorldPosition
        {
            get { return ScreenToWorld(MouseScreenPosition); }
        }

        protected abstract float Width { get; }

        protected abstract float Height { get; }

        protected virtual float Dpi => 1.0f;

        public bool DrawBounds;

        public ControlBase()
        {
            root = new Root();
        }
        
        public Vector2 ScreenToWorld(Vector2 vector)
        {
            return (vector / Dpi / Zoom + Offset);
        }

        public void WorldToScreen(Vector2 vector, out int x, out int y)
        {
            var res = ((vector - Offset) * Zoom) * Dpi;
            x = (int)res.x;
            y = (int)res.y;
        }

        public void Render(IGraphics graphics)
        {
            // divide by Dpi so that root is dpi unaware
            root.Transform.Size = new Vector2(Width / Dpi, Height / Dpi);
            
            foreach (var child in root.Transform.Children)
            {
                var layout = child.GetService<ILayout>();
                if (layout != null)
                    layout.Relayout(graphics);
            }

            graphics.Scale(new Vector2(Zoom, Zoom));
            graphics.Translate(Offset * -1);

            RecursiveDrawChildren(graphics, root);
        }

        private void RecursiveDrawChildren(IGraphics graphics, IComponent parent)
        {
            var p1 = Offset;
            var p2 = ScreenToWorld(new Vector2(Width, Height));

            foreach (var child in parent.Transform.ChildrenInRegion(p1, p2))
            {
                graphics.Translate(child.Transform.Location);
                graphics.Translate(child.Transform.Parent.Size * child.Transform.Anchor.Min);
                graphics.Scale(child.Transform.Scale);
                graphics.Translate(child.Transform.Size * child.Transform.Pivot * -1.0f);

                var pixel = child.GetService<IPixelShader>();
                if (pixel != null)
                {
                    WorldToScreen(child.Transform.WorldTopLeftPoint, out int startX, out int startY);
                    WorldToScreen(child.Transform.WorldTopLeftPoint + child.Transform.Size, out int endX, out int endY);
                    startX = startX.Clamp(0, (int)Width);
                    startY = startY.Clamp(0, (int)Height);
                    endX = endX.Clamp(0, (int)Width);
                    endY = endY.Clamp(0, (int)Height);
                    int diffX = endX - startX;
                    int diffY = endY - startY;

                    if (diffX >= 0 && diffY >= 0)
                    {
                        var world = ScreenToWorld(new Vector2(startX, startY));
                        var world2 = ScreenToWorld(new Vector2(startX + 1, startY + 1));

                        var step = world2 - world;
                        var start = world - child.Transform.WorldTopLeftPoint;

                        ProcessPixelFragment(startX, startY, endX, endY, step, start, pixel.PixelShader);
                    }
                }

                var renderer = child.GetService<IRenderer>();
                if (renderer != null)
                    renderer.Render(graphics);

                if (child.Transform.Children.Any())
                    RecursiveDrawChildren(graphics, child);

                var rendererAfterChildren = child.GetService<IAfterChildrenRenderer>();
                if (rendererAfterChildren != null)
                    rendererAfterChildren.Render(graphics);

                if (DrawBounds)
                   graphics.DrawRectangle(new AnyPen(255, 0, 0, 255, 1), Vector2.Zero, child.Transform.Size);
                
                graphics.Translate(child.Transform.Size * child.Transform.Pivot);
                graphics.Scale(1.0f/child.Transform.Scale);
                graphics.Translate(child.Transform.Parent.Size * child.Transform.Anchor.Min * -1.0f);
                graphics.Translate(child.Transform.Location * -1.0f);

            }
        }

        protected abstract void ProcessPixelFragment(int startX, int startY, int endX, int endY, Vector2 step, Vector2 start, Func<uint, uint, float, float, int> pixelShader);

        public void MouseMoved(Vector2 mouse)
        {
            MouseScreenPosition = mouse / ScalingFactor;
        }

        public void OnMouseDown(MouseButtons buttons)
        {
            DragStartOffset = Offset;
            MouseStartScreenPosition = MouseScreenPosition;
            DelayedMouseScreenPosition = MouseScreenPosition;
            MouseStartWorldPosition = MouseWorldPosition;

            DownButtons |= buttons;

            Dragging = Draggable && ((DownButtons & DragOnButton) != 0);

            root.GetService<IMouseEvents>().MouseDown(buttons, MouseWorldPosition);
        }

        public void OnMouseUp(MouseButtons buttons)
        {
            bool wasDragging = Dragging;
            
            DownButtons &= ~buttons;

            if ((DownButtons & DragOnButton) == 0)
                Dragging = false;

            if (wasDragging && !Dragging)
            {
                OffsetDirection = (MouseScreenPosition - DelayedMouseScreenPosition).ToUnitVector();
                OffsetVelocity = 4;
            }

            root.GetService<IMouseEvents>().MouseUp(buttons, MouseWorldPosition);
        }

        public void OnMouseMove(MouseButtons buttons)
        {
            if (Dragging)
                Offset = DragStartOffset + (MouseStartScreenPosition - MouseScreenPosition) / Zoom * ScalingFactor;

            root.GetService<IMouseEvents>().MouseMove(buttons, MouseWorldPosition);
        }

        public void OnMouseWheel(float Delta)
        {
            if (Dragging)
                return;

            if (Zoomable)
            {
                MouseStartZoomScreen = MouseScreenPosition;
                if (Delta > 0)
                {
                    ZoomVelocity = 4;
                }
                else if (Delta < 0)
                {
                    ZoomVelocity = -4;
                }
            }

            root.GetService<IMouseWheel>().MouseWheel(Delta);
        }

        public void Update()
        {
            DelayedMouseScreenPosition = MouseScreenPosition;
            if (Math.Abs(ZoomVelocity) > 0.02f)
            {
                float oldzoom = Zoom;
                ZoomVelocity *= 0.9f;
                Zoom += ZoomVelocity * 0.01f * Zoom;
                Zoom = Zoom.Clamp(0.01f, 500);

                Vector2 oldimage = MouseStartZoomScreen / oldzoom;
                Vector2 newimage = MouseStartZoomScreen / Zoom;

                Offset -= newimage - oldimage;
            }

            if (Math.Abs(OffsetVelocity) > 0.1f)
            {
                OffsetVelocity *= 0.9f;

                Offset -= OffsetDirection * OffsetVelocity * 5 / Zoom;
            }
        }

        public IComponent AtPoint(Vector2 point)
        {
            return AtPointRecursive(ScreenToWorld(point), root);
        }

        private IComponent AtPointRecursive(Vector2 point, IComponent parent)
        {
            var children = parent.Transform.ChildrenInRegion(point, point + Vector2.Half);
            foreach (var child in children)
                return AtPointRecursive(point, child);

            return parent;
        }

        public void AddComponent(IComponent component)
        {
            root.Transform.AddChild(component);
        }
    }
}
