using System.Reflection;
using Hwdtech;

namespace SpaceBattle.Lib;

public class CompileGameAdapterStrategy : IStrategy
{
    public object Init(params object[] args)
    {
        var typeObject = (Type)args[0];
        var typeTarget = (Type)args[1];

        return new CompileGameAdapterCommand(typeObject, typeTarget);
    }
}

public class CompileGameAdapterCommand : ICommand
{
    private readonly Type _typeObject;
    private readonly Type _typeTarget;
    public CompileGameAdapterCommand(Type typeObject, Type typeTarget)
    {
        _typeObject = typeObject;
        _typeTarget = typeTarget;
    }
    public void Execute()
    {
        var adapterCode = IoC.Resolve<string>("Game.Adapter.GenerateCode", _typeObject, _typeTarget);
        var adapterAssembly = IoC.Resolve<Assembly>("Code.Compile", adapterCode);

        var assemblyDictionary = IoC.Resolve<IDictionary<KeyValuePair<Type, Type>, Assembly>>("Game.Adapter.Assemblies.Dictionary");
        var pairOfTypes = new KeyValuePair<Type, Type>(_typeObject, _typeTarget);

        assemblyDictionary[pairOfTypes] = adapterAssembly;
    }
}
