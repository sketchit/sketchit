using SketchIt.Api.Interfaces;
using SketchIt.Api.Static;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace SketchIt.Api.Renderers
{
    /// <summary>
    /// The default GDI+ renderer implementation.
    /// </summary>
    public class GdiPlusRenderer : RendererBase
    {
        private class DeviceContextHandler : IDisposable
        {
            public Graphics DrawingSurface { get; private set; }
            private GdiPlusRenderer _renderer;
            private bool _initialized = false;

            public DeviceContextHandler(GdiPlusRenderer renderer)
            {
                this._renderer = renderer;

                if (renderer._drawingSurface == null)
                {
                    _initialized = true;
                    _renderer.IRenderer.BeginDraw();
                }

                DrawingSurface = renderer._drawingSurface;
            }

            void IDisposable.Dispose()
            {
                if (_initialized)
                {
                    _renderer.IRenderer.EndDraw();
                }

                DrawingSurface = null;
            }
        }

        private Graphics _drawingSurface;
        private Stack<GraphicsContainer> _matrixStack = new Stack<GraphicsContainer>();

        public IRenderer IRenderer => this;
        public GdiPlusRenderer(Canvas canvas)
            : base(canvas)
        {
        }

        public override void BeginDraw()
        {
            if (_drawingSurface == null)
            {
                _drawingSurface = Graphics.FromImage(Canvas.Bitmap);
                SetGraphicsOptions();
            }

            //ResetMatrix();
        }

        public override void EndDraw()
        {
            if (_drawingSurface != null)
            {
                _drawingSurface.Dispose();
            }

            _drawingSurface = null;
        }

        private void SetGraphicsOptions()
        {
            _drawingSurface.SmoothingMode = SmoothingMode.HighQuality;
            _drawingSurface.InterpolationMode = InterpolationMode.HighQualityBicubic;
            _drawingSurface.PixelOffsetMode = PixelOffsetMode.HighQuality;
            _drawingSurface.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            //_graphics.CompositingMode = CompositingMode.SourceOver;
            //_graphics.CompositingQuality = CompositingQuality.AssumeLinear;

            //ColorMatrix colorMatrix = new ColorMatrix(new float[][] {
            //            new float[] { 1.0f, 0.0f, 0.0f, 0.0f, 0.0f },
            //            new float[] { 0.0f, 1.0f, 0.0f, 0.0f, 0.0f },
            //            new float[] { 0.0f, 1.0f, 1.0f, 0.0f, 0.0f },
            //            new float[] { 0.0f, 0.0f, 0.0f, 0.8f, 0.0f },
            //            new float[] { 0.0f, 0.0f, 0.0f, 0.0f, 1.0f }
            //        });

            //ImageAttributes imageAtt = new ImageAttributes();
            //imageAtt.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
        }

        private DeviceContextHandler GetDeviceContextHandler()
        {
            return new DeviceContextHandler(this);
        }

        private GraphicsPath GetShapePath(Shape shape)
        {
            GraphicsPath path = new GraphicsPath();
            List<PointF> points = new List<PointF>();
            Vertex[] vertices = shape.Vertices;

            for (int i = 0; i < vertices.Length; i++)
            {
                Vertex v = shape.Vertices[i];

                switch (shape.Kind)
                {
                    case ShapeKind.Points:
                        path.AddEllipse(v.X, v.Y, 1, 1);
                        break;

                    case ShapeKind.Lines:
                        if ((i + 1) % 2 == 0)
                        {
                            path.AddLine(vertices[i - 1].X, vertices[i - 1].Y, vertices[i - 0].X, vertices[i - 0].Y);
                            path.StartFigure();
                        }
                        break;

                    case ShapeKind.Triangles:
                        if ((i + 1) % 3 == 0)
                        {
                            path.AddLine(vertices[i - 2].X, vertices[i - 2].Y, vertices[i - 1].X, vertices[i - 1].Y);
                            path.AddLine(vertices[i - 1].X, vertices[i - 1].Y, vertices[i - 0].X, vertices[i - 0].Y);
                            path.AddLine(vertices[i - 0].X, vertices[i - 0].Y, vertices[i - 2].X, vertices[i - 2].Y);
                            path.StartFigure();
                        }
                        break;

                    case ShapeKind.TriangleStrip:
                        if (i >= 2)
                        {
                            path.AddLine(vertices[i - 2].X, vertices[i - 2].Y, vertices[i - 1].X, vertices[i - 1].Y);
                            path.AddLine(vertices[i - 1].X, vertices[i - 1].Y, vertices[i - 0].X, vertices[i - 0].Y);
                            path.AddLine(vertices[i - 0].X, vertices[i - 0].Y, vertices[i - 2].X, vertices[i - 2].Y);
                            path.StartFigure();
                        }
                        break;

                    case ShapeKind.Quads:
                        if ((i + 1) % 4 == 0)
                        {
                            path.AddLine(vertices[i - 3].X, vertices[i - 3].Y, vertices[i - 2].X, vertices[i - 2].Y);
                            path.AddLine(vertices[i - 2].X, vertices[i - 2].Y, vertices[i - 1].X, vertices[i - 1].Y);
                            path.AddLine(vertices[i - 1].X, vertices[i - 1].Y, vertices[i - 0].X, vertices[i - 0].Y);
                            path.AddLine(vertices[i - 0].X, vertices[i - 0].Y, vertices[i - 3].X, vertices[i - 3].Y);
                            path.StartFigure();
                        }
                        break;

                    case ShapeKind.QuadStrip:
                        if (i >= 3 && (i + 1) % 2 == 0)
                        {
                            path.AddLine(vertices[i - 3].X, vertices[i - 3].Y, vertices[i - 2].X, vertices[i - 2].Y);
                            path.AddLine(vertices[i - 3].X, vertices[i - 3].Y, vertices[i - 1].X, vertices[i - 1].Y);
                            path.AddLine(vertices[i - 1].X, vertices[i - 1].Y, vertices[i - 0].X, vertices[i - 0].Y);
                            path.AddLine(vertices[i - 0].X, vertices[i - 0].Y, vertices[i - 2].X, vertices[i - 2].Y);
                            path.StartFigure();
                        }
                        break;

                    default:
                        points.Add(new PointF(v.X, v.Y));
                        break;
                }
            }

            switch (shape.Kind)
            {
                case ShapeKind.Points:
                case ShapeKind.Lines:
                case ShapeKind.Triangles:
                case ShapeKind.TriangleStrip:
                case ShapeKind.Quads:
                case ShapeKind.QuadStrip:
                    break;

                default:
                    switch (shape.EndMode)
                    {
                        case EndShapeMode.Close:
                            path.AddPolygon(points.ToArray());
                            break;

                        default:
                            path.AddLines(points.ToArray());
                            break;
                    }
                    break;
            }

            return path;
        }

        public override void DrawArc(ArcParameters parms)
        {
            using (DeviceContextHandler dch = GetDeviceContextHandler())
            {
                Rectangle rect = Style.GetAdjustedRectangle(parms);

                if (!Style.FillParameters.Disabled)
                {
                    dch.DrawingSurface.FillPie(Style.FillParameters.ToBrush(), rect.X, rect.Y, rect.Width, rect.Height, parms.Start, parms.Stop);
                }

                if (!Style.StrokeParameters.Disabled)
                {
                    dch.DrawingSurface.DrawArc(Style.StrokeParameters.ToPen(), rect.X, rect.Y, rect.Width, rect.Height, parms.Start, parms.Stop);
                }
            }
        }

        public override void DrawBezier(BezierParameters parms)
        {
            using (DeviceContextHandler dch = GetDeviceContextHandler())
            {
                if (!Style.FillParameters.Disabled)
                {
                    dch.DrawingSurface.DrawBezier(Style.StrokeParameters.ToPen(), parms.X1, parms.Y1, parms.X2, parms.Y2, parms.X3, parms.Y3, parms.X4, parms.Y4);
                }
            }
        }

        public override void DrawCurve(CurveParameters parms)
        {
            using (DeviceContextHandler dch = GetDeviceContextHandler())
            {
                if (!Style.FillParameters.Disabled)
                {
                    List<PointF> pts = new List<PointF>();

                    foreach (Point pt in parms.Points)
                    {
                        pts.Add(pt.ToSystemPointF());
                    }

                    dch.DrawingSurface.DrawCurve(Style.StrokeParameters.ToPen(), pts.ToArray());
                }
            }
        }

        public override void DrawEllipse(EllipseParameters parms)
        {
            using (DeviceContextHandler dch = GetDeviceContextHandler())
            {
                Rectangle rect = Style.GetAdjustedRectangle(parms);

                if (!Style.FillParameters.Disabled)
                {
                    dch.DrawingSurface.FillEllipse(Style.FillParameters.ToBrush(), rect.X, rect.Y, rect.Width, rect.Height);
                }

                if (!Style.StrokeParameters.Disabled)
                {
                    dch.DrawingSurface.DrawEllipse(Style.StrokeParameters.ToPen(), rect.X, rect.Y, rect.Width, rect.Height);
                }
            }
        }

        public override void DrawImage(ImageParameters parms)
        {
            using (DeviceContextHandler dch = GetDeviceContextHandler())
            {
                try
                {
                    Rectangle rect = Style.GetAdjustedRectangle(parms);

                    if (!Style.TintParameters.Disabled)
                    {
                        dch.DrawingSurface.DrawImage(parms.Image.Bitmap, rect.SystemRectangle, 0, 0, parms.Image.Width, parms.Image.Height, GraphicsUnit.Pixel, Style.TintParameters.ImageAttributes);
                    }
                    else
                    {
                        dch.DrawingSurface.DrawImage(parms.Image.Bitmap, rect.SystemRectangleF);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message + Environment.NewLine + e.StackTrace);
                }
            }
        }

        //public override void DrawLine(LineParameters parms)
        //{
        //    if (!Style.StrokeParameters.Disabled)
        //    {
        //        using (DeviceContextHandler dch = GetDeviceContextHandler())
        //        {
        //            dch.DrawingSurface.DrawLine(Style.StrokeParameters.ToPen(), parms.X1, parms.Y1, parms.X2, parms.Y2);
        //        }
        //    }
        //}

        public override void DrawPoint(PointParameters parms)
        {
            if (!Style.StrokeParameters.Disabled)
            {
                float w = Style.StrokeParameters.PenWidth;
                float x = parms.X;
                float y = parms.Y;

                using (DeviceContextHandler dch = GetDeviceContextHandler())
                {
                    dch.DrawingSurface.FillEllipse(Style.StrokeParameters.ToBrush(), x - w / 2, y - w / 2, w, w);
                }
            }
        }

        //public override void DrawQuad(QuadParameters parms)
        //{
        //    using (DeviceContextHandler dch = GetDeviceContextHandler())
        //    using (GraphicsPath path = new GraphicsPath())
        //    {
        //        PointF[] points = new PointF[]
        //        {
        //            new PointF(parms.X1, parms.Y1),
        //            new PointF(parms.X2, parms.Y2),
        //            new PointF(parms.X3, parms.Y3),
        //            new PointF(parms.X4, parms.Y4)
        //        };

        //        path.AddPolygon(points);

        //        if (!Style.FillParameters.Disabled)
        //        {
        //            dch.DrawingSurface.FillPath(Style.FillParameters.ToBrush(), path);
        //        }

        //        if (!Style.StrokeParameters.Disabled)
        //        {
        //            dch.DrawingSurface.DrawPath(Style.StrokeParameters.ToPen(), path);
        //        }
        //    }
        //}

        //public override void DrawRectangle(RectangleParameters parms)
        //{
        //    using (DeviceContextHandler dch = GetDeviceContextHandler())
        //    {
        //        RectangleF rect = Style.GetAdjustedRectangle(parms);

        //        if (!Style.FillParameters.Disabled)
        //        {
        //            dch.DrawingSurface.FillRectangle(Style.FillParameters.ToBrush(), rect.X, rect.Y, rect.Width, rect.Height);
        //        }

        //        if (!Style.StrokeParameters.Disabled)
        //        {
        //            dch.DrawingSurface.DrawRectangle(Style.StrokeParameters.ToPen(), rect.X, rect.Y, rect.Width, rect.Height);
        //        }
        //    }
        //}

        public override void DrawShape(ShapeParameters parms)
        {
            using (DeviceContextHandler dch = GetDeviceContextHandler())
            using (GraphicsPath path = GetShapePath(parms.Shape))
            {
                PushMatrix();
                Translate(parms.X, parms.Y);

                if (!Style.FillParameters.Disabled)
                {
                    dch.DrawingSurface.FillPath(Style.FillParameters.ToBrush(), path);
                }

                if (!Style.StrokeParameters.Disabled)
                {
                    dch.DrawingSurface.DrawPath(Style.StrokeParameters.ToPen(), path);
                }

                if (parms.Shape.Texture != null)
                {
                    PointF tl = new PointF(path.PathPoints[0].X, path.PathPoints[0].Y);
                    PointF tr = new PointF(path.PathPoints[1].X, path.PathPoints[1].Y);
                    PointF bl = new PointF(path.PathPoints[3].X, path.PathPoints[3].Y);
                    PointF br = new PointF(path.PathPoints[2].X, path.PathPoints[2].Y);

                    using (Image texture = ((Image)parms.Shape.Texture).ToTrapezoid(tl, tr, bl, br))
                    {
                        dch.DrawingSurface.DrawImage(texture.Bitmap, new PointF());
                    }
                }

                PopMatrix();
            }
        }

        public override void DrawText(TextParameters parms)
        {
            if (!Style.StrokeParameters.Disabled)
            {
                using (DeviceContextHandler dch = GetDeviceContextHandler())
                {
                    Rectangle rect = new Rectangle(parms.X, parms.Y, parms.Width, parms.Height);
                    //TextFormatFlags tff = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter | TextFormatFlags.WordBreak;
                    //TextRenderer.DrawText(_graphics, parms.Text, _currentFont, rect, _currentStroke.Color, tff);

                    StringFormat sf = new StringFormat();
                    sf.LineAlignment =
                        Style.TextVerticalAlignment == VerticalAlignment.Bottom ? StringAlignment.Far :
                        Style.TextVerticalAlignment == VerticalAlignment.Top ? StringAlignment.Near :
                        StringAlignment.Center;
                    sf.Alignment =
                        Style.TextHorizontalAlignment == HorizontalAlignment.Right ? StringAlignment.Far :
                        Style.TextHorizontalAlignment == HorizontalAlignment.Left ? StringAlignment.Near :
                        StringAlignment.Center;
                    dch.DrawingSurface.DrawString(parms.Text, Style.Font, Style.StrokeParameters.ToBrush(), rect.SystemRectangleF, sf);
                }
            }
        }

        public override void Clear()
        {
            //_graphics.ClearPixles();
            //return;
            using (DeviceContextHandler dch = GetDeviceContextHandler())
            {
                dch.DrawingSurface.Clear(System.Drawing.Color.Transparent);
            }
        }

        public override void DrawBackground(BackgroundParameters parms)
        {
            using (DeviceContextHandler dch = GetDeviceContextHandler())
            {
                if (parms.Image != null)
                {
                    dch.DrawingSurface.DrawImage(parms.Image.Bitmap, 0, 0, Canvas.Width, Canvas.Height);
                }
                else
                {
                    //dch.Graphics.FillRectangle(parms.ToBrush(), 0, 0, _graphics.Width, _graphics.Height);
                    dch.DrawingSurface.Clear(parms.Color.ToSystemColor());
                }
            }
        }

        public override void PushMatrix()
        {
            using (DeviceContextHandler dch = GetDeviceContextHandler())
            {
                _matrixStack.Push(dch.DrawingSurface.BeginContainer());
                SetGraphicsOptions();
            }
        }

        public override void PopMatrix()
        {
            using (DeviceContextHandler dch = GetDeviceContextHandler())
            {
                dch.DrawingSurface.EndContainer(_matrixStack.Pop());
            }
        }

        public override void Translate(float x, float y)
        {
            using (DeviceContextHandler dch = GetDeviceContextHandler())
            {
                dch.DrawingSurface.TranslateTransform(x, y);
            }
        }

        public override void Rotate(float angle)
        {
            using (DeviceContextHandler dch = GetDeviceContextHandler())
            {
                dch.DrawingSurface.RotateTransform(angle);
            }
        }

        public override void Scale(float x, float y)
        {
            using (DeviceContextHandler dch = GetDeviceContextHandler())
            {
                dch.DrawingSurface.ScaleTransform(x, y);
            }
        }

        public override void ResetMatrix()
        {
            using (DeviceContextHandler dch = GetDeviceContextHandler())
            {
                dch.DrawingSurface.ResetTransform();
            }
        }

        public override void SetSize(float width, float height)
        {
            if (_drawingSurface != null)
            {
                _drawingSurface = null;
                BeginDraw();
            }
        }

        //public override void Tint(TintParameters parms)
        //{
        //    using (DeviceContextHandler dch = GetDeviceContextHandler())
        //    using (ImageAttributes attributes = new ImageAttributes())
        //    {
        //        Bitmap image = Canvas.Bitmap;
        //        float[][] ptsArray = {
        //            new float[] {1, 0, 0, 0, 0},
        //            new float[] {0, 1, 0, 0, 0},
        //            new float[] {0, 0, 1, 0, 0},
        //            new float[] { parms.Color.Value1 / 255f, parms.Color.Value2 / 255f, parms.Color.Value3 / 255f, parms.Color.Alpha / 255f, 0},
        //            new float[] {0, 0, 0, 0, 1}
        //        };

        //        attributes.SetColorMatrix(new ColorMatrix(ptsArray));
        //        dch.DrawingSurface.DrawImage(image, new System.Drawing.Rectangle(0, 0, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
        //    }
        //}
    }
}
