using Hwdtech;

namespace SpaceBattle.Lib;

public class DequeueGameQueueStrategy : IStrategy
{
    public object Init(params object[] args)
    {
        var gameID = (string)args[0];
        var gameQueue = IoC.Resolve<Queue<ICommand>>("Game.Queue.GetByGameID", gameID);

        return gameQueue.Dequeue();
    }
}
