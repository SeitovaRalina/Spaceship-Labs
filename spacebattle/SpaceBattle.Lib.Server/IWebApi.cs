﻿using System.Net;
using CoreWCF;
using CoreWCF.OpenApi.Attributes;
using CoreWCF.Web;

namespace WebHttp
{
    [ServiceContract]
    [OpenApiBasePath("/api")]
    internal interface IWebApi
    {
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/order")]
        [OpenApiTag("Order message")]
        [OpenApiResponse(ContentTypes = new[] { "application/json" }, Description = "Success", StatusCode = HttpStatusCode.OK, Type = typeof(OrderContract))]
        [OpenApiResponse(Description = "Unsupported Format", StatusCode = HttpStatusCode.UnsupportedMediaType)]
        OrderContract HandleOrder(
            [OpenApiParameter(ContentTypes = new[] { "application/json" }, Description = "Send an order to a spaceship.")] OrderContract param);
    }
}
