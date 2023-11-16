namespace SpaceBattle.Lib;

public class VectorTurn
{
    private int angle;
    public virtual int Angle
    {
        get => angle;
        set => angle = value % 360;
    }
    public VectorTurn(int angle)
    {
        Angle = angle;
    }
    public static VectorTurn operator +(VectorTurn x, VectorTurn y)
    {
        x.Angle += y.Angle;
        return x;
    }
    public override bool Equals(object? obj)
    {
        return obj is VectorTurn turn && Angle == turn.Angle;
    }
    public override int GetHashCode()
    {
        return Angle.GetHashCode();
    }
}
