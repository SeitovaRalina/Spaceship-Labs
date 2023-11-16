namespace SpaceBattle.Lib;

public interface IMovable
{
    Vector Position { get; set; }
    Vector Velocity { get; }
}

public class MoveCommand : ICommand
{
    private IMovable movable;
    
    public MoveCommand(IMovable movable)
    {
        this.movable = movable;
    }

    public void Execute()
    {
        movable.Position = movable.Position + movable.Velocity;
    }
}
