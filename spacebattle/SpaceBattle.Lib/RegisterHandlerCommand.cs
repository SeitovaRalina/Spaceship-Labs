using Hwdtech;

namespace SpaceBattle.Lib;

public interface IHandler
{
    public void Handle(ICommand command, Exception exception);
}

public class RegisterHandlerCommand : ICommand
{
    private readonly IEnumerable<Type> _types;
    private readonly IHandler _handler;

    public RegisterHandlerCommand(IEnumerable<Type> types, IHandler handler)
    {
        _types = types;
        _handler = handler;
    }
    public void Execute()
    {
        var hashcode = IoC.Resolve<object>("Component.GetHashCode", _types);
        var tree = IoC.Resolve<IDictionary<object, IHandler>>("Game.ExceptionHandler.Tree");

        tree.Add(hashcode, _handler);
    }
}
