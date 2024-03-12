namespace SpaceBattle.Lib;

using System.Collections.Concurrent;
using Hwdtech;

public class StopServerCommand : ICommand
{
    public void Execute()
    {
        // словарь очередей потоков
        var thread_queues = IoC.Resolve<ConcurrentDictionary<int, ISender>>("Server.Thread.SenderDictionary");

        thread_queues.ToList().ForEach(sender =>
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