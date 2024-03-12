namespace SpaceBattle.Lib;
using Hwdtech;

public class RunApplicationCommand : ICommand
{
    private readonly int _size;
    public RunApplicationCommand(int size)
    {
        _size = size;
    }
    public void Execute()
    {
        Console.WriteLine("Hello, Ivan Vladimirovich!");
        Console.WriteLine($"The server is starting with the number of threads {_size}");
        IoC.Resolve<ICommand>("Server.Start", _size).Execute();

        Console.WriteLine("Press any key to stop the server softly ...");
        Console.Read();
        IoC.Resolve<ICommand>("Server.Stop").Execute();

        Console.WriteLine("The server has successfully stopped its work. Press any key to exit...");
        Console.Read();
    }
}
