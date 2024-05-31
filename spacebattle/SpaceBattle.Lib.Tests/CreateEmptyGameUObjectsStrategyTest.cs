using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests;

public class CreateEmptyGameUObjectsStrategyTest
{
    public CreateEmptyGameUObjectsStrategyTest()
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
            (object[] args) => new CreateEmptyGameUObjectsStrategy().Init(args)
        ).Execute();
    }
    [Fact]
    public void SuccessfulCreatingEmptyGameUObjects()
    {
        var uObjectsCount = 3;

        var emptyUObject = new Mock<IUObject>();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.UObject.CreateEmpty",
            (object[] args) => emptyUObject.Object
        ).Execute();

        var uObjectsDict = new Dictionary<int, IUObject>();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.UObject.Add",
            (object[] args) => new ActionCommand(() => { uObjectsDict.Add((int)args[0], (IUObject)args[1]); })
        ).Execute();

        var generatedUObjects = IoC.Resolve<IEnumerable<IUObject>>("Game.UObjects.CreateEmpty", uObjectsCount);

        Assert.True(generatedUObjects.All(obj => obj == emptyUObject.Object));

        Assert.Equal(uObjectsCount, uObjectsDict.Count);
        Assert.True(uObjectsDict.Keys.SequenceEqual(Enumerable.Range(0, uObjectsCount)));
    }
}
