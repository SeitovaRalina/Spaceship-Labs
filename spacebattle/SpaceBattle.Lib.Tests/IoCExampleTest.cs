using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests;

public class IoCExampleTests
{
    public IoCExampleTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
    }

    [Fact]
    public void IoCExampleRegisteringDependencyForWorkingWithQueue()
    {
        var qReal = new Queue<Lib.ICommand>();
        var qMock = new Mock<IQueue>();
        qMock.Setup(q => q.Take()).Returns(qReal.Dequeue);
        qMock.Setup(q => q.Add(It.IsAny<Lib.ICommand>())).Callback(
        (Lib.ICommand cmd) =>
        {
            qReal.Enqueue(cmd);
        });

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Queue",
            (object[] args) =>
            {
                return qMock.Object;
            }
        ).Execute();

        var cmd = new Mock<Lib.ICommand>();
        IoC.Resolve<IQueue>("Game.Queue").Add(cmd.Object);

        var cmdFromIoC = IoC.Resolve<IQueue>("Game.Queue").Take();

        Assert.Equal(cmd.Object, cmdFromIoC);
    }

    [Fact]
    public void IoCExampleActionCommand()
    {
        var ac = new ActionCommand(() =>
        {

        });

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Command.EmptyCommand",
            (object[] args) =>
            {
                return ac;
            }
        ).Execute();
    }
}

public class ActionCommand : Lib.ICommand
{
    private readonly Action _action;
    public ActionCommand(Action action) => _action = action;
    public void Execute()
    {
        _action();
    }
}