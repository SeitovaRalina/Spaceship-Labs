namespace SpaceBattle.Lib;

using System.Collections.Concurrent;
using Hwdtech;

public class StopServerCommand : ICommand
{
    public void Execute()
    {
        // словарь очередей потоков
        var threadQueues = IoC.Resolve<ConcurrentDictionary<int, BlockingCollection<ICommand>>>("Server.Thread.SenderDictionary");

        threadQueues.ToList().ForEach(sender =>
            IoC.Resolve<ICommand>(
                "Server.Thread.SendCommand",
                //id потока
                sender.Key,
                // команда SoftStop, которую нужно добавить в очередь потока, чтобы она изменила его поведение
                IoC.Resolve<ICommand>("Server.Thread.SoftStop", sender.Key)
            ).Execute()
        );
    }
}
