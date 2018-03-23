namespace UserGuiLib.Common.Drawing
{
    public struct AnyPen
    {
        public byte a { get; }
        public byte r { get; }
        public byte g { get; }
        public byte b { get; }

        public float Width { get; }

        public AnyPen(byte a, byte r, byte g, byte b, float width)
        {
            this.a = a;
            this.b = b;
            this.g = g;
            this.r = r;
            this.Width = width;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is AnyPen))
                return false;

            AnyPen other = (obj as AnyPen?).Value;

            return other.a == a && other.b == b && other.r == r && other.g == g && other.Width == Width;
        }

        public override int GetHashCode()
        {
            return Width.GetHashCode() * 37 + ((a << 24) | (r << 16) | (g << 8) | b);
        }
    }

    public struct AnyFont
    {
        public string FontName { get; }
        public float Size { get; }
        
        public AnyFont(string name, float size)
        {
            FontName = name;
            Size = size;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is AnyFont))
                return false;

            var other = (obj as AnyFont?).Value;

            return other.FontName == FontName && other.Size == Size;
        }

        public override int GetHashCode()
        {
            return FontName.GetHashCode() * 37 + Size.GetHashCode();
        }
    }

    public struct AnyImage
    {
        public string FilePath { get; }

        public AnyImage(string filePath)
        {
            FilePath = filePath;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is AnyImage))
                return false;

            var other = (obj as AnyImage?).Value;

            return FilePath.Equals(other.FilePath);
        }

        public override int GetHashCode()
        {
            return FilePath.GetHashCode();
        }
    }
}
