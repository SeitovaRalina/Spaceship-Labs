namespace SpaceBattle.Lib;

public class Vectors
{
    private int[] coordinates;
    private readonly int coord_cont;
    public Vectors(params int[] coordinates)
    {
        this.coordinates = coordinates;
        coord_cont = coordinates.Length;
    }
    public static Vectors operator +(Vectors a, Vectors b)
    {
        Vectors c = new(new int[a.coord_cont]);
        c.coordinates = (a.coordinates.Select((x, index) => x + b.coordinates[index]).ToArray());
        return c;
    }
    public override bool Equals(object obj)
    {
        return coordinates.SequenceEqual(((Vectors)obj).coordinates);
    }
    public override int GetHashCode()
    {
        return coordinates.GetHashCode();
    }
}
