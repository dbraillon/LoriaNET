namespace Loria.Core
{
    public interface IAction : IHasCommand
    {
        string[] SupportedIntents { get; }
        string[] SupportedEntities { get; }
        string[] Samples { get; }
    }
}
