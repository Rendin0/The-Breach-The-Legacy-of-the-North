
public interface ICommandProcessor
{
    public void RegisterHandler<TCommand>(ICommandHandler<TCommand> commandHandler) where TCommand : ICommand;
    public bool Process<TCommnd>(TCommnd command) where TCommnd : ICommand;
}
