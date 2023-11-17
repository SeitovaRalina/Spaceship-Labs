namespace SpaceBattle.Lib;

public class VectorAngle
{
    private int angle;
    private int parts;
    public virtual int Angle
    {
        get => angle;
        set => angle = value % parts;
    }
    public VectorAngle(int angle, int parts = 360)
    {
        this.parts = parts;
        Angle = angle;
        Minimize();
    }
    private static int GCD(int a, int b)
    {
        return b == 0 ? a : GCD(b, a % b);
    }
    private void Minimize()
    {
        var gcd = GCD(angle, parts);
        angle /= gcd;
        parts /= gcd;
    }

    public static VectorAngle operator +(VectorAngle x, VectorAngle y)
    {
        x.angle = x.angle * y.parts + y.angle * x.parts;
        x.parts *= y.parts;
        x.Minimize();
        return x;
    }
    public override bool Equals(object? obj)
    {
        return obj is VectorAngle turn && Angle == turn.Angle;
    }
    public override int GetHashCode()
    {
        return Angle.GetHashCode();
    }
}
