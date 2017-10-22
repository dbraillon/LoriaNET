namespace Loria.Core
{
    public interface IPropagator
    {
        void Propagate(Command command);
        void PropagateAction(Command command);
        void PropagateCallback(Command command);
    }
}
