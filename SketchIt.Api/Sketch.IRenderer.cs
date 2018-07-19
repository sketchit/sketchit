using SketchIt.Api.Interfaces;
using SketchIt.Api.Static;

namespace SketchIt.Api
{
    public partial class Sketch : IRenderer
    {
        public void Clear()
        {
            ((IRenderer)Renderer).Clear();
        }

        public void DrawBackground(BackgroundParameters parms)
        {
            ((IRenderer)Renderer).DrawBackground(parms);
        }

        public void DrawArc(ArcParameters parms)
        {
            ((IRenderer)Renderer).DrawArc(parms);
        }

        public void DrawEllipse(EllipseParameters parms)
        {
            ((IRenderer)Renderer).DrawEllipse(parms);
        }

        public void DrawLine(LineParameters parms)
        {
            ((IRenderer)Renderer).DrawLine(parms);
        }

        public void DrawPoint(PointParameters parms)
        {
            ((IRenderer)Renderer).DrawPoint(parms);
        }

        public void DrawBezier(BezierParameters parms)
        {
            ((IRenderer)Renderer).DrawBezier(parms);
        }

        public void DrawCurve(CurveParameters parms)
        {
            ((IRenderer)Renderer).DrawCurve(parms);
        }

        public void DrawQuad(QuadParameters parms)
        {
            ((IRenderer)Renderer).DrawQuad(parms);
        }

        public void DrawRectangle(RectangleParameters parms)
        {
            ((IRenderer)Renderer).DrawRectangle(parms);
        }

        public void DrawText(TextParameters parms)
        {
            ((IRenderer)Renderer).DrawText(parms);
        }

        public void DrawImage(ImageParameters parms)
        {
            ((IRenderer)Renderer).DrawImage(parms);
        }

        public void DrawShape(ShapeParameters parms)
        {
            ((IRenderer)Renderer).DrawShape(parms);
        }

        public void PushMatrix()
        {
            ((IRenderer)Renderer).PushMatrix();
        }

        public void PopMatrix()
        {
            ((IRenderer)Renderer).PopMatrix();
        }

        public void ResetMatrix()
        {
            ((IRenderer)Renderer).ResetMatrix();
        }

        public void Scale(float x, float y)
        {
            ((IRenderer)Renderer).Scale(x, y);
        }

        public void Translate(float x, float y)
        {
            ((IRenderer)Renderer).Translate(x, y);
        }

        public void Rotate(float angle)
        {
            ((IRenderer)Renderer).Rotate(angle);
        }

        public void BeginDraw()
        {
            ((IRenderer)Renderer).BeginDraw();
        }

        public void EndDraw()
        {
            ((IRenderer)Renderer).EndDraw();
        }

        public void BeginShape(ShapeKind kind)
        {
            ((IRenderer)Renderer).BeginShape(kind);
        }

        public void EndShape(EndShapeMode mode)
        {
            ((IRenderer)Renderer).EndShape(mode);
        }

        public void Vertex(float x, float y, float z, float u, float v)
        {
            ((IRenderer)Renderer).Vertex(x, y, z, u, v);
        }

        public void Texture(IImage image)
        {
            ((IRenderer)Renderer).Texture(image);
        }
    }
}
