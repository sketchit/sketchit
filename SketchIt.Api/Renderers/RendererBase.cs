using SketchIt.Api.Interfaces;
using SketchIt.Api.Internal;
using SketchIt.Api.Static;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SketchIt.Api.Renderers
{
    /// <summary>
    /// Provides the abstract base class for a renderer.
    /// </summary>
    public abstract class RendererBase : IRenderer, IRendererExtended
    {
        protected RendererBase(Canvas canvas)
        {
            Canvas = canvas;
            _shapeStack = new Stack<Shape>();
        }

        private Shape _currentShape;
        private Stack<Shape> _shapeStack = new Stack<Shape>();

        public Style Style
        {
            get => Canvas.Style;
        }

        #region IRenderer
        /// <summary>
        /// Called when drawing is about to begin.
        /// </summary>
        public virtual void BeginDraw() => NotImplemented();

        /// <summary>
        /// Begins recording vertices of a new shape.
        /// </summary>
        /// <param name="kind">The shape type to be recorded.</param>
        public virtual void BeginShape(ShapeKind kind)
        {
            _shapeStack.Push(new Shape(kind));
            _currentShape = _shapeStack.Peek();
        }

        /// <summary>
        /// Clears the background of the canvas.
        /// </summary>
        public virtual void Clear() => NotImplemented();

        public virtual void DrawArc(ArcParameters parms)
        {
            int sign = Math.Sign(parms.Stop - parms.Start);
            int count = 2 * sign;
            Rectangle rect = Style.GetAdjustedRectangle(parms);
            float theta = 2 * Constants.PI / (360f / count);
            float c = Functions.Cos(theta);
            float s = Functions.Sin(theta);
            float xx = parms.Width / 2;
            float yy = 0;
            float t;
            float ii = parms.Start;
            bool done = false;
            Point start = Point.Empty;
            Point point = Point.Empty;

            BeginShape(ShapeKind.Polygon);

            if (parms.Mode == ArcMode.Pie)
            {
                Vertex(parms.X, parms.Y);
            }

            while (true)
            {
                point.X = xx + (rect.X + (rect.Width / 2));
                point.Y = yy + (rect.Y + (rect.Height / 2));

                if (start.IsEmpty)
                {
                    start = point;
                }

                Vertex(point.X, point.Y);

                //apply the rotation matrix
                t = xx;
                xx = c * xx - s * yy;
                yy = s * t + c * yy;

                ii += count;

                switch (sign)
                {
                    case -1:
                        if (ii < parms.Stop) done = true;
                        break;

                    default:
                        if (ii > parms.Stop) done = true;
                        break;
                }

                if (done) break;
            }

            if (parms.Mode == ArcMode.Pie)
            {
                Vertex(parms.X, parms.Y);
            }

            EndShape(parms.Mode == ArcMode.Open ? EndShapeMode.Open : EndShapeMode.Close);
        }

        public virtual void DrawBackground(BackgroundParameters parms) => NotImplemented();
        public virtual void DrawEllipse(EllipseParameters parms)
        {
            int count = 4;
            Rectangle rect = Style.GetAdjustedRectangle(parms);
            float theta = 2 * Constants.PI / (360f / count);
            float c = Functions.Cos(theta);
            float s = Functions.Sin(theta);
            float xx = rect.Width / 2;
            float yy = 0;
            float t;

            BeginShape(ShapeKind.Polygon);
            for (int ii = 0; ii <= 360; ii += count)
            {
                //apply radius and offset
                Vertex(xx + (rect.X + (rect.Width / 2)), yy + (rect.Y + (rect.Height / 2)));

                //apply the rotation matrix
                t = xx;
                xx = c * xx - s * yy;
                yy = s * t + c * yy;
            }
            EndShape(EndShapeMode.Close);
        }

        public virtual void DrawImage(ImageParameters parms) => NotImplemented();

        public virtual void DrawLine(LineParameters parms)
        {
            BeginShape(ShapeKind.Lines);
            Vertex(parms.X1, parms.Y1);
            Vertex(parms.X2, parms.Y2);
            EndShape(EndShapeMode.Open);
        }

        public virtual void DrawPoint(PointParameters parms)
        {
            Color fillColor = Style.FillParameters.Color;
            EllipseMode ellipseMode = Style.EllipseMode;

            Style.EllipseMode = EllipseMode.Center;
            Style.FillParameters.Color = Style.StrokeParameters.Color;
            DrawEllipse(new EllipseParameters(parms.X, parms.Y, Style.StrokeParameters.PenWidth));
            Style.EllipseMode = ellipseMode;
            Style.FillParameters.Color = fillColor;
        }

        public virtual void DrawQuad(QuadParameters parms)
        {
            BeginShape(ShapeKind.Polygon);
            Vertex(parms.X1, parms.Y1);
            Vertex(parms.X2, parms.Y2);
            Vertex(parms.X3, parms.Y3);
            Vertex(parms.X4, parms.Y4);
            EndShape(EndShapeMode.Close);
        }

        public virtual void DrawRectangle(RectangleParameters parms)
        {
            Rectangle rect = Style.GetAdjustedRectangle(parms);
            BeginShape(ShapeKind.Polygon);
            Vertex(rect.X, rect.Y);
            Vertex(rect.X + rect.Width, rect.Y);
            Vertex(rect.X + rect.Width, rect.Y + rect.Height);
            Vertex(rect.X, rect.Y + rect.Height);
            EndShape(EndShapeMode.Close);
        }

        public virtual void DrawBezier(BezierParameters parms) => NotImplemented();
        public virtual void DrawShape(ShapeParameters parms) => NotImplemented();
        public virtual void DrawCurve(CurveParameters parms) => NotImplemented();
        public virtual void DrawText(TextParameters parms) => NotImplemented();

        /// <summary>
        /// Called when drawing has completed.
        /// </summary>
        public virtual void EndDraw() => NotImplemented();

        /// <summary>
        /// Ends recording of the current shape.
        /// </summary>
        /// <param name="mode"></param>
        public virtual void EndShape(EndShapeMode mode)
        {
            _currentShape.End(mode);
            DrawShape(new ShapeParameters(_currentShape, 0, 0));

            _shapeStack.Pop();

            if (_shapeStack.Count > 0)
            {
                _currentShape = _shapeStack.Peek();
            }
            else
            {
                _currentShape = null;
            }
        }

        public virtual void PopMatrix() => NotImplemented();
        public virtual void PushMatrix() => NotImplemented();
        public virtual void ResetMatrix() => NotImplemented();
        public virtual void Rotate(float angle) => NotImplemented();
        public virtual void Scale(float x, float y) => NotImplemented();
        public virtual void SetSize(float width, float height) => NotImplemented();

        public virtual void Texture(IImage image)
        {
            _currentShape.SetTexture(image);
        }


        public virtual void Translate(float x, float y) => NotImplemented();

        public virtual void Vertex(float x, float y, float z, float u, float v)
        {
            _currentShape.Vertex(x, y, z, u, v);
        }

        public Canvas Canvas
        {
            get;
            private set;
        }

        public virtual void SetTint(TintParameters parms) => NotImplemented();
        public virtual void SetNoTint() => NotImplemented();
        #endregion

        #region IRendererExtended
        public void DrawArc(float x, float y, float width, float height, float start, float stop) => DrawArc(new ArcParameters(x, y, width, height, start, stop));
        public void DrawEllipse(float x, float y, float diameter) => DrawEllipse(new EllipseParameters(x, y, diameter));
        public void DrawEllipse(float x, float y, float width, float height) => DrawEllipse(new EllipseParameters(x, y, width, height));
        public void DrawLine(float x1, float y1, float x2, float y2) => DrawLine(new LineParameters(x1, y1, x2, y2));
        public void DrawPoint(float x, float y) => DrawPoint(new PointParameters(x, y));
        public void DrawQuad(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4) => DrawQuad(new QuadParameters(x1, y1, x2, y2, x3, y3, x4, y4));
        public void DrawBezier(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4) => DrawBezier(new BezierParameters(x1, y1, x2, y2, x3, y3, x4, y4));
        public void DrawCurve(params float[] points) => DrawCurve(new CurveParameters(points));
        public void DrawCurve(params Point[] points) => DrawCurve(new CurveParameters(points));
        public void DrawRectangle(float x, float y, float size) => DrawRectangle(new RectangleParameters(x, y, size, size));
        public void DrawRectangle(float x, float y, float width, float height) => DrawRectangle(new RectangleParameters(x, y, width, height));
        public void DrawText(object text, float x, float y) => DrawText(new TextParameters(text, x, y));
        public void DrawText(object text, float x, float y, float width, float height) => DrawText(new TextParameters(text, x, y, width, height));
        public void DrawImage(IImage image, float x, float y) => DrawImage(new ImageParameters(image, x, y));
        public void DrawImage(IImage image, float x, float y, float width, float height) => DrawImage(new ImageParameters(image, x, y, width, height));
        public void DrawBackground(IImage image) => DrawBackground(new BackgroundParameters(image));
        public void DrawBackground(float gray) => DrawBackground(new BackgroundParameters(Style.GetColor(gray)));
        public void DrawBackground(float gray, float alpha) => DrawBackground(new BackgroundParameters(Style.GetColor(gray, alpha)));
        public void DrawBackground(float r, float g, float b) => DrawBackground(new BackgroundParameters(Style.GetColor(r, g, b)));
        public void DrawBackground(float r, float g, float b, float alpha) => DrawBackground(new BackgroundParameters(Style.GetColor(r, g, b, alpha)));
        public void DrawBackground(Color color, float alpha) => DrawBackground(new BackgroundParameters(new Color(color, alpha)));
        public void DrawBackground(Color color) => DrawBackground(new BackgroundParameters(color));
        public void BeginShape() => BeginShape(ShapeKind.Polygon);
        public void Vertex(float x, float y) => Vertex(x, y, 0, 0, 0);
        public void SetTint(float gray) => SetTint(new TintParameters(Style.GetColor(gray)));
        public void SetTint(float gray, float alpha) => SetTint(new TintParameters(Style.GetColor(gray, alpha)));
        public void SetTint(float r, float g, float b) => SetTint(new TintParameters(Style.GetColor(r, g, b)));
        public void SetTint(float r, float g, float b, float alpha) => SetTint(new TintParameters(Style.GetColor(r, g, b, alpha)));
        public void SetTint(Color color, float alpha) => SetTint(new TintParameters(new Color(color, alpha)));
        public void SetTint(Color color) => SetTint(new TintParameters(color));
        #endregion

        /// <summary>
        /// Logs a message to the console when a call is made to a method that has not been implemented.
        /// </summary>
        private void NotImplemented()
        {
            using (ThreadLocker.Lock())
            {
                StackTrace stack = new StackTrace();
                string method = stack.GetFrame(1).GetMethod().Name;
                string type = this.GetType().Name;
                Console.WriteLine(string.Format("Method not implemented: {0}.{1}", type, method));
            }
        }
    }
}
