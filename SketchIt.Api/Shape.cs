using SketchIt.Api.Interfaces;
using SketchIt.Api.Static;
using System.Collections.Generic;

namespace SketchIt.Api
{
    public class Shape
    {
        public ShapeKind Kind { get; private set; }
        public EndShapeMode EndMode { get; private set; }
        public IImage Texture { get; private set; }
        public Vertex[] Vertices { get { return _vertices.ToArray(); } }

        private List<Vertex> _vertices = new List<Vertex>();

        public Shape(int kind)
            : this((ShapeKind)kind)
        {
        }

        public Shape()
            : this(ShapeKind.Polygon)
        {
        }

        public Shape(ShapeKind kind)
        {
            Kind = kind;
        }

        public void SetTexture(IImage image)
        {
            Texture = image;
        }

        public void Vertex(float x, float y) => Vertex(x, y, 0, 0, 0);
        public void Vertex(float x, float y, float z) => Vertex(x, y, z, 0, 0);
        public void Vertex(float x, float y, float z, float u, float v)
        {
            _vertices.Add(new Vertex(x, y, z, u, v));
        }

        public void VertexAt(int index, float x, float y)
        {
            _vertices.Insert(index, new Vertex(x, y));
        }

        public void End() => End(EndShapeMode.Open);
        public void End(int mode) => End((EndShapeMode)mode);
        public void End(EndShapeMode mode)
        {
            EndMode = mode;
        }

        public void Move(float x, float y)
        {
            for (int i = 0; i < _vertices.Count; i++)
            {
                Vertex v = _vertices[i];
                _vertices[i] = new Vertex(v.X + x, v.Y + y);
            }
        }

        public void RemoveAt(int index)
        {
            _vertices.RemoveAt(index);
        }
    }
}
