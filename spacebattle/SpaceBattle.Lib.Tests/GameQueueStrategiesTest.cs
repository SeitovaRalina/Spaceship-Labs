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
            "Game.Scopes.New",
            (object[] args) => new CreateGameScopeStrategy().Init(args)
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Scopes.Dictionary",
            (object[] args) => new Dictionary<string, object>()
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

        var scope = IoC.Resolve<object>("Game.Scopes.New", gameID, IoC.Resolve<object>("Scopes.Current"), 3D);
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();

        IoC.Resolve<ICommand>("Game.Queue.Enqueue", gameID, enqueuedCommand.Object).Execute();

        Assert.Single(gameQueue);

        var dequeuedCommand = IoC.Resolve<ICommand>("Game.Queue.Dequeue", gameID);

        Assert.Empty(gameQueue);
        enqueuedCommand.Equals(dequeuedCommand);
    }
    [Fact]
    public void TryGetGameQueueByGameIDThrowsException()
    {
        var gameID = "0000000-0000-0001";
        var enqueuedCommand = new Mock<ICommand>();

        var getGameQueueByGameID = new Mock<IStrategy>();
        getGameQueueByGameID.Setup(s => s.Init(gameID)).Throws(() => new Exception()).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Queue.GetByGameID",
            (object[] args) => getGameQueueByGameID.Object.Init((string)args[0])
        ).Execute();

        var scope = IoC.Resolve<object>("Game.Scopes.New", gameID, IoC.Resolve<object>("Scopes.Current"), 3D);
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();

        Assert.Throws<Exception>(() => IoC.Resolve<ICommand>("Game.Queue.Enqueue", gameID, enqueuedCommand.Object).Execute());
        Assert.Throws<Exception>(() => IoC.Resolve<ICommand>("Game.Queue.Dequeue", gameID));

        getGameQueueByGameID.VerifyAll();
    }
    [Fact]
    public void TryDequeueEmptyGameQueueThrowsException()
    {
        var gameID = "0000000-0000-0002";
        var gameQueue = new Queue<ICommand>();

        var getGameQueueByGameID = new Mock<IStrategy>();
        getGameQueueByGameID.Setup(s => s.Init(gameID)).Returns(gameQueue);
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Queue.GetByGameID",
            (object[] args) => getGameQueueByGameID.Object.Init((string)args[0])
        ).Execute();

        var scope = IoC.Resolve<object>("Game.Scopes.New", gameID, IoC.Resolve<object>("Scopes.Current"), 3D);
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();

        Assert.Empty(gameQueue);

        Assert.Throws<InvalidOperationException>(() => IoC.Resolve<ICommand>("Game.Queue.Dequeue", gameID));
    }
}
