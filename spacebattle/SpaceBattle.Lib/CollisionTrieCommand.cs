namespace SpaceBattle.Lib;
using Hwdtech;

public class CollisionTrieCommand : ICommand
{
    private readonly string _path;

    public CollisionTrieCommand(string path) => _path = path;

    public void Execute()
    {
        IoC.Resolve<ICollisionTrieBuilder>("Game.CollisionTrie.Builder").BuildTrieFromFile(_path);
    }
}
