using System.Runtime.Serialization;

namespace Service.Unlimint.Signer.Grpc.Models
{
    [DataContract]
    public class GetCircleApiKeyRequest
    {
        [DataMember(Order = 1)] public string BrokerId { get; set; }
    }
}