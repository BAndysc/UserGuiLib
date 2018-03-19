using PaintDotNet;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UserGuiLib.Common.Algebra;
using UserGuiLib.Common.Component;
using UserGuiLib.Common.Control;

namespace UserGuiLib.GDI
{
    public class UserGuiControl : Control, IControlBase
    {
        private class InternalBase : ControlBase
        {
            private UserGuiControl Parent;
            public Surface surface;

            protected override float Width => Parent.Width;

            protected override float Height => Parent.Height;

            public InternalBase(UserGuiControl parent)
            {
                Parent = parent;

                Draggable = false;

                Zoomable = false;
            }

            protected override void ProcessPixelFragment(int startX, int startY, int endX, int endY, Vector2 step, Vector2 start, Func<uint, uint, float, float, int> pixelShader)
            {
                int heightInPixels = endY - startY;
                int widthInPixels = endX - startX;

                // rendering in half resolution
                Parallel.For(0, heightInPixels - 1, y =>
                {
                    if (y % 2 == 1)
                        return;

                    float _y = y * step.y + start.y;
                    float _x = start.x;
                    for (int x = 0; x < widthInPixels; x += 2)
                    {
                        int res = pixelShader((uint)(x + startX), (uint)(y + startY), _x, _y);
                        var c = ColorBgra.FromInt32(res);
                        if (c.A == 255)
                        {
                            surface.SetPointFast(x + startX, y + startY, c);
                            surface.SetPointFast(x + startX + 1, y + startY, c);
                        }
                        _x += step.x * 2;
                    }
                    PaintDotNet.SystemLayer.Memory.Copy(surface.Scan0.Pointer + (y + 1 + startY) * surface.Stride + startX * 4,
                        surface.Scan0.Pointer + (y + startY) * surface.Stride + startX * 4,
                        (uint)(widthInPixels * 4));
                });
            }
        }

        private InternalBase baseControl;
        private FastBitmap surface;
        private GDIGraphics graphics;
        
        // public

        public UserGuiControl()
        {
            baseControl = new InternalBase(this);
            graphics = new GDIGraphics(null);

            OnSizeChanged(null);

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);


            var thisThread = Thread.CurrentThread;
            var context = SynchronizationContext.Current;
            
            var thread = new Thread(() =>
            {
                while (thisThread.IsAlive)
                {
                    if (Created)
                    {
                        baseControl.Update();
                        context.Send((o) =>
                        {
                            Invalidate();
                        }, null);
                    }
                    Thread.Sleep(16);
                }
            });

            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Runtime)
                thread.Start();
        }

        public void AddComponent(IComponent component)
        {
            baseControl.AddComponent(component);
        }

        // protected

        protected override void OnSizeChanged(EventArgs e)
        {
            if (surface != null)
                surface.Dispose();

            if (Width > 0 && Height > 0)
            {
                surface = new FastBitmap(Width, Height, ColorBgra.White);
                graphics.Graphics = surface.Graphics;
                baseControl.surface = surface.Surface;
            }
            else
                surface = null;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (e.ClipRectangle.IsEmpty)
                return;

            graphics.Graphics.ResetTransform();
            surface.Clear();

            baseControl.Render(graphics);

            surface.Flip(e.Graphics, e.ClipRectangle);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // do nothing to optimize
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            baseControl.MouseMoved(new Vector2(e.X, e.Y));
            baseControl.OnMouseDown(ToMouseButtons(e.Button));
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            baseControl.MouseMoved(new Vector2(e.X, e.Y));
            baseControl.OnMouseUp(ToMouseButtons(e.Button));
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            baseControl.MouseMoved(new Vector2(e.X, e.Y));
            baseControl.OnMouseMove(ToMouseButtons(e.Button));
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            baseControl.OnMouseWheel(e.Delta);
        }

        // private

        private Common.Services.MouseEvents.MouseButtons ToMouseButtons(MouseButtons buttons)
        {
            var result = Common.Services.MouseEvents.MouseButtons.None;

            if (buttons.HasFlag(MouseButtons.Left))
                result |= Common.Services.MouseEvents.MouseButtons.Left;

            if (buttons.HasFlag(MouseButtons.Right))
                result |= Common.Services.MouseEvents.MouseButtons.Right;

            return result;
        }
    }
}
