namespace SketchIt.Api.Interfaces
{
    public interface IStyleExtended
    {
        void SetTextAlignment(int horizontal, int vertical);

        void SetEllipseMode(int mode);

        void SetRectangleMode(int mode);

        void SetColorMode(int mode);
        void SetColorMode(int mode, float max);
        void SetColorMode(int mode, float max1, float max2, float max3);

        void SetStroke(float gray);
        void SetStroke(float gray, float alpha);
        void SetStroke(float r, float g, float b);
        void SetStroke(float r, float g, float b, float alpha);
        void SetStroke(Color color, float alpha);
        void SetStroke(Color color);

        void SetFill(float gray);
        void SetFill(float gray, float alpha);
        void SetFill(float r, float g, float b);
        void SetFill(float r, float g, float b, float alpha);
        void SetFill(Color color, float alpha);
        void SetFill(Color color);
        void SetFill(Image image);

        void SetTint(float gray);
        void SetTint(float gray, float alpha);
        void SetTint(float r, float g, float b);
        void SetTint(float r, float g, float b, float alpha);
        void SetTint(Color color, float alpha);
        void SetTint(Color color);
    }
}
