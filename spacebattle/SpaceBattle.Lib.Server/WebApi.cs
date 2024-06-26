﻿using CoreWCF;
using Hwdtech;

namespace WebHttp
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    internal class WebApi : IWebApi
    {
        public void HandleOrder(OrderContract request)
        {
            var dto = new OrderDTO(request); // приводим к IOrder из SpaceBattle.Lib !
            IoC.Resolve<SpaceBattle.Lib.ICommand>("Server.WebHttp.Command.HandleOrder", dto).Execute();
        }
    }
}
