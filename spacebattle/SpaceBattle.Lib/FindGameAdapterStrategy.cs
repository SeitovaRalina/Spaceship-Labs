using System.Reflection;
using Hwdtech;
using SpaceBattle.Lib;

namespace SpaceGame.Lib;

public class FindGameAdapterStrategy : IStrategy
{
    public object Init(params object[] args)
    {
        var uObject = (IUObject)args[0];
        var typeTarget = (Type)args[1];

        var assemblyDictionary = IoC.Resolve<IDictionary<KeyValuePair<Type, Type>, Assembly>>("Game.Adapter.Assemblies.Dictionary");
        var pairOfTypes = new KeyValuePair<Type, Type>(uObject.GetType(), typeTarget);
        var assembly = assemblyDictionary[pairOfTypes];

        var type = assembly.GetType(IoC.Resolve<string>("Game.Adapter.AssemblyName", typeTarget));
        
        return Activator.CreateInstance(type!, uObject)!;
    }
}
