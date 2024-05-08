using Hwdtech;

namespace SpaceBattle.Lib;

public class CreateGameScopeStrategy : IStrategy
{
    public object Init(params object[] args)
    {
        var gameID = (string)args[0];
        var parentScope = args[1];
        var timeQuant = (double)args[2];

        var newGameScope = IoC.Resolve<object>("Scopes.New", parentScope);

        var scopesDictionary = IoC.Resolve<IDictionary<string, object>>("Game.Scopes.Dictionary");
        scopesDictionary.Add(gameID, newGameScope);

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", newGameScope).Execute();

        // стратегия для разрешения значения Кванта времени, выделенного для игры.
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Time.GetQuant",
            (object[] args) => (object)timeQuant
        ).Execute();
        // Scope Игры содержит стратегии для разрешения Команд, выполняемых над очередью Игры
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Queue.Enqueue",
            (object[] args) => new EnqueueGameQueueStrategy().Init(args)
        ).Execute();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Queue.Dequeue",
            (object[] args) => new DequeueGameQueueStrategy().Init(args)
        ).Execute();
        // Scope Игры содержит стратегии для разрешения Команд, выполняемых над множеством игровых объектов
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.UObject.Get",
            (object[] args) => new GetGameUObjectStrategy().Init(args)
        ).Execute();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.UObject.Delete",
            (object[] args) => new DeleteGameUObjectStrategy().Init(args)
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", parentScope).Execute();

        return newGameScope;
    }
}
