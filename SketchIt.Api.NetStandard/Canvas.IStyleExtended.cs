using SketchIt.Api.Interfaces;
using SketchIt.Api.Static;

namespace SketchIt.Api
{
    public partial class Canvas : IStyleExtended
    {
        public void SetTextAlignment(int horizontal, int vertical) { SetTextAlignment((HorizontalAlignment)horizontal, (VerticalAlignment)vertical); }

        public void SetEllipseMode(int mode) { SetEllipseMode((EllipseMode)mode); }

        public void SetRectangleMode(int mode) { SetRectangleMode((RectangleMode)mode); }

        public void SetColorMode(int mode) { SetColorMode(new ColorModeParameters((ColorMode)mode)); }
        public void SetColorMode(int mode, float max) { SetColorMode(new ColorModeParameters((ColorMode)mode, max)); }
        public void SetColorMode(int mode, float max1, float max2, float max3) { SetColorMode(new ColorModeParameters((ColorMode)mode, max1, max2, max3)); }

        public void SetStroke(float gray) { SetStroke(new StrokeParameters(GetColor(gray))); }
        public void SetStroke(float gray, float alpha) { SetStroke(new StrokeParameters(GetColor(gray, alpha))); }
        public void SetStroke(float r, float g, float b) { SetStroke(new StrokeParameters(GetColor(r, g, b))); }
        public void SetStroke(float r, float g, float b, float alpha) { SetStroke(new StrokeParameters(GetColor(r, g, b, alpha))); }
        public void SetStroke(Color color, float alpha) { SetStroke(new StrokeParameters(new Color(color, alpha))); }
        public void SetStroke(Color color) { SetStroke(new StrokeParameters(color)); }

        public void SetFill(float gray) { SetFill(new FillParameters(Style.GetColor(gray))); }
        public void SetFill(float gray, float alpha) { SetFill(new FillParameters(Style.GetColor(gray, alpha))); }
        public void SetFill(float r, float g, float b) { SetFill(new FillParameters(Style.GetColor(r, g, b))); }
        public void SetFill(float r, float g, float b, float alpha) { SetFill(new FillParameters(Style.GetColor(r, g, b, alpha))); }
        public void SetFill(Color color, float alpha) { SetFill(new FillParameters(new Color(color, alpha))); }
        public void SetFill(Color color) { SetFill(new FillParameters(color)); }
        public void SetFill(Image image) { SetFill(new FillParameters(image)); }

        public void SetTint(float gray) { SetTint(new TintParameters(Style.GetColor(gray))); }
        public void SetTint(float gray, float alpha) { SetTint(new TintParameters(Style.GetColor(gray, alpha))); }
        public void SetTint(float r, float g, float b) { SetTint(new TintParameters(Style.GetColor(r, g, b))); }
        public void SetTint(float r, float g, float b, float alpha) { SetTint(new TintParameters(Style.GetColor(r, g, b, alpha))); }
        public void SetTint(Color color, float alpha) { SetTint(new TintParameters(new Color(color, alpha))); }
        public void SetTint(Color color) { SetTint(new TintParameters(color)); }
    }
}
