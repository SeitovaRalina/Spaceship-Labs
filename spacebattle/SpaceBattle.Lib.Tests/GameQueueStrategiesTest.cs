using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests;

public class GameQueueStrategiesTest
{
    public GameQueueStrategiesTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>(
            "Scopes.Current.Set",
            IoC.Resolve<object>(
                "Scopes.New",
                IoC.Resolve<object>("Scopes.Root")
            )
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Queue.Enqueue",
            (object[] args) => new EnqueueGameQueueStrategy().Init(args)
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Queue.Dequeue",
            (object[] args) => new DequeueGameQueueStrategy().Init(args)
        ).Execute();
    }
    [Fact]
    public void SuccessfulEnqueuingAndDequeingGameQueue()
    {
        var gameID = "0000000-0000-0000";
        var enqueuedCommand = new Mock<ICommand>();
        var gameQueue = new Queue<ICommand>();

        var getGameQueueByGameID = new Mock<IStrategy>();
        getGameQueueByGameID.Setup(s => s.Init(gameID)).Returns(gameQueue);
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Queue.GetByGameID",
            (object[] args) => getGameQueueByGameID.Object.Init((string)args[0])
        ).Execute();

        IoC.Resolve<ICommand>("Game.Queue.Enqueue", gameID, enqueuedCommand.Object).Execute();

        Assert.Single(gameQueue);

        var dequeuedCommand = IoC.Resolve<ICommand>("Game.Queue.Dequeue", gameID);

        Assert.Empty(gameQueue);
        enqueuedCommand.Equals(dequeuedCommand);
    }
}
