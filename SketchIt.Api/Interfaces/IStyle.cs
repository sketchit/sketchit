using SketchIt.Api.Static;
using System.Drawing;

namespace SketchIt.Api.Interfaces
{
    public interface IStyle
    {
        ColorMode ColorMode { get; set; }
        ColorRange ColorRange { get; }
        EllipseMode EllipseMode { get; set; }
        FillParameters FillParameters { get; }
        Font Font { get; }
        RectangleMode RectangleMode { get; set; }
        ImageMode ImageMode { get; set; }
        StrokeParameters StrokeParameters { get; }

        Color GetColor(float gray);
        Color GetColor(float gray, float alpha);
        Color GetColor(float r, float g, float b);
        Color GetColor(float r, float g, float b, float alpha);

        void SetColorMode(ColorMode mode);
        void SetColorMode(ColorModeParameters parms);

        void SetFont(FontParameters parms);
        void SetFont(string name, float size);
        void SetFont(string name, float size, bool bold, bool italic);
        void SetTextAlignment(HorizontalAlignment horizontal, VerticalAlignment vertical);

        void SetStrokeWeight(float weight);
        void SetFill(FillParameters parms);
        void SetNoFill();
        void SetStroke(StrokeParameters parms);
        void SetNoStroke();
        void SetTint(TintParameters parms);
        void SetNoTint();

        void SetEllipseMode(EllipseMode mode);
        void SetRectangleMode(RectangleMode mode);
        void SetImageMode(ImageMode mode);
    }
}