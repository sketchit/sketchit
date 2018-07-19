using System.Drawing;

namespace SketchIt.Api.Interfaces
{
    public interface IImage
    {
        int Width { get; }
        int Height { get; }
        Bitmap Bitmap { get; }
    }
}
