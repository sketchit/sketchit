using SketchIt.Api.Interfaces;
using SketchIt.Api.Static;
using System.Drawing;

namespace SketchIt.Api
{
    public class ColorRange
    {
        private float _defaultMax1 = 255f;
        private float _defaultMax2 = 255f;
        private float _defaultMax3 = 255f;
        private float _defaultMaxAlpha = 255f;

        /// <summary>
        /// Red / Hue
        /// </summary>
        public float Max1 { get; set; }

        /// <summary>
        /// Green / Saturation
        /// </summary>
        public float Max2 { get; set; }

        /// <summary>
        /// Blue / Brightness
        /// </summary>
        public float Max3 { get; set; }

        /// <summary>
        /// Alpha
        /// </summary>
        public float MaxAlpha { get; set; }

        public ColorRange()
        {
            Max1 = 255;
            Max2 = 255;
            Max3 = 255;
            MaxAlpha = 255;
        }

        public bool IsDefault
        {
            get
            {
                return !(Max1 != _defaultMax1 || Max2 != _defaultMax2 || Max3 != _defaultMax3 || MaxAlpha != _defaultMaxAlpha);
            }
        }

        internal void SetDefaults(float max1, float max2, float max3, float maxAlpha)
        {
            _defaultMax1 = Max1 = max1;
            _defaultMax2 = Max2 = max2;
            _defaultMax3 = Max3 = max3;
            _defaultMaxAlpha = MaxAlpha = maxAlpha;
        }
    }

    public class Style : IStyle
    {
        public HorizontalAlignment TextHorizontalAlignment { get; set; } = HorizontalAlignment.Left;
        public VerticalAlignment TextVerticalAlignment { get; set; } = VerticalAlignment.Top;
        public ColorMode ColorMode { get; set; }
        public RectangleMode RectangleMode { get; set; }
        public EllipseMode EllipseMode { get; set; }
        public ColorRange ColorRange { get; private set; }
        public ImageMode ImageMode { get; set; }
        public FillParameters FillParameters { get; private set; }
        public StrokeParameters StrokeParameters { get; private set; }
        public TintParameters TintParameters { get; private set; }
        public Font Font { get; private set; }

        public Style()
        {
            ColorRange = new ColorRange();
            ColorMode = ColorMode.Rgb;
            EllipseMode = EllipseMode.Center;
            RectangleMode = RectangleMode.Corner;
            ImageMode = ImageMode.Corner;

            FillParameters = new FillParameters(new Color(255, 255, 255, 255));
            StrokeParameters = new StrokeParameters(new Color(0, 0, 0, 255));
            TintParameters = new TintParameters(new Color()) { Disabled = true };
            Font = new Font("Arial", 10f);
        }

        public void SetEllipseMode(int mode) => SetEllipseMode((EllipseMode)mode);
        public void SetEllipseMode(EllipseMode mode)
        {
            EllipseMode = mode;
        }

        public void SetRectangleMode(int mode) => SetRectangleMode((RectangleMode)mode);
        public void SetRectangleMode(RectangleMode mode)
        {
            RectangleMode = mode;
        }

        public void SetImageMode(int mode) => SetImageMode((ImageMode)mode);
        public void SetImageMode(ImageMode mode)
        {
            ImageMode = mode;
        }

        public void SetTextAlignment(int horizontal, int vertical) => SetTextAlignment((HorizontalAlignment)horizontal, (VerticalAlignment)vertical);
        public void SetTextAlignment(HorizontalAlignment horizontal, VerticalAlignment vertical)
        {
            TextHorizontalAlignment = horizontal;
            TextVerticalAlignment = vertical;
        }

        public void SetFill(FillParameters parms)
        {
            if (FillParameters != null) FillParameters.Dispose();
            FillParameters = parms;
            FillParameters.Style = this;
        }

        public void SetStroke(StrokeParameters parms)
        {
            parms.PenWidth = StrokeParameters.PenWidth;
            StrokeParameters = parms;
            StrokeParameters.Style = this;
        }

        public void SetTint(TintParameters parms)
        {
            if (TintParameters != null) TintParameters.Dispose();
            TintParameters = parms;
        }

        public void SetNoFill()
        {
            FillParameters.Disabled = true;
        }

        public void SetNoStroke()
        {
            StrokeParameters.Disabled = true;
        }

        public void SetNoTint()
        {
            TintParameters.Disabled = true;
        }

        public void SetStrokeWeight(float weight)
        {
            StrokeParameters.PenWidth = weight;
        }

        public void SetFont(string name, float size) => SetFont(new FontParameters(name, size));
        public void SetFont(string name, float size, bool bold, bool italic) => SetFont(new FontParameters(name, size, bold, italic));
        public void SetFont(FontParameters parms)
        {
            float size = parms.Size ?? Font.Size;
            bool bold = parms.Bold ?? Font.Bold;
            bool italic = parms.Italic ?? Font.Italic;
            FontStyle style = FontStyle.Regular;

            style |= bold ? FontStyle.Bold : FontStyle.Regular;
            style |= italic ? FontStyle.Italic : FontStyle.Regular;

            Font = new Font(parms.Name, size, style);
        }

        public void SetColorMode(int mode) => SetColorMode((ColorMode)mode);
        public void SetColorMode(ColorMode mode) => SetColorMode(new ColorModeParameters(mode));
        public void SetColorMode(ColorModeParameters parms)
        {
            ColorMode = parms.Mode;

            switch (parms.Mode)
            {
                //case ColorMode.Hsb:
                //    ColorRange.SetDefaults(360f, 1f, 1f, 255f);
                //    break;

                default:
                    ColorRange.SetDefaults(255f, 255f, 255f, 255f);
                    break;
            }

            ColorRange.Max1 = parms.Max1 ?? ColorRange.Max1;
            ColorRange.Max2 = parms.Max2 ?? ColorRange.Max2;
            ColorRange.Max3 = parms.Max3 ?? ColorRange.Max3;
            ColorRange.MaxAlpha = parms.MaxAlpha ?? ColorRange.MaxAlpha;
        }

        public Color GetColor(float r, float g, float b, float alpha)
        {
            if (r > ColorRange.Max1) r = ColorRange.Max1;
            if (g > ColorRange.Max2) g = ColorRange.Max2;
            if (b > ColorRange.Max3) b = ColorRange.Max3;
            if (alpha > ColorRange.MaxAlpha) alpha = ColorRange.MaxAlpha;

            if (r < 0) r = 0;
            if (g < 0) g = 0;
            if (b < 0) b = 0;
            if (alpha < 0) alpha = 0;

            switch (ColorMode)
            {
                case ColorMode.Hsb:
                    if (!ColorRange.IsDefault)
                    {
                        r = Functions.Map(r, 0, ColorRange.Max1, 0, 255);
                        g = Functions.Map(g, 0, ColorRange.Max2, 0, 255);
                        b = Functions.Map(b, 0, ColorRange.Max3, 0, 255);
                        alpha = Functions.Map(alpha, 0, ColorRange.MaxAlpha, 0, 255);
                    }
                    return Functions.ColorFromAhsb((int)alpha, r / 255f, g / 255f, b / 255f);

                default:
                    if (!ColorRange.IsDefault)
                    {
                        r = Functions.Map(r, 0, ColorRange.Max1, 0, 255);
                        g = Functions.Map(g, 0, ColorRange.Max2, 0, 255);
                        b = Functions.Map(b, 0, ColorRange.Max3, 0, 255);
                        alpha = Functions.Map(alpha, 0, ColorRange.MaxAlpha, 0, 255);
                    }
                    return new Color(r, g, b, alpha);
            }
        }

        public Color GetColor(float gray)
        {
            return GetColor(gray, 255);
        }

        public Color GetColor(float gray, float alpha)
        {
            return new Color(gray, gray, gray, alpha);
        }

        public Color GetColor(float r, float g, float b)
        {
            return GetColor(r, g, b, ColorRange.MaxAlpha);
        }

        internal Rectangle GetAdjustedRectangle(RectangleParameters parms) => GetAdjustedRectangle((int)RectangleMode, parms.X, parms.Y, parms.Width, parms.Height);
        internal Rectangle GetAdjustedRectangle(ArcParameters parms) => GetAdjustedRectangle((int)EllipseMode, parms.X, parms.Y, parms.Width, parms.Height);
        internal Rectangle GetAdjustedRectangle(EllipseParameters parms) => GetAdjustedRectangle((int)EllipseMode, parms.X, parms.Y, parms.Width, parms.Height);
        internal Rectangle GetAdjustedRectangle(ImageParameters parms) => GetAdjustedRectangle((int)ImageMode, parms.X ?? 0, parms.Y ?? 0, parms.Width ?? parms.Image.Width, parms.Height ?? parms.Image.Height);
        internal Rectangle GetAdjustedRectangle(int mode, float x, float y, float width, float height)
        {
            switch (mode)
            {
                case Constants.CENTER:
                    x -= width / 2;
                    y -= height / 2;
                    break;

                case Constants.RADIUS:
                    width *= 2;
                    height *= 2;
                    x -= width / 2;
                    y -= height / 2;
                    break;

                case Constants.CORNERS:
                    width -= x;
                    height -= y;
                    break;
            }

            return new Rectangle(x, y, width, height);
        }
    }
}
