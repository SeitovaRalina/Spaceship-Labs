using Hwdtech;

namespace SpaceBattle.Lib;

public interface ICollisionTrieBuilder
{
    public void BuildTrieFromFile(string path);
}

public class CollisionTrieBuilder : ICollisionTrieBuilder
{
    private static IEnumerable<IEnumerable<int>> ReadFileData(string dataPath)
    {
        return File.ReadAllLines(dataPath).Select(line => line.Split().Select(int.Parse));
    }
    
    public void BuildTrieFromFile(string path)
    {
        var trie = IoC.Resolve<CollisionTrie>("Game.CollisionTree");
        ReadFileData(path).ToList().ForEach(trie.Insert);
    }
}
