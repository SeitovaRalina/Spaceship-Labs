using Hwdtech;

namespace SpaceBattle.Lib;

public class HandleOrderStrategy : IStrategy
{
    public object Init(params object[] args)
    {
        var orderDTO = (IOrder)args[0];
        var threadID = IoC.Resolve<string>("Server.Thread.ThreadIDByGameID", orderDTO.GameID);
        var cmdInterpret = IoC.Resolve<ICommand>("Game.Command.Interpret", orderDTO);

        return IoC.Resolve<ICommand>("Server.Thread.SendCommand", threadID, cmdInterpret);
    }
}
