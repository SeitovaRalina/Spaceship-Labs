using System.Reflection;
using Hwdtech;
using Hwdtech.Ioc;
using Microsoft.CodeAnalysis;

namespace SpaceBattle.Lib.Tests;

public class FindGameAdapterStrategyTest
{
    public FindGameAdapterStrategyTest()
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
            "Code.Compile.AssemblyName",
            (object[] args) => "Game Adapter Code Compilation"
        ).Execute();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Code.Compile.References",
            (object[] args) => new List<MetadataReference> {
                            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                            MetadataReference.CreateFromFile(Assembly.LoadFrom("SpaceBattle.Lib.dll").Location)}
        ).Execute();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Code.Compile",
            (object[] args) => new CompileStringCodeStrategy().Init(args)
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Adapter.Name",
            (object[] args) => args[0].ToString() + "Adapter"
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Adapter.Find",
            (object[] args) => new FindGameAdapterStrategy().Init(args)
        ).Execute();
    }
    [Fact]
    public void SuccessfulFindingGameAdapter()
    {
        var uObject = new Mock<IUObject>();
        uObject.Setup(x => x.GetProperty("Angle")).Returns(new VectorAngle(60));

        var targetType = typeof(ITurnable);

        var adapterCode = @"namespace SpaceBattle.Lib;

public class ITurnableAdapter : ITurnable
{
    private readonly IUObject _uObject;
    public ITurnableAdapter(IUObject uObject)
    {
        _uObject = uObject;
    }

    public VectorAngle Angle { get => (VectorAngle)_uObject.GetProperty(""Angle""); 
    set => _uObject.SetProperty(""Angle"", value); }

    public VectorAngle DeltaAngle => (VectorAngle)_uObject.GetProperty(""DeltaAngle"");
}
        ";
        var asmDict = new Dictionary<KeyValuePair<Type, Type>, Assembly>
        {
            [new KeyValuePair<Type, Type>(uObject.Object.GetType(), targetType)] = IoC.Resolve<Assembly>("Code.Compile", adapterCode)
        };
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Adapter.Assemblies.Dictionary",
            (object[] args) => asmDict
        ).Execute();

        var adapter = (ITurnable)IoC.Resolve<object>("Game.Adapter.Find", uObject.Object, targetType);

        Assert.NotNull(adapter);
        Assert.Equal("ITurnableAdapter", adapter.GetType().Name);
        Assert.Equal(new VectorAngle(60), adapter.Angle);
    }
}
