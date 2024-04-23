namespace SpaceBattle.Lib;

public interface IOrder
{
    public string GameID { get; }
    public string GameItemID { get; }
    public string OrderType { get; }
    public IDictionary<string, object> OrderProperties { get; }
}
