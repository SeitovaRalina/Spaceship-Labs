using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests;

public class EndCommandTest
{
    private static void InitialState()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        var CreateEndMoveCommand = (object[] args) =>
        {
            var command = (IMoveCommandEndable)args[0];
            return new EndMoveCommand(command);
        };

        var InjectCommand = (object[] args) =>
        {
            var wherefromInject = (IInjectableCommand)args[0];
            var whatInject = (ICommand)args[1];
            wherefrom.Inject(whatInject);
            return wherefromInject;
        };

        var DeleteObjectProperties = (object[] args) =>
        {
            var obj = (IUObject)args[0];
            var keys = (IEnumerate<string>)args[1];
            foreach (var key in keys)
            {
                obj.DeleteProperty(key);
            }

            return "ok";
        }
    }
    ICommand emptyCommand = new EmptyCommand();
    I
}