using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests;

public class CreateEmptyGameUObjectsCommandTest
{
    public CreateEmptyGameUObjectsCommandTest()
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
            "Game.UObjects.CreateEmpty",
            (object[] args) => new CreateEmptyGameUObjectsCommand((int)args[0])
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.UObject.Create",
            (object[] args) => new Mock<IUObject>().Object
        ).Execute();
    }
    [Fact]
    public void SuccessfulCreatingEmptyGameUObjects()
    {
        var uObjectsCount = 5;

        var uObjectsDict = new Dictionary<int, IUObject>();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.UObjects.Dictionary",
            (object[] args) => uObjectsDict
        ).Execute();

        IoC.Resolve<ICommand>("Game.UObjects.CreateEmpty", uObjectsCount).Execute();

        Assert.Equal(uObjectsCount, uObjectsDict.Count);
        Assert.True(uObjectsDict.Keys.SequenceEqual(Enumerable.Range(0, uObjectsCount)));
    }
}
