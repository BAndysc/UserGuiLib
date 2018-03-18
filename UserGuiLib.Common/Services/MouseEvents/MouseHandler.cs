using System;
using UserGuiLib.Common.Algebra;
using UserGuiLib.Common.Component;

namespace UserGuiLib.Common.Services.MouseEvents
{
    public class MouseHandler : IMouseEvents
    {
        public event Action<MouseButtons, Vector2> OnMouseMove = delegate { };
        public event Action<MouseButtons, Vector2> OnMouseDown = delegate { };
        public event Action<MouseButtons, Vector2> OnMouseUp = delegate { };
        public event Action OnMouseEnter = delegate { };
        public event Action OnMouseExit = delegate { };

        public IComponent Owner { get; set; }

        private ITransform current;
        private IMouseEvents currentOver;
        
        public bool IsLeftDown { get; private set; }
        public bool IsOver { get; private set; }

        private ITransform RayCast(Vector2 mouse)
        {
            var list = Owner.Transform.ChildrenInRegion(mouse, mouse+Vector2.Zero);
            foreach (var el in list)
                return el.Transform;
            return null;
        }

        public void MouseMove(MouseButtons buttons, Vector2 mouse)
        {
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
                    mouseEvents.MouseMove(buttons, mouse - raycast.Location);
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
                var service = raycast.Object.GetService<IMouseEvents>();
                if (service != null)
                    service.MouseDown(buttons, cursor - raycast.Location);
            }
            else
            {
                OnMouseDown(buttons, cursor);
                if (buttons.HasFlag(MouseButtons.Left))
                    IsLeftDown = true;
            }
            
        }

        public void MouseUp(MouseButtons buttons, Vector2 cursor)
        {
            var raycast = RayCast(cursor);

            if (raycast != null)
            {
                var service = raycast.Object.GetService<IMouseEvents>();
                if (service != null)
                    service.MouseUp(buttons, cursor - raycast.Location);
            }
            else
            {
                OnMouseUp(buttons, cursor);
                if (buttons.HasFlag(MouseButtons.Left))
                    IsLeftDown = false;
            }
        }
    }
}
