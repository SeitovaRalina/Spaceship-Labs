using Hwdtech;

namespace SpaceBattle.Lib;

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
            IoC.Resolve<ICommand>("Game.UObjects.Setup", id, _propertyName, iteratorByProperty).Execute());
        iteratorByProperty.Reset();
    }
}
