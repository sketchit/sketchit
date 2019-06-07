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
        private bool _drawing = false;
        private bool _drawn = false;
        private bool _perspective = true;
        private int _matrixStack = 0;

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

            _openGL.ClearColor(1.0f, 0.0f, 0.0f, 1.0f);

            _openGL.Enable(OpenGL.GL_DEPTH_TEST);
            _openGL.DepthFunc(OpenGL.GL_LEQUAL);
            _openGL.ClearDepth(1.0f);

            //_openGL.Hint(OpenGL.GL_PERSPECTIVE_CORRECTION_HINT, OpenGL.GL_NICEST);

            _openGL.Enable(OpenGL.GL_BLEND);
            _openGL.BlendFunc(BlendingSourceFactor.SourceAlpha, BlendingDestinationFactor.OneMinusSourceAlpha);

            _openGL.Enable(OpenGL.GL_LINE_SMOOTH);
            _openGL.Hint(OpenGL.GL_LINE_SMOOTH_HINT, OpenGL.GL_NICEST);
            _openGL.ShadeModel(OpenGL.GL_SMOOTH);

            defCameraFOV = 60f;
            defCameraX = Canvas.Width / 2f;
            defCameraY = Canvas.Height / 2f;
            defCameraZ = defCameraY / ((float)Math.Tan((defCameraFOV / 360f * Constants.TWO_PI) / 2f));
            defCameraNear = defCameraZ / 10f;
            defCameraFar = defCameraZ * 10f;
            defCameraAspect = (float)Canvas.Width / (float)Canvas.Height;

            _openGL.Enable(OpenGL.GL_COLOR_MATERIAL);
            _openGL.ColorMaterial(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_AMBIENT_AND_DIFFUSE);
            _openGL.Material(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_SPECULAR, new float[] { 1, 1, 1, 1 });
            _openGL.Material(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_EMISSION, new float[] { 0, 0, 0, 1 });
            _openGL.Material(OpenGL.GL_FRONT, OpenGL.GL_SHININESS, 128);

            _openGL.Light(LightName.Light0, LightParameter.Ambient, new float[] { 0.2f, 0.2f, 0.2f, 1.0f });
            _openGL.Light(LightName.Light0, LightParameter.Diffuse, new float[] { 0.8f, 0.8f, 0.8f, 1.0f });
            _openGL.Light(LightName.Light0, LightParameter.Specular, new float[] { 0.5f, 0.5f, 0.5f, 1.0f });
            _openGL.Light(LightName.Light0, LightParameter.Position, new float[] { -1.5f, 1.0f, 4.0f, 1 });
            _openGL.Light(LightName.Light0, LightParameter.SpotCutoff, 180.0f);

            SetPerspective();
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
            if (_drawing)
            {
                throw new Exception();
            }

            _drawn = false;
            _drawing = true;
            _openGL.MakeCurrent();
            ResetMatrix();
            _openGL.PushMatrix();

            if (_perspective)
            {
                //SetPerspective();
            }
            else
            {
                //SetOrtho();
            }
        }

        public override void DrawBackground(BackgroundParameters parms)
        {
            float[] rgba = parms.Color.GetNormalizedValues();
            _openGL.ClearColor(rgba[0], rgba[1], rgba[2], rgba[3]);

            if (rgba[3] != 1)
            {
                StrokeParameters stroke = Canvas.StrokeParameters;
                FillParameters fill = Canvas.FillParameters;
                bool strokeDisabled = stroke.Disabled;
                bool fillDisabled = fill.Disabled;

                Canvas.SetNoStroke();
                Canvas.SetFill(parms.Color);
                DrawRectangle(0, 0, Canvas.Width, Canvas.Height);

                stroke.Disabled = strokeDisabled;
                fill.Disabled = fillDisabled;
                Canvas.SetStroke(stroke);
                Canvas.SetFill(fill);
            }
            else
            {
                _openGL.Clear(OpenGL.GL_COLOR_BUFFER_BIT);
            }
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

        public override void DrawImage(ImageParameters parms)
        {
            //uint[] gtexture = new uint[1];
            //System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, parms.Image.Width, parms.Image.Height);
            //System.Drawing.Imaging.BitmapData gbitmapdata = parms.Image.Bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            //parms.Image.Bitmap.UnlockBits(gbitmapdata);
            //_openGL.GenTextures(1, gtexture);
            //_openGL.BindTexture(OpenGL.GL_TEXTURE_2D, gtexture[0]);
            //_openGL.TexImage2D(OpenGL.GL_TEXTURE_2D, 0, (int)OpenGL.GL_RGB8, parms.Image.Bitmap.Width, parms.Image.Bitmap.Height, 0, OpenGL.GL_BGR_EXT, OpenGL.GL_UNSIGNED_BYTE, gbitmapdata.Scan0);

            //uint[] array = new uint[] { OpenGL.GL_NEAREST };
            //_openGL.TexParameterI(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MIN_FILTER, array);
            //_openGL.TexParameterI(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MAG_FILTER, array);

            //_openGL.Enable(OpenGL.GL_TEXTURE_2D);
            //_openGL.BindTexture(OpenGL.GL_TEXTURE_2D, gtexture[0]);
            //_openGL.Color(1.0f, 1.0f, 1.0f, 0.1f); //Must have, weirdness!
            //_openGL.Begin(OpenGL.GL_QUADS);
            //_openGL.TexCoord(1.0f, 1.0f);
            //_openGL.Vertex(parms.Width ?? parms.Image.Width, parms.Height ?? parms.Image.Height, 1.0f);
            //_openGL.TexCoord(0.0f, 1.0f);
            //_openGL.Vertex(0.0f, parms.Height ?? parms.Image.Height, 1.0f);
            //_openGL.TexCoord(0.0f, 0.0f);
            //_openGL.Vertex(0.0f, 0.0f, 1.0f);
            //_openGL.TexCoord(1.0f, 0.0f);
            //_openGL.Vertex(parms.Width ?? parms.Image.Width, 0.0f, 1.0f);
            //_openGL.End();
            //_openGL.Disable(OpenGL.GL_TEXTURE_2D);
        }

        public override void EndDraw()
        {
            //if (Canvas.Sketch.Graphics != null)
            //{
            //    IntPtr dc = Canvas.Sketch.Graphics.GetHdc();
            //    _openGL.Blit(dc);
            //    Canvas.Sketch.Graphics.ReleaseHdc(dc);
            //}

            _openGL.PopMatrix();
            _drawing = false;
            _drawn = true;
        }

        public override void Flush()
        {
            if (_drawn && Canvas.Sketch.Graphics != null)
            {
                IntPtr dc = Canvas.Sketch.Graphics.GetHdc();
                _openGL.Flush();
                _openGL.Blit(dc);
                Canvas.Sketch.Graphics.ReleaseHdc(dc);
            }
        }

        public override void PopMatrix()
        {
            _openGL.PopMatrix();
            _matrixStack--;
        }

        public override void PushMatrix()
        {
            _openGL.PushMatrix();
            _matrixStack++;
        }

        public override void ResetMatrix()
        {
            while (_matrixStack > 0)
            {
                PopMatrix();
            }

            if (_perspective)
            {
                SetPerspective();
            }
            else
            {
                SetOrtho();
            }
        }

        public override void SetPerspective()
        {
            _perspective = true;
            _openGL.Clear(OpenGL.GL_DEPTH_BUFFER_BIT);

            _openGL.MatrixMode(MatrixMode.Projection);
            _openGL.LoadIdentity();
            _openGL.Perspective(defCameraFOV, defCameraAspect, defCameraNear, defCameraFar);

            Translate(-defCameraX, -defCameraY, -defCameraZ);

            _openGL.Scale(Canvas.Sketch.Zoom, Canvas.Sketch.Zoom, Canvas.Sketch.Zoom);
        }

        public override void SetOrtho()
        {
            _perspective = false;
            _openGL.Clear(OpenGL.GL_DEPTH_BUFFER_BIT);

            //_openGL.MatrixMode(MatrixMode.Projection);
            //_openGL.LoadIdentity();

            _openGL.MatrixMode(MatrixMode.Modelview);
            _openGL.LoadIdentity();
            _openGL.Ortho(-Canvas.Width / 2, Canvas.Width / 2, -Canvas.Height / 2, Canvas.Height / 2, defCameraNear, defCameraFar);

            Translate(-Canvas.Width / 2, -Canvas.Height / 2, -defCameraZ);

            _openGL.Scale(Canvas.Sketch.Zoom, Canvas.Sketch.Zoom, Canvas.Sketch.Zoom);
        }

        public override void SetLights()
        {
            //_openGL.MatrixMode(MatrixMode.Modelview);
            //_openGL.LoadIdentity();
            //_openGL.Light(LightName.Light0, LightParameter.Position, new float[] { -1.5f, 1.0f, -4.0f, -1 });

            //_openGL.Light(LightName.Light0, LightParameter.Ambient, new float[] { 0.2f, 0.2f, 0.2f, 1.0f });
            //_openGL.Light(LightName.Light0, LightParameter.Diffuse, new float[] { 0.8f, 0.8f, 0.8f, 1.0f });
            //_openGL.Light(LightName.Light0, LightParameter.Specular, new float[] { 0.5f, 0.5f, 0.5f, 1.0f });
            //_openGL.Light(LightName.Light0, LightParameter.Position, new float[] { -1.5f, 1.0f, 4.0f, 1 });
            //_openGL.Light(LightName.Light0, LightParameter.SpotCutoff, 180.0f);
            _openGL.Enable(OpenGL.GL_LIGHTING);
            _openGL.Enable(OpenGL.GL_LIGHT0);

            //if (_perspective)
            //{
            //    SetPerspective();
            //}
            //else
            //{
            //    SetOrtho();
            //}
        }

        private void Rotate(float angleX, float angleY, float angleZ)
        {
            if (Functions.AngleMode == AngleMode.Degrees)
            {
                _openGL.Rotate(angleX, angleY, angleZ);
            }
            else
            {
                _openGL.Rotate(Functions.Degrees(angleX), Functions.Degrees(angleY), Functions.Degrees(angleZ));
            }
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
