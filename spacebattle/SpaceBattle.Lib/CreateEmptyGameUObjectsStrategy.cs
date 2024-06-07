using Hwdtech;

namespace SpaceBattle.Lib;

public class CreateEmptyGameUObjectsStrategy : IStrategy
{
    public object Init(params object[] args)
    {
        var count = (int)args[0];

        var emptyNewUObject = IoC.Resolve<IUObject>("Game.UObject.CreateEmpty");
        var generator = new GameUObjectsGenerator(emptyNewUObject);

        return generator.CreateUObjects(count);
    }
}

public class GameUObjectsGenerator
{
    private readonly IUObject _uObject;
    public GameUObjectsGenerator(IUObject uObject)
    {
        _uObject = uObject;
    }
    public IEnumerable<IUObject> CreateUObjects(int count)
    {
        for (var i = 0; i < count; i++)
        {
            IoC.Resolve<ICommand>("Game.UObject.Add", i, _uObject).Execute();
            yield return _uObject;
        }
    }
}
