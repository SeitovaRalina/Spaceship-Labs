using Hwdtech;

namespace SpaceBattle.Lib;

public class DeleteGameUObjectStrategy : IStrategy
{
    public object Init(params object[] args)
    {
        var gameID = (string)args[0];
        var objectID = (int)args[1];

        // получить по id игры нужный словарь с игровыми объектами
        var objectsDictionary = IoC.Resolve<Dictionary<int, IUObject>>("Game.UObjects.GetByGameID", gameID);

        return new ActionCommand(() => { objectsDictionary.Remove(objectID); });
    }
}
