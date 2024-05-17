using System.Diagnostics.CodeAnalysis;

namespace SpaceBattle.Lib;

[ExcludeFromCodeCoverage]
public class Vector
{
    private int[] coordinates;
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
        var temp = new Vector(x.Length())
        {
            coordinates = x.coordinates.Select((value, index) => value + y[index]).ToArray()
        };
        return temp;
    }
    public override bool Equals(object? obj)
    {
        return obj != null && coordinates.SequenceEqual(((Vector)obj).coordinates);
    }
    public override int GetHashCode()
    {
        return coordinates.GetHashCode();
    }
}
