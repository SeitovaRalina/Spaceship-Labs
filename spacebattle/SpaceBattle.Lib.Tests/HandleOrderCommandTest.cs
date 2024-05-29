using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests;

public class HandleOrderStrategyTest
{
    public HandleOrderStrategyTest()
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
            "Server.WebHttp.Command.HandleOrder",
            (object[] args) => new HandleOrderCommand((IOrder)args[0])
        ).Execute();
    }

    [Fact]
    public void SuccessfulOrderHandling()
    {
        var order = new Mock<IOrder>();
        order.SetupGet(o => o.GameID).Returns("0000000-0000-0000").Verifiable();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Server.Thread.ThreadIDByGameID",
            (object[] args) => $"ThreadID for {(string)args[0]}"
        ).Execute();

        var cmdInterpretation = new Mock<ICommand>();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Command.Interpret",
            (object[] args) => cmdInterpretation.Object
        ).Execute();

        var sendToThreadCommand = new Mock<ICommand>();
        sendToThreadCommand.Setup(c => c.Execute()).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Server.Thread.SendCommand",
            (object[] args) => sendToThreadCommand.Object
        ).Execute();

        var handleOrderCommand = IoC.Resolve<ICommand>("Server.WebHttp.Command.HandleOrder", order.Object);

        handleOrderCommand.Execute();

        order.VerifyAll();
        sendToThreadCommand.Verify(cmd => cmd.Execute(), Times.Once);
    }

    [Fact]
    public void TryGetGameIDFromOrderThrowsException()
    {
        var order = new Mock<IOrder>();
        order.SetupGet(o => o.GameID).Throws(() => new Exception()).Verifiable();

        var threadID = new Mock<IStrategy>();
        threadID.Setup(x => x.Init(It.IsAny<string>())).Returns("DEFAULT");
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Server.Thread.ThreadIDByGameID",
            (object[] args) => threadID.Object.Init((string)args[0])
        ).Execute();

        var cmdInterpretation = new Mock<IStrategy>();
        cmdInterpretation.Setup(x => x.Init(It.IsAny<IOrder>())).Returns(new Mock<ICommand>().Object);
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Command.Interpret",
            (object[] args) => cmdInterpretation.Object.Init((IOrder)args[0])
        ).Execute();

        var sendToThreadCommand = new Mock<ICommand>();
        sendToThreadCommand.Setup(c => c.Execute()).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Server.Thread.SendCommand",
            (object[] args) => sendToThreadCommand.Object
        ).Execute();

        var handleOrderCommand = IoC.Resolve<ICommand>("Server.WebHttp.Command.HandleOrder", order.Object);

        Assert.Throws<Exception>(handleOrderCommand.Execute);

        order.VerifyAll();
        sendToThreadCommand.Verify(cmd => cmd.Execute(), Times.Never);
    }

    [Fact]
    public void TryGetThreadIDbyGameIDThrowsException()
    {
        var order = new Mock<IOrder>();
        order.SetupGet(o => o.GameID).Returns("0000000-0000-0000");

        var threadID = new Mock<IStrategy>();
        threadID.Setup(x => x.Init(It.IsAny<string>())).Throws(() => new Exception()).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Server.Thread.ThreadIDByGameID",
            (object[] args) => threadID.Object.Init((string)args[0])
        ).Execute();

        var cmdInterpretation = new Mock<IStrategy>();
        cmdInterpretation.Setup(x => x.Init(It.IsAny<IOrder>())).Returns(new Mock<ICommand>().Object);
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Command.Interpret",
            (object[] args) => cmdInterpretation.Object.Init((IOrder)args[0])
        ).Execute();

        var sendToThreadCommand = new Mock<ICommand>();
        sendToThreadCommand.Setup(c => c.Execute()).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Server.Thread.SendCommand",
            (object[] args) => sendToThreadCommand.Object
        ).Execute();

        var handleOrderCommand = IoC.Resolve<ICommand>("Server.WebHttp.Command.HandleOrder", order.Object);

        Assert.Throws<Exception>(handleOrderCommand.Execute);

        threadID.Verify();
        sendToThreadCommand.Verify(cmd => cmd.Execute(), Times.Never);
    }

    [Fact]
    public void TryGetInterpretationCommandThrowsException()
    {
        var order = new Mock<IOrder>();
        order.SetupGet(o => o.GameID).Returns("0000000-0000-0000");

        var threadID = new Mock<IStrategy>();
        threadID.Setup(x => x.Init(It.IsAny<object[]>())).Returns("ThreadID for 0000000-0000-0000");
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Server.Thread.ThreadIDByGameID",
            (object[] args) => threadID.Object.Init(args)
        ).Execute();

        var cmdInterpretation = new Mock<IStrategy>();
        cmdInterpretation.Setup(x => x.Init(It.IsAny<object[]>())).Throws(() => new Exception()).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Command.Interpret",
            (object[] args) => cmdInterpretation.Object.Init(args)
        ).Execute();

        var sendToThreadCommand = new Mock<ICommand>();
        sendToThreadCommand.Setup(c => c.Execute()).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Server.Thread.SendCommand",
            (object[] args) => sendToThreadCommand.Object
        ).Execute();

        var handleOrderCommand = IoC.Resolve<ICommand>("Server.WebHttp.Command.HandleOrder", order.Object);

        Assert.Throws<Exception>(handleOrderCommand.Execute);

        cmdInterpretation.VerifyAll();
        sendToThreadCommand.Verify(cmd => cmd.Execute(), Times.Never);
    }

    [Fact]
    public void TryExecuteSendCommandThrowsException()
    {
        var order = new Mock<IOrder>();
        order.SetupGet(o => o.GameID).Returns("0000000-0000-0000");

        var threadID = new Mock<IStrategy>();
        threadID.Setup(x => x.Init(It.IsAny<object[]>())).Returns("ThreadID for 0000000-0000-0000");
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Server.Thread.ThreadIDByGameID",
            (object[] args) => threadID.Object.Init(args)
        ).Execute();

        var cmdInterpretation = new Mock<IStrategy>();
        cmdInterpretation.Setup(x => x.Init(It.IsAny<object[]>())).Returns(new Mock<ICommand>().Object);
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Command.Interpret",
            (object[] args) => cmdInterpretation.Object.Init(args)
        ).Execute();

        var sendToThreadCommand = new Mock<ICommand>();
        sendToThreadCommand.Setup(c => c.Execute()).Throws(() => new Exception()).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Server.Thread.SendCommand",
            (object[] args) => sendToThreadCommand.Object
        ).Execute();

        var handleOrderCommand = IoC.Resolve<ICommand>("Server.WebHttp.Command.HandleOrder", order.Object);

        Assert.Throws<Exception>(handleOrderCommand.Execute);

        sendToThreadCommand.Verify();
    }
}
