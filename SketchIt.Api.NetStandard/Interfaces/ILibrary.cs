namespace SketchIt.Api.Interfaces
{
    public interface ILibrary
    {
        string Name { get; }
        string[] EmbeddableDependancies { get; }
        string[] AdditionalDependancies { get; }
        bool Embeddable { get; }
    }
}
