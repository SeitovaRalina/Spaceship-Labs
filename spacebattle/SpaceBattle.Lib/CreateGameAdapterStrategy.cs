﻿using System.Reflection;
using Hwdtech;

namespace SpaceBattle.Lib;

public class CreateGameAdapterStrategy : IStrategy
{
    public object Init(params object[] args)
    {
        var uObject = (IUObject)args[0];
        var typeTarget = (Type)args[1];

        var assemblyDictionary = IoC.Resolve<IDictionary<KeyValuePair<Type, Type>, Assembly>>("Game.Adapter.Assemblies.Dictionary");
        var pairOfTypes = new KeyValuePair<Type, Type>(uObject.GetType(), typeTarget);

        if (!assemblyDictionary.ContainsKey(pairOfTypes))
        {
            IoC.Resolve<ICommand>("Game.Adapter.Compile", uObject.GetType(), typeTarget).Execute();
        }

        return IoC.Resolve<object>("Game.Adapter.Find", uObject, typeTarget);
    }
}
