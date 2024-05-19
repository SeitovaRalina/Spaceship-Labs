using Hwdtech;

namespace SpaceBattle.Lib;

public class SetUObjectPropertyStrategy : IStrategy
{
    public object Init(params object[] args)
    {
        var uObject = IoC.Resolve<Dictionary<int, IUObject>>("Game.UObjects.Dictionary")[(int)args[0]];
        var propertyName = (string)args[1];
        var iterator = (IEnumerator<object>)args[2];

        return new ActionCommand(() =>
            {
                uObject.SetProperty(propertyName, iterator.Current);
                iterator.MoveNext();
            });
    }
}

public class SetupObjectsPropertyCommand : ICommand
{
    private readonly IEnumerable<int> _objectIDs;
    private readonly string _propertyName;
    public SetupObjectsPropertyCommand(IEnumerable<int> objectIDs, string propertyName)
    {
        _objectIDs = objectIDs;
        _propertyName = propertyName;
    }
    public void Execute()
    {
        var values = IoC.Resolve<IEnumerable<object>>("Game.Enumerable.GetByPropertyName", _propertyName);
        var iteratorByProperty = values.GetEnumerator();

        _objectIDs.ToList().ForEach(id =>
            IoC.Resolve<ICommand>("Game.UObject.SetProperty", id, _propertyName, iteratorByProperty).Execute()
        );
        iteratorByProperty.Reset();
    }
}
