using Hwdtech;

namespace SpaceBattle.Lib;

public class CreateEmptyGameUObjectsCommand : ICommand
{
    private readonly int _count;
    public CreateEmptyGameUObjectsCommand(int count)
    {
        _count = count;
    }
    public void Execute()
    {
        var objects = IoC.Resolve<Dictionary<int, IUObject>>("Game.UObjects.Dictionary");

        Enumerable.Range(0, _count).ToList().ForEach(
            i => objects.Add(i, IoC.Resolve<IUObject>("Game.UObject.Create"))
        );
    }
}
