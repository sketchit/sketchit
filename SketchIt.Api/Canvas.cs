using SketchIt.Api.Interfaces;
using SketchIt.Api.Renderers;

namespace SketchIt.Api
{
    public partial class Canvas : Image
    {
        public Canvas(Sketch sketch, int width, int height)
            : base(width, height)
        {
            Sketch = sketch;
            SetRenderer(RendererType.GdiPlus);
        }

        public Sketch Sketch { get; private set; }
        public Point Location { get; private set; } = new Point();
        public Style Style { get; private set; } = new Style();
        public RendererType RendererType { get; internal set; }
        public RendererBase Renderer { get; private set; }

        public void SetRenderer(RendererType rendererType)
        {
            RendererType = rendererType;

            switch (rendererType)
            {
                case RendererType.GdiPlus:
                    Renderer = new GdiPlusRenderer(this);
                    break;

                case RendererType.OpenGL:
                    Renderer = new SharpGLRenderer(this);
                    break;
            }
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
