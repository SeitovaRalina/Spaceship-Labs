using System.Collections.Concurrent;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests;

public class StartAndStopServerTest
{
    private readonly Mock<ICommand> _sendToThreadCommand;
    private readonly Mock<ICommand> _startThreadCommand;
    public StartAndStopServerTest()
    {
        var _senderDictionary = new ConcurrentDictionary<int, BlockingCollection<ICommand>>();

        _sendToThreadCommand = new Mock<ICommand>();
        _sendToThreadCommand.Setup(c => c.Execute()).Verifiable();

        _startThreadCommand = new Mock<ICommand>();
        _startThreadCommand.Setup(c => c.Execute()).Verifiable();

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
            "Server.Thread.SenderDictionary",
            (object[] args) => _senderDictionary
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Server.Thread.Start",
            (object[] args) =>
            {
                var id = (int)args[0];

                _senderDictionary[id] = new BlockingCollection<ICommand>();

                // аналогично происходит инициализация класса ServerThread 
                // и добавление его в словарь потоков ConcurrentDictionary<int, ServerThread>

                return _startThreadCommand.Object;
            }
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Server.Start",
            (object[] args) => new StartServerCommand((int)args[0])
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Server.Thread.SendCommand",
            (object[] args) =>
            {
                var id = (int)args[0];
                var message = (ICommand)args[1];

                var queue = _senderDictionary[id];
                queue.Add(message);

                return _sendToThreadCommand.Object;
            }
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Server.Thread.SoftStop",
            (object[] args) =>
            {
                // представим, что SoftStopCommand успешно отработала
                // после завершения потока вызывается Action

                var action = (Action)args[1];
                action.Invoke();

                return new Mock<ICommand>().Object;
            }
        ).Execute();
    }

    [Fact]
    public void SuccessfulStartingandStoppingServer()
    {
        var sizeServer = 3;
        var currentSenderDictionary = IoC.Resolve<ConcurrentDictionary<int, BlockingCollection<ICommand>>>("Server.Thread.SenderDictionary");
        var barrier = new Barrier(participantCount: sizeServer + 1);

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Server.Thread.SoftStop.Action",
            (object[] args) =>
            {
                return () => { barrier.RemoveParticipant(); };
            }
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Server.Stop",
            (object[] args) => new ActionCommand(() =>
                {
                    new StopServerCommand().Execute();
                    // завершить основной поток
                    barrier.SignalAndWait();
                })
        ).Execute();

        Assert.True(currentSenderDictionary.Count() == 0);

        IoC.Resolve<ICommand>("Server.Start", sizeServer).Execute();

        Assert.True(currentSenderDictionary.Count() == sizeServer);
        Assert.True(currentSenderDictionary.All(pair => pair.Value.Count == 0));
        Assert.Equal(barrier.CurrentPhaseNumber, 0);
        _startThreadCommand.Verify(cmd => cmd.Execute(), Times.Exactly(sizeServer));

        IoC.Resolve<ICommand>("Server.Stop").Execute();

        Assert.True(currentSenderDictionary.All(pair => pair.Value.Count == 1));
        Assert.Equal(barrier.CurrentPhaseNumber, 1);
        _sendToThreadCommand.Verify(cmd => cmd.Execute(), Times.Exactly(sizeServer));
    }

    [Fact]
    public void StartingandStoppingServerAsConsoleApplication()
    {
        // без барьеров
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Server.Thread.SoftStop.Action",
            (object[] args) => () => { }
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Server.Stop",
            (object[] args) => new StopServerCommand()
        ).Execute();

        var sizeServer = 11;

        var consoleInput = new StringReader("any");
        var consoleOutput = new StringWriter();
        Console.SetIn(consoleInput);
        Console.SetOut(consoleOutput);

        var app = new RunApplicationCommand(sizeServer);

        app.Execute();

        var output = consoleOutput.ToString();

        Assert.Contains($"The server is starting with the number of threads {sizeServer}", output);
        Assert.Contains("Press any key to stop the server softly ...", output);
        Assert.Contains("The server has successfully stopped its work. Press any key to exit...", output);
    }
}
