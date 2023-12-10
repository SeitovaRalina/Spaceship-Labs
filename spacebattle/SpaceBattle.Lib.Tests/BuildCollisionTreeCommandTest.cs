using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests;
using IDict = IDictionary<int, object>;

public class TestCollisionTreeBuilder
{
    public TestCollisionTreeBuilder()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>(
            "Scopes.Current.Set",
            IoC.Resolve<object>(
                "Scopes.New",
                IoC.Resolve<object>("Scopes.Root")
            )
        ).Execute();

        var tree = new Dictionary<int, object>();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.CollisionTree",
            (object[] args) => tree
        ).Execute();

        var trieBuilder = new TrieBuilder();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.CollisionTree.Builder",
            (object[] args) => trieBuilder
        ).Execute();

    }

    [Fact]
    public void SuccessfullyBuildingCollisionTreeFromFileWithSomBranches()
    {
        var path = "../../../Data/collisions.txt";
        var buildtree = new BuildCollisionTreeCommand(path);

        buildtree.Execute();

        var resultingTree = IoC.Resolve<IDict>("Game.CollisionTree");

        // проверка многоуровневости
        Assert.Equal(2, resultingTree.Count);
        Assert.Equal(2, ((IDict)resultingTree[1]).Count);
        Assert.Equal(3, ((IDict)((IDict)resultingTree[1])[2]).Count);
        Assert.Equal(3, ((IDict)((IDict)((IDict)resultingTree[1])[3])[7]).Count);

        // проверка отдельных узлов 
        Assert.True(resultingTree.ContainsKey(5));
        Assert.True(((IDict)resultingTree[5]).ContainsKey(3));
        Assert.True(((IDict)((IDict)resultingTree[5])[3]).ContainsKey(7));
        Assert.True(((IDict)((IDict)((IDict)resultingTree[5])[3])[7]).ContainsKey(1));
    }

    [Fact]
    public void IncorrectFilePathInputThrowExceptionWhenBuildingTree()
    {
        var buildTree = new BuildCollisionTreeCommand("lalala.txt");

        Assert.Throws<FileNotFoundException>(buildTree.Execute);
    }
}
