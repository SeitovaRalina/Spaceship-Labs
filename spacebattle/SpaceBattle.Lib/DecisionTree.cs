namespace SpaceBattle.Lib;
using Dict = Dictionary<int, object>;
using Hwdtech;

public class DecisionTree : ICommand
{
    private readonly string _path;
    public DecisionTree(string path) => _path = path;

    public void Execute()
    {
        
    }
}
