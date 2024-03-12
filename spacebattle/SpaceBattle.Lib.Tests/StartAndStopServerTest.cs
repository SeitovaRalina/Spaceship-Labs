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
        var _senderDictionary = new ConcurrentDictionary<int, ISender>();

        _sendToThreadCommand = new Mock<ICommand>();
        _sendToThreadCommand.Setup(c => c.Execute()).Verifiable();

        _startThreadCommand =  new Mock<ICommand>();
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

                return new ActionCommand(() =>
                {
                    var queue = new BlockingCollection<ICommand>();
                    _senderDictionary[id] = new SenderAdapter(queue);

                    // аналогично происходит инициализация класса ServerThread 
                    // и добавление его в словарь потоков ConcurrentDictionary<int, ServerThread>

                    _startThreadCommand.Object.Execute();
                });
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

                return new ActionCommand(() =>
                {
                    new SendCommand(id, message).Execute();
                    _sendToThreadCommand.Object.Execute();
                });
            }
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Server.Thread.SoftStop",
            (object[] args) => new Mock<ICommand>().Object
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register", 
            "Server.Stop",
            (object[] args) => new StopServerCommand()
        ).Execute();
    }

    [Fact]
    public void SuccessfulStartingandStoppingServer()
    {
        var sizeServer = 3;

        IoC.Resolve<ICommand>("Server.Start", sizeServer).Execute();

        var currentSenderDictionary = IoC.Resolve<ConcurrentDictionary<int, ISender>>("Server.Thread.SenderDictionary");
        
        Assert.True(currentSenderDictionary.Count() == sizeServer);
        _startThreadCommand.Verify(cmd => cmd.Execute(), Times.Exactly(sizeServer));

        IoC.Resolve<ICommand>("Server.Stop").Execute();

        _sendToThreadCommand.Verify(cmd => cmd.Execute(), Times.Exactly(sizeServer));
    }
}
