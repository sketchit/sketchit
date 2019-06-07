using SketchIt.Api.Interfaces;

namespace SketchIt.Sound
{
    public class Library : ILibrary
    {
        public Library()
        {
        }

        public bool Embeddable
        {
            get => true;
        }

        public string Name
        {
            get => "Sound library using the NAudio library";
        }

        public string[] EmbeddableDependancies
        {
            get
            {
                return new string[] { "NAudio.dll" };
            }
        }

        public string[] AdditionalDependancies
        {
            get
            {
                return new string[]
                {
                };
            }
        }
    }
}
