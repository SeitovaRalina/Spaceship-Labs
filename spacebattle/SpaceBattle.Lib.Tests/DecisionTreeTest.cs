namespace SpaceBattle.Lib.Tests;
using Dict = Dictionary<int, object>;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;

public class DecisionTreeTest
{
    readonly string path;

    public DecisionTreeTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>(
            "Scopes.Current.Set", 
            IoC.Resolve<object>("Scopes.New", 
            IoC.Resolve<object>("Scopes.Root")))
        .Execute();

        var tree = new Dict();
        IoC.Resolve<ICommand>(
            "IoC.Register", "Game.DecisionTree", 
            (object[] args) => tree
        ).Execute();

        path = @"../../../Vectors.txt";
    }
    
    [Fact]
    public void BuildDecisionTreeWithoutExceptions()
    {
        var decisionTreeCommand = new DecisionTree(path);
        decisionTreeCommand.Execute();

        var tree = IoC.Resolve<Dict>("Game.DecisionTree");

        Assert.True(
            tree.ContainsKey(4)
        );

        Assert.True(
            ((Dict)tree[4]).ContainsKey(6)
        );

        Assert.True(
            ((Dict)((Dict)tree[4])[6]).ContainsKey(4)
        );

        Assert.True(
            ((Dict)((Dict)((Dict)tree[4])[6])[4]).ContainsKey(6)
        );
    }
}