namespace SketchIt.Api
{
    public struct Point
    {
        public static readonly Point Empty;

        public Point(float x, float y)
            : this(x, y, 0)
        {
        }

        public Point(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public bool IsEmpty
        {
            get
            {
                return !(X != 0 || Y != 0 || Z != 0);
            }
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

        public float Z
        {
            get;
            set;
        }

        public System.Drawing.PointF ToSystemPointF()
        {
            return new System.Drawing.PointF(X, Y);
        }
    }
}
