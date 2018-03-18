namespace UserGuiLib.Common.Algebra
{
    public struct Rect
    {
        public Vector2 Location;
        public Vector2 Size;

        public float Left => Location.x;
        public float Right => Location.x + Size.x;
        public float Top => Location.y;
        public float Bottom => Location.y + Size.y;

        public float Width => Size.x;
        public float Height => Size.y;

        public Vector2 PointAtOffset(Vector2 offset)
        {
            return Location + new Vector2(Size.x * offset.x, Size.y * offset.y);
        }

        public bool Contains(Vector2 vec)
        {
            var bottomRight = Location + Size;
            return Location.x <= vec.x && Location.y <= vec.y && bottomRight.x >= vec.x && bottomRight.y >= vec.y;
        }

        public bool IntersectsWith(Rect other)
        {
            return Left < other.Right && Right > other.Left && Top < other.Bottom && Bottom > other.Top;
        }

        public Rect(Vector2 location, Vector2 size)
        {
            Location = location;
            Size = size;
            if (size.x < 0)
            {
                Size.x *= -1;
                Location.x -= Size.x;
            }
            if (size.y < 0)
            {
                Size.y *= -1;
                Location.y -= Size.y;
            }
        }
    }
}
