using SketchIt.Api.Interfaces;
using SketchIt.Api.Static;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace SketchIt.Api
{
    public class ShapeParameters
    {
        public ShapeParameters(Shape shape, float x, float y)
        {
            Shape = shape;
            X = x;
            Y = y;
        }

        public float X
        {
            get;
            set;
        }

        public float Y
        {
            get;
            set;
        }

        public Shape Shape
        {
            get;
            set;
        }
    }

    public class ColorModeParameters
    {
        public ColorModeParameters(ColorMode mode, float? max1 = null, float? max2 = null, float? max3 = null, float? maxAlpha = null)
        {
            Mode = mode;
            Max1 = max1;
            Max2 = max2 ?? max1;
            Max3 = max3 ?? max1;
            MaxAlpha = maxAlpha;
        }

        public ColorMode Mode
        {
            get;
            set;
        }

        public float? Max1
        {
            get;
            set;
        }

        public float? Max2
        {
            get;
            set;
        }

        public float? Max3
        {
            get;
            set;
        }

        public float? MaxAlpha
        {
            get;
            set;
        }
    }

    public class TintParameters : ColorParameter
    {
        public System.Drawing.Imaging.ImageAttributes ImageAttributes { get; private set; }

        public TintParameters(Color color)
            : base(color)
        {
            ImageAttributes = new System.Drawing.Imaging.ImageAttributes();

            float[][] ptsArray = {
                //new float[] {1, 0, 0, 0, 0},
                //new float[] {0, 1, 0, 0, 0},
                //new float[] {0, 0, 1, 0, 0},
                //new float[] { color.Value1 / 255f, color.Value2 / 255f, color.Value3 / 255f, color.Alpha / 255f, 0},
                //new float[] {0, 0, 0, 0, 1}
                new float[] {color.Value1 / 255f, 0, 0, 0, 0},
                new float[] {0, color.Value2 / 255f, 0, 0, 0},
                new float[] {0, 0, color.Value3 / 255f, 0, 0},
                new float[] {0, 0, 0, color.Alpha / 255f, 0},
                new float[] {0, 0, 0, 0, 1}

                //inverse image
                //new float[] {-1, 0, 0, 0, 0},
                //new float[] {0, -1, 0, 0, 0},
                //new float[] {0, 0, -1, 0, 0},
                //new float[] {0, 0, 0, 1, 0},
                //new float[] {1, 1, 1, 0, 1}
            };

            ImageAttributes.SetColorMatrix(new System.Drawing.Imaging.ColorMatrix(ptsArray));
        }

        public override void Dispose()
        {
            if (ImageAttributes != null) ImageAttributes.Dispose();
            base.Dispose();
        }
    }

    public class ImageParameters
    {
        public ImageParameters(IImage image, float? x = null, float? y = null, float? width = null, float? height = null)
        {
            Image = image;
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public IImage Image
        {
            get;
            set;
        }

        public float? X
        {
            get;
            set;
        }

        public float? Y
        {
            get;
            set;
        }

        public float? Width
        {
            get;
            set;
        }

        public float? Height
        {
            get;
            set;
        }
    }

    public class QuadParameters
    {
        public QuadParameters(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
            X3 = x3;
            Y3 = y3;
            X4 = x4;
            Y4 = y4;
        }

        public float X1
        {
            get;
            set;
        }

        public float Y1
        {
            get;
            set;
        }

        public float X2
        {
            get;
            set;
        }

        public float Y2
        {
            get;
            set;
        }

        public float X3
        {
            get;
            set;
        }

        public float Y3
        {
            get;
            set;
        }

        public float X4
        {
            get;
            set;
        }

        public float Y4
        {
            get;
            set;
        }
    }

    public class ArcParameters
    {
        public ArcParameters(float x, float y, float width, float height, float start, float stop)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Start = start;
            Stop = stop;
        }

        public ArcMode Mode
        {
            get;
            set;
        }

        public float X
        {
            get;
            set;
        }

        public float Y
        {
            get;
            set;
        }

        public float Width
        {
            get;
            set;
        }

        public float Height
        {
            get;
            set;
        }

        public float Start
        {
            get;
            set;
        }

        public float Stop
        {
            get;
            set;
        }
    }

    public class EllipseParameters
    {
        public EllipseParameters(float x, float y, float diameter)
            : this(x, y, diameter, diameter)
        {
        }

        public EllipseParameters(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public float X
        {
            get;
            set;
        }

        public float Y
        {
            get;
            set;
        }

        public float Width
        {
            get;
            set;
        }

        public float Height
        {
            get;
            set;
        }
    }

    public class LineParameters
    {
        public LineParameters(float x1, float y1, float x2, float y2)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }

        public float X1
        {
            get;
            set;
        }

        public float Y1
        {
            get;
            set;
        }

        public float X2
        {
            get;
            set;
        }

        public float Y2
        {
            get;
            set;
        }
    }

    public class PointParameters
    {
        public PointParameters(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X
        {
            get;
            set;
        }

        public float Y
        {
            get;
            set;
        }
    }

    public class RectangleParameters
    {
        public RectangleParameters(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public float X
        {
            get;
            set;
        }

        public float Y
        {
            get;
            set;
        }

        public float Width
        {
            get;
            set;
        }

        public float Height
        {
            get;
            set;
        }
    }

    public class ColorParameter : IDisposable
    {
        public IImage Image
        {
            get;
            private set;
        }

        private Color _color;
        private Brush _brush;
        private Pen _pen;
        private float _penWidth = 1;
        private bool _disabled;

        internal Style Style
        {
            get;
            set;
        }

        public ColorParameter(IImage image)
            : this(new Color())
        {
            Image = image;
        }

        public ColorParameter(Color color)
        {
            Color = color;
        }

        public float PenWidth
        {
            get { return _penWidth; }
            set
            {
                _penWidth = value;
                _pen = null;
            }
        }

        public bool Disabled
        {
            get { return _disabled; }
            set { _disabled = value; }
        }

        public Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
                _pen = null;
                _brush = null;
                _disabled = false;
            }
        }

        public Brush ToBrush()
        {
            if (_brush == null)
            {
                if (Image != null)
                {
                    _brush = new TextureBrush(Image.Bitmap, new RectangleF(0, 0, Image.Width, Image.Height), Style != null && Style.TintParameters.Disabled ? new System.Drawing.Imaging.ImageAttributes() : Style.TintParameters.ImageAttributes);
                }
                else
                {
                    _brush = new SolidBrush(_color.ToSystemColor());
                    //_brush = new LinearGradientBrush(new PointF(0, 0), new PointF(10, 10), _color, Color.White);
                }
            }

            return _brush;
        }

        public Pen ToPen()
        {
            if (_pen == null)
            {
                if (Image != null)
                {
                    _pen = new Pen(new TextureBrush(Image.Bitmap, new RectangleF(0, 0, Image.Width, Image.Height), Style != null && Style.TintParameters.Disabled ? new System.Drawing.Imaging.ImageAttributes() : Style.TintParameters.ImageAttributes));
                }
                else
                {
                    _pen = new Pen(_color.ToSystemColor(), _penWidth);
                }
            }

            return _pen;
        }

        public virtual void Dispose()
        {
            if (_pen != null)
            {
                if (_pen.Brush != null)
                {
                    _pen.Brush.Dispose();
                }

                _pen.Dispose();
            }

            if (_brush != null)
            {
                _brush.Dispose();
            }
        }
    }

    public class BackgroundParameters : ColorParameter
    {
        public BackgroundParameters(Color color) : base(color) { }
        public BackgroundParameters(IImage image) : base(image) { }
    }

    public class FillParameters : ColorParameter
    {
        public FillParameters(Color color) : base(color) { }
        public FillParameters(IImage image) : base(image) { }
    }

    public class StrokeParameters : ColorParameter
    {
        public StrokeParameters(Color color) : base(color) { }
        public StrokeParameters(IImage image) : base(image) { }
    }

    public class TextParameters
    {
        public string Text { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public TextParameters(object text, float x, float y)
            : this(text, x, y, 0, 0)
        {
        }

        public TextParameters(object text, float x, float y, float width, float height)
        {
            Text = text.ToString();
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }

    public class FontParameters
    {
        public string Name { get; set; }
        public float? Size { get; set; }
        public bool? Bold { get; set; }
        public bool? Italic { get; set; }

        public FontParameters(string name, float size)
            : this(name, size, null, null)
        {
        }

        public FontParameters(string name, float? size, bool? bold, bool? italic)
        {
            Name = name;
            Size = size;
            Bold = bold;
            Italic = italic;
        }
    }

    public class BezierParameters : QuadParameters
    {
        public BezierParameters(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
            : base(x1, y1, x2, y2, x3, y3, x4, y4)
        {
        }
    }

    public class CurveParameters
    {
        public Point[] Points { get; private set; }

        public CurveParameters(params float[] points)
        {
            List<Point> pts = new List<Point>();

            for (int i = 0; i < points.Length; i += 2)
            {
                pts.Add(new Point(points[i], points[i + 1]));
            }

            Points = pts.ToArray();
        }

        public CurveParameters(params Point[] points)
        {
            Points = points;
        }
    }
}
