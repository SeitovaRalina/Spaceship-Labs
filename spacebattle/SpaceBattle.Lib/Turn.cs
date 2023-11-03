namespace SpaceBattle.Lib;

public interface ITurnable
{
    VectorTurn Angle {get ; set; }
    VectorTurn DeltaAngle {get; }
}

public class TurnCommand : ICommand
{
    private readonly ITurnable turnable;
    public TurnCommand(ITurnable turnable)
    {
        this.turnable = turnable;
    }
    public void Execute()
    {
        turnable.Angle = turnable.Angle + turnable.DeltaAngle;
    }
}