using System.Reflection;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests;

public class CreateGameAdapterStrategyTest
{
    public CreateGameAdapterStrategyTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>(
            "Scopes.Current.Set",
            IoC.Resolve<object>(
                "Scopes.New",
                IoC.Resolve<object>("Scopes.Root")
            )
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Adapter.Create",
            (object[] args) => new CreateGameAdapterStrategy().Init(args)
        ).Execute();
    }
    [Fact]
    public void SuccessfulCreatingGameAdapterWithoutCompilation()
    {
        var uObject = new Mock<IUObject>();
        var typeTarget = typeof(Type);

        var asmDict = new Dictionary<KeyValuePair<Type, Type>, Assembly>
        {
            [new KeyValuePair<Type, Type>(uObject.Object.GetType(), typeTarget)] = Assembly.LoadFrom("SpaceBattle.Lib.Tests.dll")
        };
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Adapter.Assemblies.Dictionary",
            (object[] args) => asmDict
        ).Execute();

        var compileCommand = new Mock<ICommand>();
        compileCommand.Setup(cmd => cmd.Execute()).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Adapter.Compile",
            (object[] args) => compileCommand.Object
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Adapter.Find",
            (object[] args) => "foundAdapter"
        ).Execute();

        Assert.NotEmpty(asmDict);
        compileCommand.Verify(x => x.Execute(), Times.Never());

        var createdAdapter = IoC.Resolve<object>("Game.Adapter.Create", uObject.Object, typeTarget);

        Assert.Equal("foundAdapter", createdAdapter);
        compileCommand.Verify(x => x.Execute(), Times.Never());
    }
    [Fact]
    public void SuccessfulCreatingGameAdapterWithCompilation()
    {
        var uObject = new Mock<IUObject>();
        var typeTarget = typeof(Type);

        var asmDict = new Dictionary<KeyValuePair<Type, Type>, Assembly>();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Adapter.Assemblies.Dictionary",
            (object[] args) => asmDict
        ).Execute();

        var compileCommand = new Mock<ICommand>();
        var key = new KeyValuePair<Type, Type>(uObject.Object.GetType(), typeTarget);
        var asm = new Mock<Assembly>().Object;
        compileCommand.Setup(cmd => cmd.Execute()).Callback(() => asmDict.Add(key, asm)).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Adapter.Compile",
            (object[] args) => compileCommand.Object
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Adapter.Find",
            (object[] args) => "foundAdapter"
        ).Execute();

        Assert.Empty(asmDict);
        compileCommand.Verify(x => x.Execute(), Times.Never());

        var createdAdapter = IoC.Resolve<object>("Game.Adapter.Create", uObject.Object, typeTarget);

        compileCommand.Verify(x => x.Execute(), Times.Once());
        Assert.Single(asmDict);
        Assert.Equal("foundAdapter", createdAdapter);
    }
}
