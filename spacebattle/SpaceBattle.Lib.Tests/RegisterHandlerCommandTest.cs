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

        var strategyReturnDict = new Mock<IStrategy>();
        strategyReturnDict.Setup(x => x.Init()).Returns(new Dictionary<object, IHandler>());
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.ExceptionHandler.Tree",
            (object[] args) => strategyReturnDict.Object.Init(args)
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

        var listOfTypes = new List<Type> { typeof(ICommand), typeof(Exception) };
        var exceptionHandlerTree = IoC.Resolve<IDictionary<object, IHandler>>("Game.ExceptionHandler.Tree");

        IoC.Resolve<RegisterHandlerCommand>("Game.ExceptionHandler.Register", listOfTypes, handler.Object).Execute();

        Assert.Single(exceptionHandlerTree);
        var hashForTypes = IoC.Resolve<int>("Component.GetHashCode", listOfTypes);
        Assert.Equal(exceptionHandlerTree[hashForTypes], handler.Object);

    }

    // Необходимо предусмотреть возможность, чтобы можно было задавать обработчики для команды, которая выбрасывает исключение, кроме указанных.
    [Fact]
    public void SuccesfullRegisterSomeHandlersCommand()
    {
        var handler1 = new Mock<IHandler>();
        var handler2 = new Mock<IHandler>();
        var handler3 = new Mock<IHandler>();

        var exceptionHandlerTree = IoC.Resolve<IDictionary<object, IHandler>>("Game.ExceptionHandler.Tree");

        IoC.Resolve<ICommand>("Game.ExceptionHandler.Register", new List<Type> { typeof(ICommand), typeof(Exception) }, handler1.Object).Execute();
        IoC.Resolve<ICommand>("Game.ExceptionHandler.Register", new List<Type> { typeof(IQueue), typeof(Exception) }, handler2.Object).Execute();
        IoC.Resolve<ICommand>("Game.ExceptionHandler.Register", new List<Type> { typeof(IQueue), typeof(KeyNotFoundException) }, handler3.Object).Execute();

        Assert.Equal(3, exceptionHandlerTree.Count());
    }
}
