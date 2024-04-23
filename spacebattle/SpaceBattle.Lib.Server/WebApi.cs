using CoreWCF;
using Hwdtech;

namespace WebHttp
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    internal class WebApi : IWebApi
    {
        public OrderContract HandleOrder(OrderContract request)
        {
            var dto = new OrderDTO(request); // приводим к IOrder из SpaceBattle.Lib !
            IoC.Resolve<SpaceBattle.Lib.ICommand>("Server.WebHttp.HandleOrderStrategy", dto).Execute();

            return request;
        }
    }
}
