using Hwdtech;

namespace SpaceBattle.Lib;

public class GetLogMessageStrategy : IStrategy
{
    public object Init(params object[] args)
    {
        var types = (IEnumerable<Type>)args[0];
        return string.Join(" ~ ", types.Select(t => t.ToString()));
    }
}

public class LogFileCommand : ICommand
{
    private readonly IEnumerable<Type> _types;

    public LogFileCommand(IEnumerable<Type> types)
    {
        _types = types;
    }
    public void Execute()
    {
        var logFilePath = IoC.Resolve<string>("Game.ExceptionHandler.GetLogFileName");
        var message = IoC.Resolve<string>("Component.GetLogMessage", _types);

        using (var writer = new StreamWriter(logFilePath))
        {
            writer.WriteLine(message);
        }
    }
}
