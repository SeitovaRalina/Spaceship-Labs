using Hwdtech;

namespace SpaceBattle.Lib;

public class BuildCollisionTreeCommand : ICommand
{
    private readonly string _path;

    public BuildCollisionTreeCommand(string path)
    {
        _path = path;
    }

    public void Execute()
    {
        // переход к построению префиксного дерева - Trie
        IoC.Resolve<ITrieBuilder>("Game.CollisionTre.Builder").BuildFromFile(_path);
    }
}
