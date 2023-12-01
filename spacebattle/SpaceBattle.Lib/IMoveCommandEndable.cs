namespace SpaceBattle.Lib;
public interface IMoveCommandEndable
{
    public BridgeCommand Move { get; }
    public IUObject Target { get; }
    public IEnumerable<string> Properties{ get; } // Queue<ICommand> ?
}