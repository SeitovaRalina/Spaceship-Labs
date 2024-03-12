namespace SpaceBattle.Lib;

public class Program
{
    private static void Main(string[] args)
    {
        var size = int.Parse(args[0]);
        new RunApplicationCommand(size).Execute();
    }
}