using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests;

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
    }

    [Fact]
    public void SuccesfullCreateCollisionTreeCommand()

    {
        var trieBuilder = new Mock<ICollisionTrieBuilder>();
        trieBuilder.Setup(x => x.BuildTrieFromFile(It.IsAny<string>())).Verifiable();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register", 
            "Game.CollisionTrie.Builder", 
            (object[] args) => trieBuilder.Object).Execute();

        var collisionTrieCommand = new CollisionTrieCommand("dataPath");

        collisionTrieCommand.Execute();

        trieBuilder.Verify(x => x.BuildTrieFromFile("dataPath"), Times.Once());
    }
}
