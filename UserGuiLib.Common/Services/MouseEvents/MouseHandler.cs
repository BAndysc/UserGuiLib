using System;
using UserGuiLib.Common.Algebra;
using UserGuiLib.Common.Component;

namespace UserGuiLib.Common.Services.MouseEvents
{
    public class MouseHandler : IMouseEvents, IMouseWheel
    {
        public event Action<MouseButtons, Vector2> OnMouseMove = delegate { };
        public event Action<MouseButtons, Vector2> OnMouseDown = delegate { };
        public event Action<MouseButtons, Vector2> OnMouseUp = delegate { };
        public event Action<float> OnMouseWheel = delegate { };
        public event Action OnMouseEnter = delegate { };
        public event Action OnMouseExit = delegate { };

        public IComponent Owner { get; set; }

        private ITransform current;
        private IMouseEvents currentOver;

        private ITransform down;

        public bool IsLeftDown { get; private set; }
        public bool IsOver { get; private set; }
        public Vector2 Position { get; private set; }

        private bool blocked;

        private ITransform RayCast(Vector2 mouse)
        {
            if (blocked)
                return null;

            if (down != null)
                return down;

            var list = Owner.Transform.ChildrenInRegionRelative(mouse, mouse + Vector2.Half);
            foreach (var el in list)
                return el.Transform;
            return null;
        }

        public void MouseMove(MouseButtons buttons, Vector2 mouse)
        {
            Position = mouse;
            var raycast = RayCast(mouse);

            if (raycast != null)
            {
                var mouseEvents = raycast.Object.GetService<IMouseEvents>();

                if (current != null && raycast != current)
                {
                    currentOver.MouseExit();
                    current = raycast;
                }

                if (mouseEvents != null)
                {
                    mouseEvents.MouseMove(buttons, (mouse - raycast.TopLeftPoint)/raycast.Scale);
                    if (currentOver != mouseEvents)
                        mouseEvents.MouseEnter();
                    currentOver = mouseEvents;
                    current = raycast;
                }
            }
            else
            {
                if (currentOver != null)
                {
                    currentOver.MouseExit();
                    currentOver = null;
                }
                current = null;
            }

            OnMouseMove(buttons, mouse);
        }

        public void MouseEnter()
        {
            OnMouseEnter();
            IsOver = true;
        }

        public void MouseExit()
        {
            IsOver = false;
            IsLeftDown = false;
            OnMouseExit();
            if (currentOver != null)
            {
                currentOver.MouseExit();
                currentOver = null;
            }
            current = null;
        }

        public void MouseDown(MouseButtons buttons, Vector2 cursor)
        {
            var raycast = RayCast(cursor);

            if (raycast != null)
            {
                down = raycast;
                var service = raycast.Object.GetService<IMouseEvents>();
                if (service != null)
                    service.MouseDown(buttons, (cursor - raycast.TopLeftPoint) / raycast.Scale);
            }
            else
            {
                down = null;
                OnMouseDown(buttons, cursor);
                if (buttons.HasFlag(MouseButtons.Left))
                    IsLeftDown = true;
            }
            
        }

        public void MouseUp(MouseButtons buttons, Vector2 cursor)
        {
            var raycast = down != null ? down : RayCast(cursor);

            if (raycast != null)
            {
                var service = raycast.Object.GetService<IMouseEvents>();
                if (service != null)
                    service.MouseUp(buttons, (cursor - raycast.TopLeftPoint) / raycast.Scale);
                down = null;
            }
            else
            {
                OnMouseUp(buttons, cursor);
                if (buttons.HasFlag(MouseButtons.Left))
                    IsLeftDown = false;
            }
        }

        public void MouseWheel(float delta)
        {
            if (current != null)
            {
                var wheel = current.Object.GetService<IMouseWheel>();
                if (wheel != null)
                    wheel.MouseWheel(delta);
                else
                    OnMouseWheel(delta);
            }
            else
                OnMouseWheel(delta);
        }

        public void Block(bool block)
        {
            if (block)
            {
                if (down != null)
                {
                    MouseUp(MouseButtons.Left, Vector2.Zero);
                }
                if (current != null)
                {
                    MouseExit();
                }
            }
            blocked = block;
        }
    }
}
