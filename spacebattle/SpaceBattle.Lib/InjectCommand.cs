namespace SpaceBattle.Lib;

public class InjectCommand: ICommand, IInjectable<ICommand>
{
    private ICommand _command;
    public InjectCommand(ICommand command) => _command = command;
    public void Inject(ICommand command) => _command = command;
    public void Execute() => _command.Execute();
    public ICommand GetCommand() => _command;
}