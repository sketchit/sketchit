using SketchIt.Api.Interfaces;
using SketchIt.Api.Renderers;
using System;

namespace SketchIt.Api
{
    public partial class Canvas : Image
    {
        public Canvas(Sketch sketch, int width, int height)
            : base(width, height)
        {
            Sketch = sketch;
            SetRenderer(Sketch.DefaultRendererType);
        }

        public Sketch Sketch { get; private set; }
        public Point Location { get; private set; } = new Point();
        public Style Style { get; set; } = new Style();
        public RendererBase Renderer { get; private set; }

        public void SetRenderer(Type rendererType)
        {
            Renderer = Activator.CreateInstance(rendererType, new object[] { this }) as RendererBase;
        }

        public void SetSize(float width, float height)
        {
            if (Width == width && Height == height) return;

            Resize((int)width, (int)height, null);
            Renderer.SetSize(width, height);

            if (Sketch != null && Sketch.OutputLayer.Equals(this))
            {
                Sketch.SetSize(width, height);
            }
        }

        public void SetLocation(float x, float y)
        {
            Location = new Point(x, y);
        }
    }
}
