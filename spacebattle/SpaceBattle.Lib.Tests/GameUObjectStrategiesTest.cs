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
            "Game.UObject.Get",
            (object[] args) => new GetGameUObjectStrategy().Init(args)
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.UObject.Delete",
            (object[] args) => new DeleteGameUObjectStrategy().Init(args)
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

        var gameUObject = IoC.Resolve<IUObject>("Game.UObject.Get", gameID, uObjectID);

        Assert.Equal(uObjectsDict[uObjectID], gameUObject);
        Assert.Single(uObjectsDict);

        IoC.Resolve<ICommand>("Game.UObject.Delete", gameID, uObjectID).Execute();

        Assert.Empty(uObjectsDict);
    }
}
