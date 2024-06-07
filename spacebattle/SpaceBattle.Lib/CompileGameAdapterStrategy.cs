using System.Reflection;
using Hwdtech;

namespace SpaceBattle.Lib;

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

        assemblyDictionary.Add(pairOfTypes, adapterAssembly);
    }
}
