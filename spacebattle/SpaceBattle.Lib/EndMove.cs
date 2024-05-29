using Hwdtech;

namespace SpaceBattle.Lib;

public interface IMoveCommandEndable
{
    public BridgeCommand Move { get; }
    public IUObject Object { get; }
    public IEnumerable<string> Properties { get; }
}

public class EndMoveCommand : ICommand
{
    private readonly IMoveCommandEndable _endable;
    public EndMoveCommand(IMoveCommandEndable endable)
    {
        _endable = endable;
    }
    public void Execute()
    {
        IoC.Resolve<string>("Game.Command.DeleteUObjectProperties", _endable.Object, _endable.Properties);
        IoC.Resolve<IInjectableCommand>("Game.Command.Inject",
            _endable.Move,
            IoC.Resolve<ICommand>("Game.Command.CreateEmpty"));
    }
}
