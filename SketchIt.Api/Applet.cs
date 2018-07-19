using SketchIt.Api.Interfaces;

namespace SketchIt.Api
{
    public abstract class Applet : IApplet
    {
        private Sketch _sketch;

        public void SetSketch(Sketch sketch)
        {
            _sketch = sketch;
        }

        public Sketch GetSketch()
        {
            return _sketch;
        }
    }
}
