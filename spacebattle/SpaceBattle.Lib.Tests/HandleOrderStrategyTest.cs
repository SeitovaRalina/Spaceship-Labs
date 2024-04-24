﻿using System.Collections.Concurrent;
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
            "Server.Thread.ThreadIDByGameID",
            (object[] args) => $"ThreadID for {(string)args[0]}"
        ).Execute();

        var cmdInterpretation = new Mock<ICommand>();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Command.Interpret",
            (object[] args) => cmdInterpretation.Object
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Server.WebHttp.Component.HandleOrder",
            (object[] args) => new HandleOrderStrategy().Init(args)
        ).Execute();
    }

    [Fact]
    public void SuccessfulOrderHandling()
    {
        var order = new Mock<IOrder>();
        order.SetupGet(o => o.GameID).Returns("0000000-0000-0000").Verifiable();

        var queueBaseOnFoundThreadID = new BlockingCollection<ICommand>();

        var sendToThreadCommand = new Mock<ICommand>();
        sendToThreadCommand.Setup(c => c.Execute()).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Server.Thread.SendCommand",
            (object[] args) =>
            {
                var message = (ICommand)args[1];
                queueBaseOnFoundThreadID.Add(message);
                return sendToThreadCommand.Object;
            }
        ).Execute();

        sendToThreadCommand.Verify(cmd => cmd.Execute(), Times.Never);

        IoC.Resolve<ICommand>("Server.WebHttp.Component.HandleOrder", order.Object).Execute();

        order.VerifyAll();
        Assert.Single(queueBaseOnFoundThreadID);
        sendToThreadCommand.Verify(cmd => cmd.Execute(), Times.Once);
    }

    [Fact]
    public void TryGetGameIDThrowsException()
    {
        var order = new Mock<IOrder>();
        order.SetupGet(o => o.GameID).Throws(() => new Exception()).Verifiable();

        var sendToThreadCommand = new Mock<ICommand>();
        sendToThreadCommand.Setup(c => c.Execute()).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Server.Thread.SendCommand",
            (object[] args) => sendToThreadCommand.Object
        ).Execute();

        sendToThreadCommand.Verify(cmd => cmd.Execute(), Times.Never);

        Assert.Throws<Exception>(() => IoC.Resolve<ICommand>("Server.WebHttp.Component.HandleOrder", order.Object).Execute());

        order.VerifyAll();
        sendToThreadCommand.Verify(cmd => cmd.Execute(), Times.Never);
    }
}
