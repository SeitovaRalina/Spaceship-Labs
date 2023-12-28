﻿using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests;

public class StartCommandTests
{
    private readonly static Mock<IQueue> queue;
    static StartCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        var movement = new Mock<ICommand>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Operations.Movement", (object[] args) => movement.Object).Execute();

        var injectable = new Mock<ICommand>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.Injectable", (object[] args) => injectable.Object).Execute();

        var setPropertiesCommand = new Mock<ICommand>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Properties.Set", (object[] args) => setPropertiesCommand.Object).Execute();

        queue = new Mock<IQueue>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Queue", (object[] args) => queue.Object).Execute();
    }

    [Fact]
    public void StartMoveCommandPropertiesAddMoveCommandQueue()
    {
        var uobject = new Mock<IUObject>();

        var startable = new Mock<IMoveCommandStartable>();
        startable.Setup(s => s.Target).Returns(uobject.Object);
        startable.Setup(s => s.Properties).Returns(new Dictionary<string, object>() { { "Velocity", new int[] { 1, 1 } } });

        var startMoveCommand = new StartMoveCommand(startable.Object);

        startMoveCommand.Execute();

        startable.Verify(s => s.Properties, Times.Once());
        queue.Verify(q => q.Add(It.IsAny<ICommand>()), Times.Once());
    }
}
