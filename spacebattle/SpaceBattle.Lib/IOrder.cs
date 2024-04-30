namespace SpaceBattle.Lib;

public interface IOrder
{
    public string GameID { get; }
    public int GameItemID { get; }
    public string OrderType { get; }
    public IDictionary<string, object> OrderProperties { get; }
}
