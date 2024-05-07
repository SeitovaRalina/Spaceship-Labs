using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests;

public class CreateGameScopeCommandTest
{
    public CreateGameScopeCommandTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Scopes.New",
            (object[] args) => new CreateGameScopeCommand((string)args[0], args[1], (double)args[2])
        ).Execute();
    }
    [Fact]
    public void SuccessfulCreatingOfGameScopeAndTestingGetTimeQuantumStrategy()
    {
        var gameID = "0000000-0000-0000";
        var parentScope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
        var quant = 4e3D;

        var gameScopesDict = new Dictionary<string, object>();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Scopes.Dictionary",
            (object[] args) => gameScopesDict
        ).Execute();

        IoC.Resolve<ICommand>("Game.Scopes.New", gameID, parentScope, quant).Execute();

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", parentScope).Execute();
        Assert.Throws<ArgumentException>(() => IoC.Resolve<object>("Game.Time.GetQuant"));

        Assert.Single(gameScopesDict);
        var gameScope = gameScopesDict[gameID];

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", gameScope).Execute();
        Assert.Equal(quant, IoC.Resolve<object>("Game.Time.GetQuant"));
    }
}
