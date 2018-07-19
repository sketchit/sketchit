using SharpGL;
using SharpGL.Enumerations;
using SharpGL.Version;
using SketchIt.Api.Interfaces;
using SketchIt.Api.Static;
using System;

namespace SketchIt.Api.Renderers
{
    /// <summary>
    /// An experimental implementation of an OpenGL renderer using the SharpGL library.
    /// One of the main concerns is that I am OpenGL illiterate. Someone, please implement a proper
    /// OpenGL renderer.
    /// </summary>
    public class SharpGLRenderer : RendererBase
    {
        private static class EnumConverter
        {
            public static BeginMode ToBeginMode(ShapeKind shapeKind, bool filled)
            {
                switch (shapeKind)
                {
                    case ShapeKind.Lines:
                        return BeginMode.Lines;

                    case ShapeKind.Points:
                        return BeginMode.Points;

                    case ShapeKind.Polygon:
                        return filled ? BeginMode.Polygon : BeginMode.LineStrip;

                    case ShapeKind.Quads:
                        return BeginMode.Quads;

                    case ShapeKind.QuadStrip:
                        return BeginMode.QuadStrip;

                    case ShapeKind.TriangleFan:
                        return BeginMode.TriangleFan;

                    case ShapeKind.Triangles:
                        return BeginMode.Triangles;

                    case ShapeKind.TriangleStrip:
                        return BeginMode.TriangleString;

                    default:
                        return BeginMode.Polygon;
                }
            }
        }

        private Sketch _sketch;
        private OpenGL _openGL;

        public IRenderer IRenderer => this;
        public SharpGLRenderer(Canvas canvas)
            : base(canvas)
        {
            _sketch = canvas.Sketch;

            if (_sketch == null) return;
            if (_sketch.Container == null) return;

            InitializeOpenGL();
        }

        private void InitializeOpenGL()
        {
            _openGL = new OpenGL();
            _openGL.Create(OpenGLVersion.OpenGL2_1, RenderContextType.FBO, Canvas.Width, Canvas.Height, 32, null);
            //_openGL.Viewport(0, 0, Canvas.Width, Canvas.Height);

            _openGL.ShadeModel(OpenGL.GL_SMOOTH);
            _openGL.ClearColor(1.0f, 0.0f, 0.0f, 1.0f);
            _openGL.ClearDepth(1.0f);
            _openGL.Enable(OpenGL.GL_DEPTH_TEST);
            _openGL.DepthFunc(OpenGL.GL_LEQUAL);
            _openGL.Hint(OpenGL.GL_PERSPECTIVE_CORRECTION_HINT, OpenGL.GL_NICEST);

            _openGL.Enable(OpenGL.GL_BLEND);
            _openGL.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            //_openGL.BlendFunc(BlendingSourceFactor.SourceAlpha, BlendingDestinationFactor.DestinationAlpha);
            _openGL.Enable(OpenGL.GL_LINE_SMOOTH);
            _openGL.Hint(OpenGL.GL_LINE_SMOOTH_HINT, OpenGL.GL_NICEST);

            _openGL.Clear(OpenGL.GL_DEPTH_BUFFER_BIT);
            //_openGL.Ortho2D(0, Canvas.Width, Canvas.Height, 0);
            _openGL.Viewport(0, 0, Canvas.Width, Canvas.Height);
            _openGL.MatrixMode(MatrixMode.Projection);
            _openGL.LoadIdentity();
            _openGL.Ortho(-1, 1, -1, 1, -1, 1);
        }

        private float MapX(float x)
        {
            return Functions.Map(x, 0, Canvas.Width, -1, 1);
        }

        private float MapY(float y)
        {
            return Functions.Map(y, 0, Canvas.Height, 1, -1);
        }

        private void SetFill()
        {
            float[] rgba = Canvas.Style.FillParameters.Color.GetNormalizedValues();
            _openGL.Color(rgba[0], rgba[1], rgba[2], rgba[3]);
        }

        private void SetStroke()
        {
            float[] rgba = Canvas.Style.StrokeParameters.Color.GetNormalizedValues();
            _openGL.LineWidth(Canvas.Style.StrokeParameters.PenWidth);
            _openGL.Color(rgba[0], rgba[1], rgba[2], rgba[3]);
        }

        private void AddShapeVertices(Shape shape, bool filled)
        {
            _openGL.Begin(EnumConverter.ToBeginMode(shape.Kind, filled));

            float x;
            float y;

            foreach (Vertex v in shape.Vertices)
            {
                x =  v.X / Canvas.Width * 2 - 1.0f;
                y = 1.0f - v.Y / Canvas.Height * 2;

                _openGL.Vertex(x, y, v.Z);
            }

            x = shape.Vertices[0].X / Canvas.Width * 2 - 1.0f;
            y = 1.0f - shape.Vertices[0].Y / Canvas.Height * 2;
            _openGL.Vertex(x, y, shape.Vertices[0].Z);

            _openGL.End();
        }

        public override void BeginDraw()
        {
            _openGL.MakeCurrent();
            _openGL.PushMatrix();
        }

        public override void DrawBackground(BackgroundParameters parms)
        {
            float[] rgba = parms.Color.GetNormalizedValues();
            _openGL.ClearColor(rgba[0], rgba[1], rgba[2], rgba[3]);
            _openGL.Clear(OpenGL.GL_COLOR_BUFFER_BIT);
        }

        //public override void DrawLine(LineParameters parms)
        //{
        //    SetStroke();
        //    _openGL.Begin(BeginMode.Lines);
        //    _openGL.Vertex(parms.X1, parms.Y1);
        //    _openGL.Vertex(parms.X2, parms.Y2);
        //    _openGL.End();
        //}

        //public override void DrawRectangle(RectangleParameters parms)
        //{
        //    RectangleF rect = Canvas.Style.GetAdjustedRectangle(parms);

        //    if (!Canvas.FillParameters.Disabled)
        //    {
        //        SetFill();
        //        _openGL.Begin(BeginMode.Polygon);
        //        _openGL.Vertex(rect.X, rect.Y);
        //        _openGL.Vertex(rect.Right, rect.Y);
        //        _openGL.Vertex(rect.Right, rect.Bottom);
        //        _openGL.Vertex(rect.X, rect.Bottom);
        //        _openGL.End();
        //    }

        //    if (!Canvas.StrokeParameters.Disabled)
        //    {
        //        SetStroke();
        //        _openGL.Begin(BeginMode.LineLoop);
        //        _openGL.Vertex(rect.X, rect.Y);
        //        _openGL.Vertex(rect.Right, rect.Y);
        //        _openGL.Vertex(rect.Right, rect.Bottom);
        //        _openGL.Vertex(rect.X, rect.Bottom);
        //        _openGL.End();
        //    }
        //}

        public override void DrawShape(ShapeParameters parms)
        {
            if (!Style.FillParameters.Disabled && parms.Shape.EndMode == EndShapeMode.Close)
            {
                SetFill();
                AddShapeVertices(parms.Shape, true);
            }

            if (!Style.StrokeParameters.Disabled)
            {
                SetStroke();
                AddShapeVertices(parms.Shape, false);
            }
        }

        public override void EndDraw()
        {
            if (Canvas.Sketch.Graphics != null)
            {
                IntPtr dc = Canvas.Sketch.Graphics.GetHdc();
                _openGL.Blit(dc);
                _openGL.PopMatrix();
                Canvas.Sketch.Graphics.ReleaseHdc(dc);
            }
        }

        public override void PopMatrix()
        {
            _openGL.PopMatrix();
        }

        public override void PushMatrix()
        {
            _openGL.PushMatrix();
        }

        public override void Rotate(float angle)
        {
            float x = (Canvas.Width / 2) + 1;
            float y = (Canvas.Height / 2) - 1;

            IRenderer.Translate(-x, -y);
            _openGL.Rotate(0, 0, -angle);
            IRenderer.Translate(x, y);
        }

        public override void SetSize(float width, float height)
        {
            InitializeOpenGL();
        }

        public override void Translate(float x, float y)
        {
            x = MapX(x) + 1;
            y = MapY(y) - 1;
            _openGL.Translate(x, y, 0);
        }
    }
}
