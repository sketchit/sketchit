using SketchIt.Api.Interfaces;
using System.Drawing;

namespace SketchIt.Video
{
    public partial class Capture : IImage
    {
        public int Width
        {
            get;
            private set;
        }

        public int Height
        {
            get;
            private set;
        }

        public Bitmap Bitmap
        {
            get;
            private set;
        }
    }
}
