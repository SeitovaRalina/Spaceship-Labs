using System.Reflection;
using Hwdtech;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace SpaceBattle.Lib;

public class CompileStringCodeStrategy : IStrategy
{
    public object Init(params object[] args)
    {
        var codeString = (string)args[0];

        var name = IoC.Resolve<string>("Code.Compile.AssemblyName");
        var references = IoC.Resolve<IEnumerable<MetadataReference>>("Code.Compile.References");
        var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
        var syntaxTree = CSharpSyntaxTree.ParseText(codeString);

        var compilation = CSharpCompilation.Create(name)
            .WithOptions(options)
            .AddReferences(references)
            .AddSyntaxTrees(syntaxTree);

        Assembly assembly;

        using (var ms = new MemoryStream())
        {
            var result = compilation.Emit(ms);
            ms.Seek(0, SeekOrigin.Begin);
            assembly = Assembly.Load(ms.ToArray());
        }

        return assembly;
    }
}
