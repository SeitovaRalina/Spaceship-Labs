using Hwdtech;

namespace SpaceBattle.Lib;

public class CreateNewGameStrategy : IStrategy
{
    public object Init(params object[] args)
    {
        var gameID = (string)args[0];

        var gameQueue = IoC.Resolve<Queue<ICommand>>("Game.Queue.CreateNew", gameID);
        var gameScope = IoC.Resolve<IDictionary<string, object>>("Game.Scopes.Dictionary")[gameID];
        var gameLikeCommand = IoC.Resolve<ICommand>("Game.Command", gameScope, gameQueue);

        var bridgeCommand = IoC.Resolve<ICommand>("Game.Command.Bridge", gameLikeCommand);

        var gamesDictionary = IoC.Resolve<IDictionary<string, ICommand>>("Game.Dictionary");
        gamesDictionary.Add(gameID, bridgeCommand);

        return bridgeCommand;
    }
}
