using System.Reflection;
using Hwdtech;
using Hwdtech.Ioc;
using Microsoft.CodeAnalysis;

namespace SpaceBattle.Lib.Tests;

public class CompileStringCodeStrategyTest
{
    public CompileStringCodeStrategyTest()
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
            "Code.Compile",
            (object[] args) => new CompileStringCodeStrategy().Init(args)
        ).Execute();

    }
    [Fact]
    public void SuccessfulCompilationStringCode()
    {
        var assemblyName = "String Code Compilation";
        var references = new List<MetadataReference> {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(Assembly.LoadFrom("SpaceBattle.Lib.dll").Location)
        };
        var code = @"namespace SpaceBattle.Lib;
                    public class FakeClass
                    {
                        static int Square(int n) => n * n;
                    }";

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Code.Compile.AssemblyName",
            (object[] args) => assemblyName
        ).Execute();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Code.Compile.References",
            (object[] args) => references
        ).Execute();

        var assembly = IoC.Resolve<Assembly>("Code.Compile", code);

        var t = assembly.GetType("SpaceBattle.Lib.FakeClass");
        var instance = Activator.CreateInstance(t!)!;

        var square = t!.GetMethod("Square", BindingFlags.NonPublic | BindingFlags.Static);
        var result = square?.Invoke(null, new object[] { 7 });

        Assert.Equal("SpaceBattle.Lib.FakeClass", instance.GetType().ToString());
        Assert.Equal(49, result);
    }
}
