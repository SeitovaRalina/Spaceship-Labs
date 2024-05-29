using Hwdtech;

namespace SpaceBattle.Lib;

public class DeleteGameUObjectStrategy : IStrategy
{
    public object Init(params object[] args)
    {
        var gameID = (string)args[0];
        var objectID = (int)args[1];

        var objectsDictionary = IoC.Resolve<Dictionary<int, IUObject>>("Game.UObjects.GetByGameID", gameID);

        return new ActionCommand(() => { objectsDictionary.Remove(objectID); });
    }
}
