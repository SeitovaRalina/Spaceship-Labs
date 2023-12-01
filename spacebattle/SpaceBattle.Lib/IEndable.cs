namespace SpaceBattle.Lib;
public interface IEndable
{
    public InjectCommand Command { get; }
    public IUObject Target { get; }
    public IEnumerable<string> Properties{ get; }
}