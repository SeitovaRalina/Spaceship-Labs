using System.Collections.Concurrent;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests;

public class CreateNewGameStrategyTest
{
    public CreateNewGameStrategyTest()
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
            "Game.Create",
            (object[] args) => new CreateNewGameStrategy().Init(args)
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Queue.CreateNew",
            (object[] args) => new Queue<ICommand>()
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Command.CreateMacro",
            (object[] args) =>
            {
                var commands = (IEnumerable<ICommand>)args[0];
                return new MacroCommand(commands);
            }
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Command.Bridge",
            (object[] args) => new BridgeCommand((ICommand)args[0])
        ).Execute();
    }
    [Fact]
    public void SuccessfulCreatingNewGame()
    {

        var gameID = "0000000-0000-0000";

        var gameScopesDict = new Dictionary<string, object>() { { gameID, "NewGameScope" } };
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Scopes.Dictionary",
            (object[] args) => gameScopesDict
        ).Execute();

        var gameLikeCommand = new Mock<ICommand>();
        gameLikeCommand.Setup(cmd => cmd.Execute()).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Command",
            (object[] args) => gameLikeCommand.Object
        ).Execute();

        var currentThreadQueue = new BlockingCollection<ICommand>();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Server.Thread.RepeatCommand",
            (object[] args) => new ActionCommand(() => { currentThreadQueue.Add((ICommand)args[0]); })
        ).Execute();

        var gamesDict = new Dictionary<string, ICommand>();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Dictionary",
            (object[] args) => gamesDict
        ).Execute();

        var otherCommand = new Mock<ICommand>();
        otherCommand.Setup(cmd => cmd.Execute()).Verifiable();
        currentThreadQueue.Add(otherCommand.Object);

        var game = IoC.Resolve<ICommand>("Game.Create", gameID);

        Assert.Equal(game, gamesDict[gameID]);

        gameLikeCommand.Verify(x => x.Execute(), Times.Never());
        Assert.Single(currentThreadQueue);

        currentThreadQueue.Add(game);
        Assert.True(currentThreadQueue.Count == 2);

        // Имитация работы ServerThread
        currentThreadQueue.Take().Execute();
        currentThreadQueue.Take().Execute();
        currentThreadQueue.Take().Execute();

        Assert.Single(currentThreadQueue);
        otherCommand.Verify(x => x.Execute(), Times.Once());
        gameLikeCommand.Verify(x => x.Execute(), Times.Exactly(2));
    }
}
