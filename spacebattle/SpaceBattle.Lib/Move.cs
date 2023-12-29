namespace SpaceBattle.Lib;

public interface IMovable
{
    public Vector Position { get; set; }
    public Vector Velocity { get; }
}

public class MoveCommand : ICommand
{
#pragma warning disable IDE0036
    private readonly IMovable movable;
#pragma warning disable IDE0036
    public MoveCommand(IMovable movable)
    {
        this.movable = movable;
    }

    public void Execute()
    {
        movable.Position += movable.Velocity;
    }
}
