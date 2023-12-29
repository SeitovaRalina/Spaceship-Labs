using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests;

public class EndCommandTest
{
    public Mock<ICommand> emptyCommand;
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

        emptyCommand = new Mock<ICommand>();
        emptyCommand.Setup(x => x.Execute()).Verifiable();

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
                return emptyCommand.Object;
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
        var command = new Mock<ICommand>();
        var bridge = new BridgeCommand(command.Object);
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
        command.Setup(x => x.Execute()).Verifiable();

        bridge.Execute();
        command.Verify(x => x.Execute(), Times.Once);
        emptyCommand.Verify(x => x.Execute(), Times.Never);

        Assert.NotNull(obj.Object.GetProperty("DeltaAngle"));
        IoC.Resolve<ICommand>("Game.Command.CreateEndMove", endable.Object).Execute();
        Assert.Throws<KeyNotFoundException>(() => obj.Object.GetProperty("DeltaAngle"));

        bridge.Execute();
        command.Verify(x => x.Execute(), Times.Once); // единожды, потому что 1 раз уже сработала и больше не выполнялась
        emptyCommand.Verify(x => x.Execute(), Times.Once);
    }

    [Fact]
    public void BridgeCommandTest()
    {
        var command1 = new Mock<ICommand>();
        command1.Setup(x => x.Execute()).Verifiable();

        var command2 = new Mock<ICommand>();
        command2.Setup(x => x.Execute()).Verifiable();

        var bridge = new BridgeCommand(command1.Object);

        bridge.Execute();

        command1.Verify(x => x.Execute(), Times.Once);
        command2.Verify(x => x.Execute(), Times.Never);

        bridge.Inject(command2.Object);

        bridge.Execute();
        command1.Verify(x => x.Execute(), Times.Once);
        command2.Verify(x => x.Execute(), Times.Once);
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
