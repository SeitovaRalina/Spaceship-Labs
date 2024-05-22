using System.Reflection;
using Hwdtech;

namespace SpaceBattle.Lib;

public class FindGameAdapterStrategy : IStrategy
{
    public object Init(params object[] args)
    {
        var uObject = (IUObject)args[0];
        var typeTarget = (Type)args[1];

        var assemblyDictionary = IoC.Resolve<IDictionary<KeyValuePair<Type, Type>, Assembly>>("Game.Adapter.Assemblies.Dictionary");
        var pairOfTypes = new KeyValuePair<Type, Type>(uObject.GetType(), typeTarget);
        var assembly = assemblyDictionary[pairOfTypes];

        var type = assembly.GetType(IoC.Resolve<string>("Game.Adapter.Name", typeTarget));

        return Activator.CreateInstance(type!, uObject)!;
    }
}
