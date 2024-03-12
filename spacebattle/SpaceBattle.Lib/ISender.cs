namespace SpaceBattle.Lib;
using System.Collections.Concurrent;
using Hwdtech;

public interface ISender
{
    public void Send(ICommand message);
}

public class SenderAdapter : ISender
{
    private readonly BlockingCollection<ICommand> _queue;
    public SenderAdapter(BlockingCollection<ICommand> queue)
    {
        _queue = queue;
    }
    public void Send(ICommand cmd)
    {
        _queue.Add(cmd);
    }
}

public class SendCommand : ICommand
{
    private readonly int _id;
    private readonly ICommand _message;

    public SendCommand(int id, ICommand message)
    {
        _id = id;
        _message = message;
    }

    public void Execute()
    {
        var threadQueues = IoC.Resolve<ConcurrentDictionary<int, ISender>>("Server.Thread.SenderDictionary");
        var sender = threadQueues[_id];

        sender.Send(_message);
    }
}