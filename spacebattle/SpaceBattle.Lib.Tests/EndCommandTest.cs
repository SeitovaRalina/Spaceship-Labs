using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests;

public class EndCommandTest
{
    public EndCommandTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>(
            "Scopes.Current.Set",
            IoC.Resolve<object>(
                "Scopes.New",
                IoC.Resolve<object>("Scopes.Root")
            )
        ).Execute();

        ICommand emptyCommand = new EmptyCommand();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Command.CreateEndMove",
            (object[] args) =>
            {
                var command = (IMoveCommandEndable)args[0];
                return new EndMoveCommand(command);
            }
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Command.DeleteUObjectProperties",
            (object[] args) =>
            {
                var obj = (IUObject)args[0];
                var keys = (IEnumerable<string>)args[1];
                keys.ToList().ForEach(p => obj.DeleteProperty(p));
                return "ok";
            }
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Command.CreateEmpty",
            (object[] args) =>
            {
                return emptyCommand;
            }
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
    public void EndMoveCommandTest()
    {
        var endable = new Mock<IMoveCommandEndable>();
        var bridge = new BridgeCommand(new Mock<ICommand>().Object);
        var obj = new Mock<IUObject>();

        var keys = new List<string> { "DeltaAngle" };
        var properties = new Dictionary<string, object>();

        obj.Setup(x => x.GetProperty(It.IsAny<string>())).Returns((string s) => properties[s]);
        obj.Setup(x => x.SetProperty(It.IsAny<string>(), It.IsAny<object>()))
            .Callback<string, object>(properties.Add);
        obj.Setup(x => x.DeleteProperty(It.IsAny<string>()))
            .Callback<string>(name => properties.Remove(name));

        obj.Object.SetProperty("DeltaAngle", 45);

        endable.SetupGet(x => x.Move).Returns(bridge).Verifiable();
        endable.SetupGet(x => x.Object).Returns(obj.Object);
        endable.SetupGet(x => x.Properties).Returns(keys);

        IoC.Resolve<ICommand>("Game.Command.CreateEndMove", endable.Object).Execute();

        Assert.Throws<KeyNotFoundException>(() => obj.Object.GetProperty("DeltaAngle"));
        Assert.True(bridge.GetCommand() == IoC.Resolve<ICommand>("Game.Command.CreateEmpty"));
    }

    [Fact]
    public void BridgeCommandTest()
    {
        var command = new Mock<ICommand>();
        command.Setup(x => x.Execute()).Verifiable();

        var bridge = new BridgeCommand(command.Object);
        bridge.Inject(IoC.Resolve<ICommand>("Game.Command.CreateEmpty"));

        bridge.Execute();

        command.Verify(x => x.Execute(), Times.Never());
    }

    [Fact]
    public void EmptyCommandTest()
    {
        var emptyCommand = IoC.Resolve<ICommand>("Game.Command.CreateEmpty");

        emptyCommand.Execute();

        Assert.NotNull(emptyCommand);
    }

    [Fact]
    public void TryGetSomePropertyOfMoveCommandEndableThrowsException()
    {
        var obj = new Mock<IUObject>();
        var bridge = new BridgeCommand(new Mock<ICommand>().Object);
        var endable = new Mock<IMoveCommandEndable>();

        endable.SetupGet(x => x.Move).Returns(bridge).Verifiable();
        endable.SetupGet(x => x.Object).Returns(obj.Object);
        endable.SetupGet(x => x.Properties).Throws(new Exception());

        var endMoveCommand = IoC.Resolve<ICommand>("Game.Command.CreateEndMove", endable.Object);

        Assert.Throws<Exception>(() => endMoveCommand.Execute());
    }
}
