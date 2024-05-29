using Hwdtech;

namespace SpaceBattle.Lib;

public class HandleOrderCommand : ICommand
{
    private readonly IOrder _order;
    public HandleOrderCommand(IOrder order)
    {
        _order = order;
    }
    public void Execute()
    {
        var threadID = IoC.Resolve<string>("Server.Thread.ThreadIDByGameID", _order.GameID);
        var cmdInterpret = IoC.Resolve<ICommand>("Game.Command.Interpret", _order);

        IoC.Resolve<ICommand>("Server.Thread.SendCommand", threadID, cmdInterpret).Execute();
    }
}
