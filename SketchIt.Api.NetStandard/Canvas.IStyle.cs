using SketchIt.Api.Interfaces;
using SketchIt.Api.Static;
using System.Drawing;

namespace SketchIt.Api
{
    public partial class Canvas : IStyle
    {
        public ColorMode ColorMode { get => ((IStyle)Style).ColorMode; set => ((IStyle)Style).ColorMode = value; }

        public ColorRange ColorRange => ((IStyle)Style).ColorRange;

        public EllipseMode EllipseMode { get => ((IStyle)Style).EllipseMode; set => ((IStyle)Style).EllipseMode = value; }

        public FillParameters FillParameters => ((IStyle)Style).FillParameters;

        public Font Font => ((IStyle)Style).Font;

        public RectangleMode RectangleMode { get => ((IStyle)Style).RectangleMode; set => ((IStyle)Style).RectangleMode = value; }

        public ImageMode ImageMode { get => ((IStyle)Style).ImageMode; set => ((IStyle)Style).ImageMode = value; }

        public StrokeParameters StrokeParameters => ((IStyle)Style).StrokeParameters;

        public Color GetColor(float gray)
        {
            return ((IStyle)Style).GetColor(gray);
        }

        public Color GetColor(float gray, float alpha)
        {
            return ((IStyle)Style).GetColor(gray, alpha);
        }

        public Color GetColor(float r, float g, float b)
        {
            return ((IStyle)Style).GetColor(r, g, b);
        }

        public Color GetColor(float r, float g, float b, float alpha)
        {
            return ((IStyle)Style).GetColor(r, g, b, alpha);
        }

        public void SetBackground(BackgroundParameters parms)
        {
            ((IStyle)Style).SetBackground(parms);
        }

        public void SetColorMode(ColorMode mode)
        {
            ((IStyle)Style).SetColorMode(mode);
        }

        public void SetColorMode(ColorModeParameters parms)
        {
            ((IStyle)Style).SetColorMode(parms);
        }

        public void SetEllipseMode(EllipseMode mode)
        {
            ((IStyle)Style).SetEllipseMode(mode);
        }

        public void SetImageMode(ImageMode mode)
        {
            ((IStyle)Style).SetImageMode(mode);
        }

        public void SetFill(FillParameters parms)
        {
            ((IStyle)Style).SetFill(parms);
        }

        public void SetFont(FontParameters parms)
        {
            ((IStyle)Style).SetFont(parms);
        }

        public void SetFont(string name, float size)
        {
            ((IStyle)Style).SetFont(name, size);
        }

        public void SetFont(string name, float size, bool bold, bool italic)
        {
            ((IStyle)Style).SetFont(name, size, bold, italic);
        }

        public void SetNoFill()
        {
            ((IStyle)Style).SetNoFill();
        }

        public void SetNoStroke()
        {
            ((IStyle)Style).SetNoStroke();
        }

        public void SetRectangleMode(RectangleMode mode)
        {
            ((IStyle)Style).SetRectangleMode(mode);
        }

        public void SetStroke(StrokeParameters parms)
        {
            ((IStyle)Style).SetStroke(parms);
        }

        public void SetStrokeWeight(float weight)
        {
            ((IStyle)Style).SetStrokeWeight(weight);
        }

        public void SetTextAlignment(HorizontalAlignment horizontal, VerticalAlignment vertical)
        {
            ((IStyle)Style).SetTextAlignment(horizontal, vertical);
        }

        public void SetTint(TintParameters parms)
        {
            ((IStyle)Style).SetTint(parms);
        }

        public void SetNoTint()
        {
            ((IStyle)Style).SetNoTint();
        }
    }
}
