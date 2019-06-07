namespace SketchIt.Api.Static
{
    public static class Constants
    {
        public const int DEFAULT = 0;

        public const float PI = 3.14159265358979323846f;
        public const float TWO_PI = PI * 2f;
        public const float HALF_PI = PI / 2f;
        public const float QUARTER_PI = PI / 4f;
        public const float TAU = TWO_PI;
        public const float E = 2.7182818284590452354f;

        public const int RGB = 0;
        public const int HSB = 1;

        public const int LEFT = 1;
        public const int RIGHT = 2;
        public const int MIDDLE = 4;

        public const int TOP = 1;
        public const int BOTTOM = 2;

        public const int CENTER = 0;
        public const int CORNER = 1;
        public const int RADIUS = 2;
        public const int CORNERS = 3;

        public const float DEG_TO_RAD = PI / 180.0f;
        public const float RAD_TO_DEG = 180.0f / PI;

        public const int POLYGON = 0;
        public const int POINTS = 1;
        public const int LINES = 2;
        public const int TRIANGLES = 3;
        public const int TRIANGLE_FAN = 4;
        public const int TRIANGLE_STRIP = 5;
        public const int QUADS = 6;
        public const int QUAD_STRIP = 7;

        public const int OPEN = 0;
        public const int CLOSE = 1;

        //public const int GDIPLUS = 0;
        //public const int OPENGL = 1;

        public const int CHORD = 1;
        public const int PIE = 2;

        public const int DEGREES = 0;
        public const int RADIANS = 1;
    }

    public enum AngleMode
    {
        Degrees = Constants.DEGREES,
        Radians = Constants.RADIANS
    }

    public enum ArcMode
    {
        Open = Constants.OPEN,
        Chord = Constants.CHORD,
        Pie = Constants.PIE
    }

    public enum EllipseMode
    {
        Center = Constants.CENTER,
        Corner = Constants.CORNER,
        Radius = Constants.RADIUS,
        Corners = Constants.CORNERS
    }

    public enum RectangleMode
    {
        Center = Constants.CENTER,
        Corner = Constants.CORNER,
        Radius = Constants.RADIUS,
        Corners = Constants.CORNERS
    }

    public enum ImageMode
    {
        Center = Constants.CENTER,
        Corner = Constants.CORNER,
        Corners = Constants.CORNERS
    }

    public enum ColorMode
    {
        Rgb = Constants.RGB,
        Hsb = Constants.HSB
    }

    public enum ShapeKind
    {
        Polygon = Constants.POLYGON,
        Points = Constants.POINTS,
        Lines = Constants.LINES,
        Triangles = Constants.TRIANGLES,
        TriangleFan = Constants.TRIANGLE_FAN,
        TriangleStrip = Constants.TRIANGLE_STRIP,
        Quads = Constants.QUADS,
        QuadStrip = Constants.QUAD_STRIP
    }

    public enum EndShapeMode
    {
        Open = Constants.OPEN,
        Close = Constants.CLOSE
    }

    public enum HorizontalAlignment
    {
        Left = Constants.LEFT,
        Center = Constants.CENTER,
        Right = Constants.RIGHT
    }

    public enum VerticalAlignment
    {
        Top = Constants.TOP,
        Center = Constants.CENTER,
        Bottom = Constants.BOTTOM
    }
}
