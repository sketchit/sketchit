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
                        return filled ? BeginMode.Quads : BeginMode.QuadStrip;

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
        private float defCameraFOV;
        private float defCameraX;
        private float defCameraY;
        private float defCameraZ;
        private float defCameraNear;
        private float defCameraFar;
        private float defCameraAspect;

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
            _openGL.Create(OpenGLVersion.OpenGL2_1, RenderContextType.FBO, Canvas.Width, Canvas.Height, 32, Canvas.Sketch.Container.WindowHandle);

            _openGL.ShadeModel(OpenGL.GL_SMOOTH);
            _openGL.ClearColor(1.0f, 0.0f, 0.0f, 1.0f);
            _openGL.ClearDepth(1.0f);
            _openGL.Enable(OpenGL.GL_DEPTH_TEST);
            _openGL.DepthFunc(OpenGL.GL_LEQUAL);
            _openGL.Hint(OpenGL.GL_PERSPECTIVE_CORRECTION_HINT, OpenGL.GL_NICEST);

            _openGL.Enable(OpenGL.GL_BLEND);
            _openGL.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            _openGL.Enable(OpenGL.GL_LINE_SMOOTH);
            _openGL.Hint(OpenGL.GL_LINE_SMOOTH_HINT, OpenGL.GL_NICEST);

            defCameraFOV = 60f;
            defCameraX = Canvas.Width / 2f;
            defCameraY = Canvas.Height / 2f;
            defCameraZ = defCameraY / ((float)Math.Tan((defCameraFOV / 360f * Constants.TWO_PI) / 2f));
            defCameraNear = defCameraZ / 10f;
            defCameraFar = defCameraZ * 10f;
            defCameraAspect = (float)Canvas.Width / (float)Canvas.Height;

            SetPerspective();
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
            _openGL.LineWidth(Canvas.Style.StrokeParameters.PenWidth * Canvas.Sketch.Zoom);
            _openGL.Color(rgba[0], rgba[1], rgba[2], rgba[3]);
        }

        private void AddShapeVertices(Shape shape, bool filled)
        {
            _openGL.Begin(EnumConverter.ToBeginMode(shape.Kind, filled));

            foreach (Vertex v in shape.Vertices)
            {
                _openGL.Vertex(v.X, v.Y * -1, v.Z);
            }

            //_openGL.Vertex(shape.Vertices[0].X, shape.Vertices[0].Y * -1, shape.Vertices[0].Z);

            _openGL.End();
        }

        public override void BeginDraw()
        {
            _openGL.MakeCurrent();
            ResetMatrix();
            _openGL.PushMatrix();
            _openGL.Scale(Canvas.Sketch.Zoom, Canvas.Sketch.Zoom, Canvas.Sketch.Zoom);
        }

        public override void DrawBackground(BackgroundParameters parms)
        {
            float[] rgba = parms.Color.GetNormalizedValues();
            _openGL.ClearColor(rgba[0], rgba[1], rgba[2], rgba[3]);
            _openGL.Clear(OpenGL.GL_COLOR_BUFFER_BIT);
        }

        public override void DrawShape(ShapeParameters parms)
        {
            PushMatrix();
            Translate(parms.X, parms.Y);

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

            PopMatrix();
        }

        public override void EndDraw()
        {
            if (Canvas.Sketch.Graphics != null)
            {
                IntPtr dc = Canvas.Sketch.Graphics.GetHdc();
                _openGL.Blit(dc);
                Canvas.Sketch.Graphics.ReleaseHdc(dc);
            }

            _openGL.PopMatrix();
        }

        public override void PopMatrix()
        {
            _openGL.PopMatrix();
        }

        public override void PushMatrix()
        {
            _openGL.PushMatrix();
        }

        public override void ResetMatrix()
        {
            _openGL.Clear(OpenGL.GL_DEPTH_BUFFER_BIT);
        }

        public override void SetPerspective()
        {
            _openGL.Clear(OpenGL.GL_DEPTH_BUFFER_BIT);

            _openGL.MatrixMode(MatrixMode.Modelview);
            _openGL.LoadIdentity();

            _openGL.MatrixMode(MatrixMode.Projection);
            _openGL.LoadIdentity();
            _openGL.Perspective(defCameraFOV, defCameraAspect, defCameraNear, defCameraFar);
            _openGL.Scale(Canvas.Sketch.Zoom, Canvas.Sketch.Zoom, Canvas.Sketch.Zoom);

            Translate(-defCameraX, -defCameraY, -defCameraZ);
        }

        public override void SetOrtho()
        {
            _openGL.Clear(OpenGL.GL_DEPTH_BUFFER_BIT);

            _openGL.MatrixMode(MatrixMode.Projection);
            _openGL.LoadIdentity();

            _openGL.MatrixMode(MatrixMode.Modelview);
            _openGL.LoadIdentity();
            _openGL.Ortho(-Canvas.Width / 2, Canvas.Width / 2, -Canvas.Height / 2, Canvas.Height / 2, defCameraNear, defCameraFar);
            _openGL.Scale(Canvas.Sketch.Zoom, Canvas.Sketch.Zoom, Canvas.Sketch.Zoom);

            Translate(-Canvas.Width / 2, -Canvas.Height / 2, -defCameraZ);
        }

        private void Rotate(float angleX, float angleY, float angleZ)
        {
            _openGL.Rotate(angleX, angleY, angleZ);
        }

        public override void Rotate(float angle)
        {
            Rotate(0, 0, angle);
        }

        public override void RotateX(float angle)
        {
            Rotate(angle, 0, 0);
        }

        public override void RotateY(float angle)
        {
            Rotate(0, angle, 0);
        }

        public override void RotateZ(float angle)
        {
            Rotate(0, 0, angle);
        }

        public override void SetSize(float width, float height)
        {
            InitializeOpenGL();
        }

        public override void Translate(float x, float y)
        {
            Translate(x, y, 0);
        }

        public override void Translate(float x, float y, float z)
        {
            //x = Functions.Map(x, 0, Canvas.Width, 0, 2);
            //y = Functions.Map(y, 0, Canvas.Height, 0, -2);
            _openGL.Translate(x, -y, z);
        }
    }
}
