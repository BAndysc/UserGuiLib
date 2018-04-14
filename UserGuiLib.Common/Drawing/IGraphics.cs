using UserGuiLib.Common.Algebra;

namespace UserGuiLib.Common.Drawing
{
    public interface IGraphics
    {
        void DrawRectangle(AnyPen pen, Vector2 p1, Vector2 p2, float radius = 0);

        void FillRectangle(AnyPen pen, Vector2 p1, Vector2 p2, float radius = 0);

        void DrawLine(AnyPen pen, Vector2 p1, Vector2 p2);

        void DrawString(string text, AnyPen pen, AnyFont font, Vector2 center, Vector2 anchor, float maxWidth = 0);

        void FillCircle(AnyPen pen, Vector2 center, Vector2 anchor, float radius);

        void DrawCircle(AnyPen pen, Vector2 center, Vector2 anchor, float radius);

        void FillSquare(AnyPen pen, Vector2 center, Vector2 anchor, float length);

        void DrawSquare(AnyPen pen, Vector2 center, Vector2 anchor, float length);

        void DrawImage(AnyImage image, Vector2 point, Vector2 destSize, Vector2 anchor);

        Vector2 MeasureString(string text, AnyFont font, float maxWidth = 0);

        void Translate(Vector2 offset);
        void Scale(Vector2 scale);
        void Clip(Vector2 point1, Vector2 point2);

        void ResetTransform();

        void ResetClip();
    }
}
