
public interface ICommandHandler<TCommand> where TCommand : ICommand
{
    public bool Handle(TCommand command);
}