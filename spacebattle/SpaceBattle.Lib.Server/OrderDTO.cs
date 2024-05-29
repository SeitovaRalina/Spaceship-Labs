using SpaceBattle.Lib;

namespace WebHttp
{
    internal class OrderDTO : IOrder
    {
        private readonly OrderContract _order;
        public OrderDTO(OrderContract order)
        {
            _order = order;
        }
        public string GameID => _order.GameID;
        public int GameItemID => _order.GameItemID;
        public string OrderType => _order.Type;
        public IDictionary<string, object> OrderProperties => _order.Properties;
    }
}
