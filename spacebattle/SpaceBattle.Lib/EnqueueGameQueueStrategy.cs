using Hwdtech;

namespace SpaceBattle.Lib;

public class EnqueueGameQueueStrategy : IStrategy
{
    public object Init(params object[] args)
    {
        var gameID = (string)args[0];
        var command = (ICommand)args[1];

        var gameQueue = IoC.Resolve<Queue<ICommand>>("Game.Queue.GetByGameID", gameID);

        return new ActionCommand(() => { gameQueue.Enqueue(command); });
    }
}
