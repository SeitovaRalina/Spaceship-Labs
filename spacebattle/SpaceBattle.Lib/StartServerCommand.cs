namespace SpaceBattle.Lib;
using Hwdtech;

public class StartServerCommand : ICommand
{
    private readonly int _size;
    public StartServerCommand(int size)
    {
        _size = size;
    }
    public void Execute()
    {
        Enumerable.Range(0, _size).ToList().ForEach(id =>
            // инициализирует по id поток и очередь для него, добавляясь в соответствующие словари
            IoC.Resolve<ICommand>("Server.Thread.Start", id).Execute()
        );
    }
}
