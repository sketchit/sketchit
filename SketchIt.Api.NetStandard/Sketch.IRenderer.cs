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

        public void DrawBox(BoxParameters parms)
        {
            ((IRenderer)Renderer).DrawBox(parms);
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

        public void Scale(float x, float y, float z)
        {
            ((IRenderer)Renderer).Scale(x, y, z);
        }

        public void Translate(float x, float y, float z)
        {
            ((IRenderer)Renderer).Translate(x, y, z);
        }

        public void Rotate(float angle)
        {
            ((IRenderer)Renderer).Rotate(angle);
        }

        public void RotateX(float angle)
        {
            ((IRenderer)Renderer).RotateX(angle);
        }

        public void RotateY(float angle)
        {
            ((IRenderer)Renderer).RotateY(angle);
        }

        public void RotateZ(float angle)
        {
            ((IRenderer)Renderer).RotateZ(angle);
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

        public void SetPerspective()
        {
            ((IRenderer)Renderer).SetPerspective();
        }

        public void SetOrtho()
        {
            ((IRenderer)Renderer).SetOrtho();
        }

        public void SetLights()
        {
            ((IRenderer)Renderer).SetLights();
        }

        public void Flush()
        {
            ((IRenderer)Renderer).Flush();
        }
    }
}
