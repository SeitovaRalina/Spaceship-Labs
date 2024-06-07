using System.Reflection;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests;

public class CompileGameAdapterStrategyTest
{
    public CompileGameAdapterStrategyTest()
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
            "Game.Adapter.Compile",
            (object[] args) => new CompileGameAdapterCommand((Type)args[0], (Type)args[1])
        ).Execute();
    }
    [Fact]
    public void SuccessfulAddingGameAdapterAssemblyAfterCompilation()
    {
        var typeObject = typeof(IUObject);
        var typeTarget = typeof(ITurnable);

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Adapter.GenerateCode",
            (object[] args) => ";"
        ).Execute();

        var currentAsm = Assembly.LoadFrom("SpaceBattle.Lib.Tests.dll");
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Code.Compile",
            (object[] args) => currentAsm
        ).Execute();

        var asmDict = new Dictionary<KeyValuePair<Type, Type>, Assembly>();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Adapter.Assemblies.Dictionary",
            (object[] args) => asmDict
        ).Execute();

        IoC.Resolve<ICommand>("Game.Adapter.Compile", typeObject, typeTarget).Execute();

        Assert.Single(asmDict);
        Assert.Equal(new KeyValuePair<Type, Type>(typeObject, typeTarget), asmDict.First().Key);
        Assert.Equal(currentAsm, asmDict.First().Value);
    }
}
