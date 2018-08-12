using SketchIt.Api.Interfaces;

namespace SketchIt.Api
{
    public partial class Sketch : IRendererExtended
    {
        public void BeginShape()
        {
            ((IRendererExtended)Renderer).BeginShape();
        }

        public void DrawArc(float x, float y, float width, float height, float start, float stop)
        {
            ((IRendererExtended)Renderer).DrawArc(x, y, width, height, start, stop);
        }

        public void DrawEllipse(float x, float y, float diameter)
        {
            ((IRendererExtended)Renderer).DrawEllipse(x, y, diameter);
        }

        public void DrawEllipse(float x, float y, float width, float height)
        {
            ((IRendererExtended)Renderer).DrawEllipse(x, y, width, height);
        }

        public void DrawBezier(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
        {
            ((IRendererExtended)Renderer).DrawBezier(x1, y1, x2, y2, x3, y3, x4, y4);
        }

        public void DrawCurve(params float[] points)
        {
            ((IRendererExtended)Renderer).DrawCurve(points);
        }

        public void DrawCurve(params Point[] points)
        {
            ((IRendererExtended)Renderer).DrawCurve(points);
        }

        public void DrawLine(float x1, float y1, float x2, float y2)
        {
            ((IRendererExtended)Renderer).DrawLine(x1, y1, x2, y2);
        }

        public void DrawPoint(float x, float y)
        {
            ((IRendererExtended)Renderer).DrawPoint(x, y);
        }

        public void DrawQuad(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
        {
            ((IRendererExtended)Renderer).DrawQuad(x1, y1, x2, y2, x3, y3, x4, y4);
        }

        public void DrawRectangle(float x, float y, float size)
        {
            ((IRendererExtended)Renderer).DrawRectangle(x, y, size);
        }

        public void DrawRectangle(float x, float y, float width, float height)
        {
            ((IRendererExtended)Renderer).DrawRectangle(x, y, width, height);
        }

        public void DrawText(object text, float x, float y)
        {
            ((IRendererExtended)Renderer).DrawText(text, x, y);
        }

        public void DrawText(object text, float x, float y, float width, float height)
        {
            ((IRendererExtended)Renderer).DrawText(text, x, y, width, height);
        }

        public void DrawImage(IImage image, float x, float y)
        {
            ((IRendererExtended)Renderer).DrawImage(image, x, y);
        }

        public void DrawImage(IImage image, float x, float y, float width, float height)
        {
            ((IRendererExtended)Renderer).DrawImage(image, x, y, width, height);
        }

        public void DrawBackground(IImage image)
        {
            ((IRendererExtended)Renderer).DrawBackground(image);
        }

        public void DrawBackground(float gray)
        {
            ((IRendererExtended)Renderer).DrawBackground(gray);
        }

        public void DrawBackground(float gray, float alpha)
        {
            ((IRendererExtended)Renderer).DrawBackground(gray, alpha);
        }

        public void DrawBackground(float r, float g, float b)
        {
            ((IRendererExtended)Renderer).DrawBackground(r, g, b);
        }

        public void DrawBackground(float r, float g, float b, float alpha)
        {
            ((IRendererExtended)Renderer).DrawBackground(r, g, b, alpha);
        }

        public void DrawBackground(Color color, float alpha)
        {
            ((IRendererExtended)Renderer).DrawBackground(color, alpha);
        }

        public void DrawBackground(Color color)
        {
            ((IRendererExtended)Renderer).DrawBackground(color);
        }

        public void Vertex(float x, float y)
        {
            ((IRendererExtended)Renderer).Vertex(x, y);
        }

        public void Vertex(float x, float y, float z)
        {
            ((IRendererExtended)Renderer).Vertex(x, y, z);
        }

        public void Translate(float x, float y)
        {
            ((IRendererExtended)Renderer).Translate(x, y);
        }

        public void Scale(float x, float y)
        {
            ((IRendererExtended)Renderer).Scale(x, y);
        }
    }
}
