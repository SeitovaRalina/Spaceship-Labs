using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests;

public class MacroCommandTest
{
    public MacroCommandTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>(
            "Scopes.Current.Set",
            IoC.Resolve<object>(
                "Scopes.New",
                IoC.Resolve<object>("Scopes.Root")
            )
        ).Execute();

        var nameOperation = "MovementAndRotation";
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Component." + nameOperation,
            (object[] args) =>
                new string[] { "Game.Command.CreateMove", "Game.Command.CreateTurn" }
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
            "Game.Strategy.MacroCommand",
            (object[] args) =>
            {
                var nameOp = (string)args[0];
                var uObj = (IUObject)args[1];
                return new MacroCommandStrategy().Init(nameOp, uObj);
            }
        ).Execute();
    }

    [Fact]
    public void SuccessfulCreateAndRunMacroCommand()
    {
        var obj = new Mock<IUObject>();

        var moveCommand = new Mock<ICommand>();
        moveCommand.Setup(x => x.Execute()).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Command.CreateMove",
            (object[] args) => moveCommand.Object
        ).Execute();

        var turnCommand = new Mock<ICommand>();
        turnCommand.Setup(x => x.Execute()).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Command.CreateTurn",
            (object[] args) => turnCommand.Object
        ).Execute();

        var macroCommand = IoC.Resolve<ICommand>("Game.Strategy.MacroCommand", "MovementAndRotation", obj.Object);

        macroCommand.Execute();

        moveCommand.Verify(x => x.Execute(), Times.Once);
        turnCommand.Verify(x => x.Execute(), Times.Once);
    }
}
