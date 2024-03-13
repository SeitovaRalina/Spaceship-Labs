using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests;

public class LogFileCommandTest
{
    public LogFileCommandTest()
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
            "Component.GetLogMessage", (object[] args) =>
            {
                var message = new GetLogMessageStrategy().Init(args[0]);
                return message;
            }
        ).Execute();
    }

    [Fact]
    public void SuccessfulLogginginTemporaryFile()
    {
        var tempFilePath = Path.GetTempFileName();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.ExceptionHandler.GetLogFileName",
            (object[] args) => tempFilePath
        ).Execute();
        var types = new List<Type> { typeof(string), typeof(ArgumentException), typeof(ICommand) };

        var logFileCommand = new LogFileCommand(types);

        logFileCommand.Execute();

        var logFileLines = File.ReadAllLines(tempFilePath);
        File.Delete(tempFilePath);

        Assert.Equal(logFileLines[0], IoC.Resolve<string>("Component.GetLogMessage", types));
    }
}
