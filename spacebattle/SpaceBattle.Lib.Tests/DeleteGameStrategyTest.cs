using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests;

public class DeleteGameStrategyTest
{
    public DeleteGameStrategyTest()
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
            "Game.Delete",
            (object[] args) => new DeleteGameStrategy().Init(args)
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Command.CreateEmpty",
            (object[] args) => new Mock<ICommand>().Object
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Command.Inject",
            (object[] args) =>
            {
                var bridgeCommand = (IInjectableCommand)args[0];
                var commandToInject = (ICommand)args[1];
                bridgeCommand.Inject(commandToInject);
                return bridgeCommand;
            }
        ).Execute();
    }
    [Fact]
    public void SuccessfulDeletingGame()
    {
        var gameID = "0000000-0000-0000";

        var gamesDict = new Dictionary<string, IInjectableCommand>() { { gameID, new Mock<IInjectableCommand>().Object } };
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Dictionary",
            (object[] args) => gamesDict
        ).Execute();

        var gameScopesDict = new Dictionary<string, object>() { { gameID, "GameScope" } };
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Scopes.Dictionary",
            (object[] args) => gameScopesDict
        ).Execute();

        Assert.Single(gamesDict);
        Assert.Single(gameScopesDict);

        IoC.Resolve<ICommand>("Game.Delete", gameID).Execute();

        Assert.Single(gamesDict);
        Assert.Empty(gameScopesDict);
    }
}
