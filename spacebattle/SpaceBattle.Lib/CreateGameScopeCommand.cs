using Hwdtech;

namespace SpaceBattle.Lib;

public class CreateGameScopeCommand : ICommand
{
    private readonly string _gameID;
    private readonly object _parentScope;
    private readonly double _timeQuant;
    public CreateGameScopeCommand(string gameID, object parentScope, double timeQuant)
    {
        _gameID = gameID;
        _parentScope = parentScope;
        _timeQuant = timeQuant;
    }
    public void Execute()
    {
        var newGameScope = IoC.Resolve<object>("Scopes.New", _parentScope);

        var scopesDictionary = IoC.Resolve<IDictionary<string, object>>("Game.Scopes.Dictionary");
        scopesDictionary.Add(_gameID, newGameScope);

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", newGameScope).Execute();

        // стратегия для разрешения значения Кванта времени, выделенного для игры.
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Time.GetQuant",
            (object[] args) => (object)_timeQuant
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

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", _parentScope).Execute();
    }
}
