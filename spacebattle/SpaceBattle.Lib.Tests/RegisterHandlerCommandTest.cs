using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests;

public class RegisterHandlerCommandTest
{
    public RegisterHandlerCommandTest()
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
            "Component.GetHashCode", (object[] args) =>
            {
                var output = new GetHashCodeStrategy().Init(args[0]);
                return output;
            }
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.ExceptionHandler.Register",
            (object[] args) => new RegisterHandlerCommand((IEnumerable<Type>)args[0], (IHandler)args[1])
        ).Execute();
    }

    [Fact]
    public void SuccesfullRegisterExistedHandlerInExceptionHandlerTree()
    {
        var handler = new Mock<IHandler>();
        var command = new Mock<ICommand>();

        var listOfTypes = new List<Type> { typeof(ICommand), typeof(Exception) };

        var exceptionHandlerTree = new Mock<IDictionary<object, IHandler>>();
        var realTree = new Dictionary<object, IHandler>();

        exceptionHandlerTree.Setup(x => x.Add(It.IsAny<object>(), It.IsAny<IHandler>())).Callback(
            (object key, IHandler handler) =>
            {
                realTree.TryAdd(key, handler);
            });

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.ExceptionHandler.Tree",
            (object[] args) => exceptionHandlerTree.Object
        ).Execute();

        IoC.Resolve<RegisterHandlerCommand>("Game.ExceptionHandler.Register", listOfTypes, handler.Object).Execute();

        Assert.Single(realTree);
        Assert.True(realTree.ContainsValue(handler.Object));
    }

    // Необходимо предусмотреть возможность, чтобы можно было задавать обработчики для команды, которая выбрасывает исключение, кроме указанных.
    [Fact]
    public void SuccesfullRegisterSomeHandlersCommand()
    {
        var handler = new Mock<IHandler>();
        var command = new Mock<ICommand>();

        var exceptionHandlerTree = new Mock<IDictionary<object, IHandler>>();
        var realTree = new Dictionary<object, IHandler>();
        exceptionHandlerTree.Setup(x => x.Add(It.IsAny<object>(), It.IsAny<IHandler>())).Callback(
        (object key, IHandler handler) =>
        {
            realTree.TryAdd(key, handler);
        });

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.ExceptionHandler.Tree",
            (object[] args) => exceptionHandlerTree.Object
        ).Execute();

        IoC.Resolve<ICommand>("Game.ExceptionHandler.Register", new List<Type> { typeof(ICommand), typeof(Exception) }, handler.Object).Execute();
        // IoC.Resolve<ICommand>("Game.ExceptionHandler.Register", new List<Type> { typeof(IQueue), typeof(Exception) }, handler.Object).Execute();
        IoC.Resolve<ICommand>("Game.ExceptionHandler.Register", new List<Type> { typeof(IQueue), typeof(KeyNotFoundException) }, handler.Object).Execute();

        exceptionHandlerTree.VerifyAll();
        Assert.Equal(2, realTree.Count());
    }
}
