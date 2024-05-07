using Hwdtech;

namespace SpaceBattle.Lib;

public class DeleteGameStrategy : IStrategy
{
    public object Init(params object[] args)
    {
        var gameID = (string)args[0];
        var gameBridgeCommand = IoC.Resolve<IDictionary<string, IInjectableCommand>>("Game.Dictionary")[gameID];
        var gameScopeDictionary = IoC.Resolve<IDictionary<string, object>>("Game.Scopes.Dictionary");

        return new ActionCommand(() =>
            {
                IoC.Resolve<IInjectableCommand>("Game.Command.Inject",
                    gameBridgeCommand,
                    IoC.Resolve<ICommand>("Game.Command.CreateEmpty"));

                gameScopeDictionary.Remove(gameID);
            }
        );
    }
}
