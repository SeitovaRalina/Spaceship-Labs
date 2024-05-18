using Hwdtech;

namespace SpaceBattle.Lib;

// SetEnemyPositionCommand и одновременно SetFuelVolumeCommad
public class SetPropertyCommand : ICommand
{
    private readonly int _objectID;
    private readonly string _propertyName;
    private readonly IEnumerator<object> _iterator;

    public SetPropertyCommand(int objectID, string propertyName, IEnumerator<object> iterator)
    {
        _objectID = objectID;
        _propertyName = propertyName;
        _iterator = iterator;
    }
    public void Execute()
    {
        var objects = IoC.Resolve<Dictionary<int, IUObject>>("Game.UObjects.Dictionary");

        IoC.Resolve<ICommand>("Game.UObject.SetProperty", objects[_objectID], _propertyName, _iterator.Current).Execute();
        _iterator.MoveNext();
    }
}
