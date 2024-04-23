using System.Runtime.Serialization;
using CoreWCF.OpenApi.Attributes;

namespace WebHttp
{
    [DataContract(Name = "ExampleContract", Namespace = "http://example.com")]
    internal class ExampleContract
    {
        [DataMember(Name = "SimpleProperty", Order = 1)]
        [OpenApiProperty(Description = "SimpleProperty description.")]
        public required string SimpleProperty { get; set; }

        [DataMember(Name = "ComplexProperty", Order = 2)]
        [OpenApiProperty(Description = "ComplexProperty description.")]
        public required InnerContract ComplexProperty { get; set; }

        [DataMember(Name = "SimpleCollection", Order = 3)]
        [OpenApiProperty(Description = "SimpleCollection description.")]
        public required List<string> SimpleCollection { get; set; }

        [DataMember(Name = "ComplexCollection", Order = 4)]
        [OpenApiProperty(Description = "ComplexCollection description.")]
        public required List<InnerContract> ComplexCollection { get; set; }
    }

    [DataContract(Name = "InnerContract", Namespace = "http://example.com")]
    internal class InnerContract
    {
        [DataMember(Name = "Name", Order = 1)]
        [OpenApiProperty(Description = "Name description.")]
        public required string Name { get; set; }
    }
}
