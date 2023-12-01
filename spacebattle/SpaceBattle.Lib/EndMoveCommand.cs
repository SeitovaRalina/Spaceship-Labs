using Hwdtech;

namespace SpaceBattle.Lib;

public class EndMoveCommand : ICommand
{
    private readonly IMoveCommandEndable _endable;
    public EndMoveCommand(IMoveCommandEndable endable) => _endable = endable;
    public void Execute()
    {
        IoC.Resolve<string>("Game.UObject.DeleteProperty", _endable.Object, _endable.Properties);
        var commandToEnd = _endable.Move;
        var emptyCommand = IoC.Resolve<ICommand>("Game.Command.CreateEmpty");
        IoC.Resolve<IInjectableCommand>("Game.Command.Inject", commandToEnd, emptyCommand);
    }
}