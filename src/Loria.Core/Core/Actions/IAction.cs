namespace LoriaNET
{
    public interface IAction : IHandleCommand
    {
        string[] SupportedIntents { get; }
        string[] SupportedEntities { get; }
        string[] Samples { get; }
    }
}
