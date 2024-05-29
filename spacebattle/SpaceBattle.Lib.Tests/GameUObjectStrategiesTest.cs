using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests;

public class GameUObjectStrategiesTest
{
    public GameUObjectStrategiesTest()
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
    public void SuccessfulGettingGameUObjectAndDeletingItByID()
    {
        var gameID = "0000000-0000-0000";
        var uObjectID = 1;

        var uObjectsDict = new Dictionary<int, IUObject> { { uObjectID, new Mock<IUObject>().Object } };
        var getUObjectsDictByGameID = new Mock<IStrategy>();
        getUObjectsDictByGameID.Setup(s => s.Init(gameID)).Returns(uObjectsDict);
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.UObjects.GetByGameID",
            (object[] args) => getUObjectsDictByGameID.Object.Init((string)args[0])
        ).Execute();

        var scope = IoC.Resolve<object>("Game.Scopes.New", gameID, IoC.Resolve<object>("Scopes.Current"), 3D);
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();

        var gameUObject = IoC.Resolve<IUObject>("Game.UObject.Get", gameID, uObjectID);

        Assert.Equal(uObjectsDict[uObjectID], gameUObject);
        Assert.Single(uObjectsDict);

        IoC.Resolve<ICommand>("Game.UObject.Delete", gameID, uObjectID).Execute();

        Assert.Empty(uObjectsDict);
    }
    [Fact]
    public void TryFoundObjectsDictForTheGameThrowsException()
    {
        var gameID = "0000000-0000-0001";
        var uObjectID = 1;

        var getUObjectsDictByGameID = new Mock<IStrategy>();
        getUObjectsDictByGameID.Setup(s => s.Init(gameID)).Throws(() => new Exception()).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.UObjects.GetByGameID",
            (object[] args) => getUObjectsDictByGameID.Object.Init((string)args[0])
        ).Execute();

        var scope = IoC.Resolve<object>("Game.Scopes.New", gameID, IoC.Resolve<object>("Scopes.Current"), 3D);
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();

        Assert.Throws<Exception>(() => IoC.Resolve<IUObject>("Game.UObject.Get", gameID, uObjectID));
        Assert.Throws<Exception>(() => IoC.Resolve<ICommand>("Game.UObject.Delete", gameID, uObjectID).Execute());

        getUObjectsDictByGameID.VerifyAll();
    }
    [Fact]
    public void TryGetGameUObjectThrowsException()
    {
        var gameID = "0000000-0000-0002";
        var uObjectID = 1;

        var uObjectsDict = new Dictionary<int, IUObject>();
        var getUObjectsDictByGameID = new Mock<IStrategy>();
        getUObjectsDictByGameID.Setup(s => s.Init(gameID)).Returns(uObjectsDict);
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.UObjects.GetByGameID",
            (object[] args) => getUObjectsDictByGameID.Object.Init((string)args[0])
        ).Execute();

        var scope = IoC.Resolve<object>("Game.Scopes.New", gameID, IoC.Resolve<object>("Scopes.Current"), 3D);
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();

        Assert.Empty(uObjectsDict);

        Assert.Throws<KeyNotFoundException>(() => IoC.Resolve<IUObject>("Game.UObject.Get", gameID, uObjectID));
    }
}
