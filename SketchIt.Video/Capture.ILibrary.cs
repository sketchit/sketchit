using SketchIt.Api.Interfaces;

namespace SketchIt.Video
{
    public partial class Capture : ILibrary
    {
        public Capture()
        {
        }

        public bool Embeddable
        {
            get => true;
        }

        public string Name
        {
            get => "Video Capture library using the Emgu.CV library";
        }

        public string[] EmbeddableDependancies
        {
            get
            {
                return new string[] { };
            }
        }

        public string[] AdditionalDependancies
        {
            get
            {
                return new string[]
                {
                    "Emgu.CV.UI.dll",
                    "Emgu.CV.World.dll",
                    "Zedgraph.dll",
                    "x86\\concrt140.dll",
                    "x86\\cvextern.dll",
                    "x86\\msvcp140.dll",
                    "x86\\opencv_ffmpeg341.dll",
                    "x86\\vcruntime140.dll",
                    "x64\\concrt140.dll",
                    "x64\\cvextern.dll",
                    "x64\\msvcp140.dll",
                    "x64\\opencv_ffmpeg341_64.dll",
                    "x64\\vcruntime140.dll"
                };
            }
        }
    }
}
