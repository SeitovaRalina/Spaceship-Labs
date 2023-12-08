namespace SpaceBattle.Lib;

public class TrieNode
{
    public IDictionary<int, TrieNode> Children { get; } = new Dictionary<int, TrieNode>();
}

public class CollisionTrie
{
    private readonly TrieNode _root = new TrieNode();

    public void Insert(IEnumerable<int> branch)
    {
        var node = _root;

        branch.ToList().ForEach(num =>
        {
            node.Children[num] = node.Children.ContainsKey(num) ? node.Children[num] : new TrieNode();
            node = node.Children[num];            
        });
    }
}
