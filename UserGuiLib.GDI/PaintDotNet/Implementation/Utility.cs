/////////////////////////////////////////////////////////////////////////////////
// Paint.NET                                                                   //
// Copyright (C) dotPDN LLC, Rick Brewster, Tom Jackson, and contributors.     //
// Portions Copyright (C) Microsoft Corporation. All Rights Reserved.          //
// See                                                                         //
// https://github.com/rivy/OpenPDN/blob/master/src/Resources/Files/License.txt //
// for full licensing and attribution details.                                 //
// .                                                                           //
/////////////////////////////////////////////////////////////////////////////////

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PaintDotNet
{
    /// <summary>
    /// Defines miscellaneous constants and static functions.
    /// </summary>
    public sealed class Utility
    {
        private Utility()
        {
        }

        internal static bool IsNumber(float x)
        {
            return x >= float.MinValue && x <= float.MaxValue;
        }

        internal static bool IsNumber(double x)
        {
            return x >= double.MinValue && x <= double.MaxValue;
        }

        public static void DrawDropShadow1px(Graphics g, Rectangle rect)
        {
            Brush b0 = new SolidBrush(Color.FromArgb(15, Color.Black));
            Brush b1 = new SolidBrush(Color.FromArgb(47, Color.Black));
            Pen p2 = new Pen(Color.FromArgb(63, Color.Black));

            g.FillRectangle(b0, rect.Left, rect.Top, 1, 1);
            g.FillRectangle(b1, rect.Left + 1, rect.Top, 1, 1);
            g.FillRectangle(b1, rect.Left, rect.Top + 1, 1, 1);

            g.FillRectangle(b0, rect.Right - 1, rect.Top, 1, 1);
            g.FillRectangle(b1, rect.Right - 2, rect.Top, 1, 1);
            g.FillRectangle(b1, rect.Right - 1, rect.Top + 1, 1, 1);

            g.FillRectangle(b0, rect.Left, rect.Bottom - 1, 1, 1);
            g.FillRectangle(b1, rect.Left + 1, rect.Bottom - 1, 1, 1);
            g.FillRectangle(b1, rect.Left, rect.Bottom - 2, 1, 1);

            g.FillRectangle(b0, rect.Right - 1, rect.Bottom - 1, 1, 1);
            g.FillRectangle(b1, rect.Right - 2, rect.Bottom - 1, 1, 1);
            g.FillRectangle(b1, rect.Right - 1, rect.Bottom - 2, 1, 1);

            g.DrawLine(p2, rect.Left + 2, rect.Top, rect.Right - 3, rect.Top);
            g.DrawLine(p2, rect.Left, rect.Top + 2, rect.Left, rect.Bottom - 3);
            g.DrawLine(p2, rect.Left + 2, rect.Bottom - 1, rect.Right - 3, rect.Bottom - 1);
            g.DrawLine(p2, rect.Right - 1, rect.Top + 2, rect.Right - 1, rect.Bottom - 3);

            b0.Dispose();
            b0 = null;
            b1.Dispose();
            b1 = null;
            p2.Dispose();
            p2 = null;
        }

        public static void DrawColorRectangle(Graphics g, Rectangle rect, Color color, bool drawBorder)
        {
            int inflateAmt = drawBorder ? -2 : 0;
            Rectangle colorRectangle = Rectangle.Inflate(rect, inflateAmt, inflateAmt);
            Brush colorBrush = new LinearGradientBrush(colorRectangle, Color.FromArgb(255, color), color, 90.0f, false);
            HatchBrush backgroundBrush = new HatchBrush(HatchStyle.LargeCheckerBoard, Color.FromArgb(191, 191, 191), Color.FromArgb(255, 255, 255));

            if (drawBorder)
            {
                g.DrawRectangle(Pens.Black, rect.Left, rect.Top, rect.Width - 1, rect.Height - 1);
                g.DrawRectangle(Pens.White, rect.Left + 1, rect.Top + 1, rect.Width - 3, rect.Height - 3);
            }

            PixelOffsetMode oldPOM = g.PixelOffsetMode;
            g.PixelOffsetMode = PixelOffsetMode.Half;
            g.FillRectangle(backgroundBrush, colorRectangle);
            g.FillRectangle(colorBrush, colorRectangle);
            g.PixelOffsetMode = oldPOM;

            backgroundBrush.Dispose();
            colorBrush.Dispose();
        }

        public static Font CreateFont(string name, float size, FontStyle style)
        {
            Font returnFont;

            try
            {
                returnFont = new Font(name, size, style);
            }

            catch (Exception)
            {
                returnFont = new Font(FontFamily.GenericSansSerif, size);
            }

            return returnFont;
        }

        public static Font CreateFont(string name, float size, string backupName, float backupSize, FontStyle style)
        {
            Font returnFont;

            try
            {
                returnFont = new Font(name, size, style);
            }

            catch (Exception)
            {
                returnFont = CreateFont(backupName, backupSize, style);
            }

            return returnFont;
        }

        public static byte ClampToByte(double x)
        {
            if (x > 255)
            {
                return 255;
            }
            else if (x < 0)
            {
                return 0;
            }
            else
            {
                return (byte)x;
            }
        }

        public static byte ClampToByte(float x)
        {
            if (x > 255)
            {
                return 255;
            }
            else if (x < 0)
            {
                return 0;
            }
            else
            {
                return (byte)x;
            }
        }

        public static byte ClampToByte(int x)
        {
            if (x > 255)
            {
                return 255;
            }
            else if (x < 0)
            {
                return 0;
            }
            else
            {
                return (byte)x;
            }
        }

        public static float Lerp(float from, float to, float frac)
        {
            return (from + frac * (to - from));
        }

        public static double Lerp(double from, double to, double frac)
        {
            return (from + frac * (to - from));
        }

        public static PointF Lerp(PointF from, PointF to, float frac)
        {
            return new PointF(Lerp(from.X, to.X, frac), Lerp(from.Y, to.Y, frac));
        }
        
        private static bool allowGCFullCollect = true;
        public static bool AllowGCFullCollect
        {
            get
            {
                return allowGCFullCollect;
            }

            set
            {
                allowGCFullCollect = value;
            }
        }

        public static void GCFullCollect()
        {
            if (AllowGCFullCollect)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        public static byte FastScaleByteByByte(byte a, byte b)
        {
            int r1 = a * b + 0x80;
            int r2 = ((r1 >> 8) + r1) >> 8;
            return (byte)r2;
        }
    }
}