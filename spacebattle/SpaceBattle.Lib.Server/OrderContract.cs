using System.Runtime.Serialization;
using CoreWCF.OpenApi.Attributes;

namespace WebHttp
{
    [DataContract(Name = "OrderContract")]
    internal class OrderContract
    {
        [DataMember(Name = "Type", Order = 1)]
        [OpenApiProperty(Description = "Type of order.")]
        public required string Type { get; set; }

        [DataMember(Name = "GameID", Order = 2)]
        [OpenApiProperty(Description = "Game identification.")]
        public required string GameID { get; set; }

        [DataMember(Name = "GameItemID", Order = 3)]
        [OpenApiProperty(Description = "Object identification.")]
        public required int GameItemID { get; set; }

        [DataMember(Name = "Properties", Order = 4)]
        [OpenApiProperty(Description = "Order parameters.")]
        public required IDictionary<string, object> Properties { get; set; }
    }
}
