namespace SketchIt.Api
{
    public struct Vertex
    {
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

        public float U
        {
            get;
            set;
        }

        public float V
        {
            get;
            set;
        }

        public Vertex(float x, float y)
            : this(x, y, 0, 0, 0)
        {
        }

        public Vertex(float x, float y, float z)
            : this(x, y, z, 0, 0)
        {
        }

        public Vertex(float x, float y, float z, float u, float v)
        {
            X = x;
            Y = y;
            Z = z;
            U = u;
            V = v;
        }
    }
}
