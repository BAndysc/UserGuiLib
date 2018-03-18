using System;
using System.Collections.Generic;
using UserGuiLib.Common.Algebra;
using UserGuiLib.Common.Drawing;

namespace UserGuiLib.GDI
{
    public class GDIGraphics : IGraphics
    {
        public System.Drawing.Graphics Graphics
        {
            get
            {
                return graphics;
            }
            set
            {
                graphics = value;
                if (graphics != null)
                    scalingFactor = 1;
            }
        }
        private float scalingFactor = 1.0f;
        private System.Drawing.Graphics graphics;

        private Dictionary<AnyFont, System.Drawing.Font> fonts = new Dictionary<AnyFont, System.Drawing.Font>();
        private Dictionary<AnyPen, System.Drawing.Pen> pens = new Dictionary<AnyPen, System.Drawing.Pen>();
        private Dictionary<AnyImage, System.Drawing.Image> images = new Dictionary<AnyImage, System.Drawing.Image>();

        public GDIGraphics(System.Drawing.Graphics g)
        {
            Graphics = g;
        }
        
        public void FillCircle(AnyPen penHandle, Vector2 center, Vector2 anchor, float radius)
        {
            var oldAntialias = Graphics.SmoothingMode;

            var pen = GetPen(penHandle);
            center *= scalingFactor;
            radius *= scalingFactor;

            Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Graphics.FillEllipse(pen.Brush, center.x - anchor.x * radius * 2, center.y - anchor.y * radius * 2, radius * 2, radius * 2);
            Graphics.SmoothingMode = oldAntialias;
        }

        public void DrawCircle(AnyPen penHandle, Vector2 center, Vector2 anchor, float radius)
        {
            var oldAntialias = Graphics.SmoothingMode;

            var pen = GetPen(penHandle);
            center *= scalingFactor;
            radius *= scalingFactor;

            Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Graphics.DrawEllipse(pen, center.x - anchor.x * radius * 2, center.y - anchor.y * radius * 2, radius * 2, radius * 2);
            Graphics.SmoothingMode = oldAntialias;
        }

        public void DrawLine(AnyPen pen, Vector2 p1, Vector2 p2)
        {
            var oldAntialias = Graphics.SmoothingMode;
            p1 *= scalingFactor;
            p2 *= scalingFactor;
            Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Graphics.DrawLine(GetPen(pen), p1.x, p1.y, p2.x, p2.y);
            Graphics.SmoothingMode = oldAntialias;
        }

        public void DrawRectangle(AnyPen pen, Vector2 p1, Vector2 p2, float radius = 0)
        {
            p1 *= scalingFactor;
            p2 *= scalingFactor;
            if (radius == 0)
            {
                float x = (int)Math.Ceiling(Math.Min(p1.x, p2.x));
                float y = (int)Math.Ceiling(Math.Min(p1.y, p2.y));
                float w = Math.Abs(p1.x - p2.x);
                float h = Math.Abs(p1.y - p2.y);

                var p = GetPen(pen);

                if (w > 0.001f && h > 0.00f)
                    Graphics.DrawRectangle(p, x, y, w - p.Width, h - p.Width);
            }
            else
            {
                using (System.Drawing.Drawing2D.GraphicsPath path = RoundedRect(p1, p2, radius))
                {
                    Graphics.DrawPath(GetPen(pen), path);
                }
            }
        }

        public void DrawSquare(AnyPen pen, Vector2 center, Vector2 anchor, float length)
        {
            var p1 = center - anchor * length;
            var p2 = p1 + new Vector2(length, length);
            DrawRectangle(pen, p1, p2, 0);
        }

        public void DrawString(string text, AnyPen pen, AnyFont font, Vector2 point, Vector2 anchor, float maxWidth = 0)
        {
            Vector2 measure = MeasureString(text, font, maxWidth);

            float x = point.x - anchor.x * measure.x;
            float y = point.y - anchor.y * measure.y;

            Graphics.DrawString(text, GetFont(font), GetPen(pen).Brush, x * scalingFactor, y * scalingFactor);
        }

        public void FillRectangle(AnyPen pen, Vector2 p1, Vector2 p2, float radius = 0)
        {
            p1 *= scalingFactor;
            p2 *= scalingFactor;
            if (radius == 0)
            {
                float x = Math.Min(p1.x, p2.x);
                float y = Math.Min(p1.y, p2.y);
                float w = Math.Abs(p1.x - p2.x);
                float h = Math.Abs(p1.y - p2.y);

                var p = GetPen(pen);

                if (w > 0.001f && h > 0.00f)
                    Graphics.FillRectangle(p.Brush, x, y, w - p.Width, h - p.Width);
            }
            else
            {
                using (System.Drawing.Drawing2D.GraphicsPath path = RoundedRect(p1, p2, radius))
                {
                    Graphics.FillPath(GetPen(pen).Brush, path);
                }
            }
        }

        public Vector2 MeasureString(string text, AnyFont font, float maxWidth = 0)
        {
            var size = Graphics.MeasureString(text, GetFont(font), (int)maxWidth);
            return new Vector2(size.Width / scalingFactor, size.Height / scalingFactor);
        }

        public void DrawImage(AnyImage image, Vector2 point, Vector2 destSize, Vector2 anchor)
        {
            destSize *= scalingFactor;
            point *= scalingFactor;
            var x = (int)(point.x - destSize.x * anchor.x);
            var y = (int)(point.y - destSize.y * anchor.y);
            Graphics.DrawImageUnscaled(GetImage(image), x, y, (int)destSize.x, (int)destSize.y);
        }

        // private

        private System.Drawing.Pen GetPen(AnyPen handle)
        {
            if (pens.ContainsKey(handle))
                return pens[handle];
            else
            {
                var pen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(handle.a, handle.r, handle.g, handle.b), handle.Width)
                {
                    Alignment = System.Drawing.Drawing2D.PenAlignment.Inset
                };
                pens[handle] = pen;
                return pen;
            }
        }

        private System.Drawing.Font GetFont(AnyFont handle)
        {
            if (fonts.ContainsKey(handle))
                return fonts[handle];
            else
            {
                var font = new System.Drawing.Font(handle.FontName, handle.Size);
                fonts[handle] = font;
                return font;
            }
        }

        private System.Drawing.Image GetImage(AnyImage handle)
        {
            System.Diagnostics.Debug.Assert(images.ContainsKey(handle));
            return images[handle];
        }

        private System.Drawing.Drawing2D.GraphicsPath RoundedRect(Vector2 p1, Vector2 p2, float radius)
        {
            float diameter = radius * 2;
            System.Drawing.SizeF size = new System.Drawing.SizeF(diameter, diameter);
            System.Drawing.RectangleF arc = new System.Drawing.RectangleF(new System.Drawing.PointF(p1.x, p1.y), size);
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();

            /*if (radius == 0)
            {
                path.AddRectangle(bounds);
                return path;
            }*/

            // top left arc  
            path.AddArc(arc, 180, 90);

            // top right arc  
            arc.X = p2.x - diameter;
            path.AddArc(arc, 270, 90);

            // bottom right arc  
            arc.Y = p2.y - diameter;
            path.AddArc(arc, 0, 90);

            // bottom left arc 
            arc.X = p1.x;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        public void Translate(Vector2 offset)
        {
            graphics.TranslateTransform(offset.x, offset.y, System.Drawing.Drawing2D.MatrixOrder.Prepend);
        }

        public void Scale(Vector2 scale)
        {
            graphics.ScaleTransform(scale.x, scale.y, System.Drawing.Drawing2D.MatrixOrder.Prepend);
        }

        public void Clip(Vector2 point1, Vector2 point2)
        {
            graphics.SetClip(new System.Drawing.RectangleF(point1.x, point1.y, point2.x - point1.x, point2.y - point1.y));
        }

        public void ResetTransform()
        {
            graphics.ResetTransform();
        }

        public void ResetClip()
        {
            graphics.ResetClip();
        }

        public static readonly float DefaultDPI = 96;
    }
}
