using Hwdtech;

namespace SpaceBattle.Lib;

public interface IStrategy
{
    public object Init(params object[] args);
}

public class MacroCommandStrategy : IStrategy
{
    public object Init(params object[] args)
    {
        var nameOperation = (string)args[0];
        var obj = (IUObject)args[1];

        var dependencies = IoC.Resolve<string[]>("Component." + nameOperation);
        var commands = dependencies.Select(dependency => IoC.Resolve<ICommand>(dependency, obj));

        return IoC.Resolve<ICommand>("Game.Command.CreateMacro", commands);
    }
}
