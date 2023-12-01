using Hwdtech;

namespace SpaceBattle.Lib;

public class EndMoveCommand : ICommand
{
    private readonly IEndable _endable;
    public EndMoveCommand(IEndable endable) => _endable = endable;
    public void Execute()
    {
        IoC.Resolve<string>("Game.UObject.DeleteProperties", _endable.Target, _endable.Properties);
        var commandToEnd = _endable.Move;
        var emptyCommand = IoC.Resolve<ICommand>("Game.Command.CreateEmpty");
        IoC.Resolve<IInjectable>("Game.Command.Inject", commandToEnd, emptyCommand);
    }
}