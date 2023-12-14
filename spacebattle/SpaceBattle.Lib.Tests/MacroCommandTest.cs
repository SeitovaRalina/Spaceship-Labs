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
    public void SuccessfulExampleOfCreatingAndRunningMacroCommand()
    {
        var nameOperation = "MovementAndRotationOperation";
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Component." + nameOperation,
            (object[] args) =>
                new string[] { "Game.Command.CreateMove", "Game.Command.CreateTurn" }
        ).Execute();

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

        var macroCommand = IoC.Resolve<ICommand>("Game.Strategy.MacroCommand", nameOperation, obj.Object);

        macroCommand.Execute();

        moveCommand.Verify(x => x.Execute(), Times.Once);
        turnCommand.Verify(x => x.Execute(), Times.Once);
    }

    [Fact]
    public void TryExecuteCommandsInMacroCommandThrowException()
    {
        var command1 = new Mock<ICommand>();
        command1.Setup(x => x.Execute()).Throws(new Exception());

        var command2 = new Mock<ICommand>();
        command2.Setup(x => x.Execute()).Verifiable();

        var commands = new List<ICommand> {command1.Object, command2.Object};

        Assert.Throws<Exception>(() => new MacroCommand(commands).Execute());
        command2.Verify(x => x.Execute(), Times.Never);
    }
}
