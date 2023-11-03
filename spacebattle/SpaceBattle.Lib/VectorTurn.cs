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
        if (obj == null || obj is not VectorTurn)
        {
            return false;
        }
        else
        {
            return Angle == ((VectorTurn)obj).Angle;
        }
    }
    public override int GetHashCode()
    {
        return Angle.GetHashCode();
    }
}