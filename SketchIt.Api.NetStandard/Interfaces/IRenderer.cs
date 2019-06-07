using SketchIt.Api.Static;

namespace SketchIt.Api.Interfaces
{
    /// <summary>
    /// The IRenderer interface is responsible for drawing.
    /// </summary>
    public interface IRenderer
    {
        void Clear();
        void DrawBackground(BackgroundParameters parms);
        void DrawArc(ArcParameters parms);
        void DrawEllipse(EllipseParameters parms);
        void DrawLine(LineParameters parms);
        void DrawPoint(PointParameters parms);
        void DrawQuad(QuadParameters parms);
        void DrawRectangle(RectangleParameters parms);
        void DrawText(TextParameters parms);
        void DrawImage(ImageParameters parms);
        void DrawShape(ShapeParameters parms);
        void DrawBezier(BezierParameters parms);
        void DrawCurve(CurveParameters parms);
        void DrawBox(BoxParameters parms);

        void PushMatrix();
        void PopMatrix();
        void ResetMatrix();
        void Scale(float x, float y, float z);
        void Translate(float x, float y, float z);
        void Rotate(float angle);
        void RotateX(float angle);
        void RotateY(float angle);
        void RotateZ(float angle);
        void SetSize(float width, float height);

        void BeginDraw();
        void EndDraw();

        void BeginShape(ShapeKind kind);
        void EndShape(EndShapeMode mode);
        void Vertex(float x, float y, float z, float u, float v);
        void Texture(IImage image);

        void SetPerspective();
        void SetOrtho();
        void SetLights();

        void Flush();

        Canvas Canvas { get; }
    }
}
