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
        Console.WriteLine("Здравствуйте, Иван Владимирович!");
        Console.WriteLine("Нажмите на любую клавишу для запуска сервера ...");
        Console.Read();
        IoC.Resolve<ICommand>("Server.Start", _size).Execute();

        Console.WriteLine("Нажмите на любую клавишу для остановки сервера ...");
        Console.Read();
        IoC.Resolve<ICommand>("Server.Stop").Execute();

        Console.WriteLine("Cервер успешно завершил свою работу. Нажмите любую кнопку для выхода... ");
        Console.Read();
    }
}