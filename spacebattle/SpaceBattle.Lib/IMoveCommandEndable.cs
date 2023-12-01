namespace SpaceBattle.Lib;
public interface IMoveCommandEndable
{
    public BridgeCommand Move { get; }
    public IUObject Object { get; }
    public IEnumerable<string> Properties{ get; } // Queue<ICommand> ?
}