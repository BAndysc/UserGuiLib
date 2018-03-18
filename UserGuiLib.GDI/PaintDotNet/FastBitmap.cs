using PaintDotNet.SystemLayer;
using System;
using System.Drawing;

namespace PaintDotNet
{
    public class FastBitmap : IDisposable
    {
        private Surface surface;
        private Bitmap bitmap;
        private Graphics graphics;

        public Graphics Graphics
        {
            get
            {
                return graphics;
            }
        }

        public Surface Surface
        {
            get
            {
                return surface;
            }
        }

        private uint[] background;

        public FastBitmap(int width, int height, ColorBgra backgroundColor)
        {
            surface = new Surface(width, height);
            bitmap = surface.CreateAliasedBitmap();
            graphics = Graphics.FromImage(bitmap);

            background = new uint[width * height];
            for (int i = 0; i < width * height; ++i)
            {
                background[i] = backgroundColor.Bgra;
            }
        }

        public unsafe void Clear()
        {
            fixed (uint* src = background)
            {
                Memory.Copy(surface.Scan0.Pointer, (IntPtr)src, (ulong)(surface.Stride * surface.Height));
            }
        }

        public void Flip(Graphics g, Rectangle clipRectangle)
        {
            surface.GetDrawBitmapInfo(out IntPtr tracking, out Point childOffset, out Size parentSize);

            SystemLayer.PdnGraphics.DrawBitmap(g, clipRectangle, g.Transform, tracking, childOffset.X, childOffset.Y);
        }

        public void Dispose()
        {
            graphics.Dispose();
            bitmap.Dispose();
            surface.Dispose();
        }
    }
}
