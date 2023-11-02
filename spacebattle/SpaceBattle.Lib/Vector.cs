namespace SpaceBattle.Lib;

public class Vector
{
    private readonly int[] coordinates;
    private int this[int i]
    {
        get => coordinates[i];
        set => coordinates[i] = value;
    }
    public Vector(int[] coordinates)
    {
        this.coordinates = coordinates;
    }
    public Vector(int length)
    {
        coordinates = new int[length];
    }
    private int Length()
    {
        return coordinates.Length;
    }

    public static Vector operator +(Vector x, Vector y)
    {
        var temp = new Vector(x.Length());
        for (var i = 0; i < temp.Length(); i++)
        {
            temp[i] = x[i] + y[i];
        }

        return temp;
    }
    public override bool Equals(object? obj)
    {
        if (obj == null || obj is not Vector)
            {
                return false;
            }
        else
            {
                return coordinates.SequenceEqual( ( (Vector)obj ).coordinates);
            }
    }
    public override int GetHashCode() => coordinates.Length + coordinates.Sum().GetHashCode();
}