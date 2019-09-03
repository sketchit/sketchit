using SketchIt.Api.Interfaces;
using System;

namespace SketchIt.Data
{
    public class Library : ILibrary
    {
        public string Name => "A library for using the OleDb data provider.";

        public string[] EmbeddableDependancies => new string[] { };

        public string[] AdditionalDependancies => new string[] { };

        public bool Embeddable => true;
    }
}
