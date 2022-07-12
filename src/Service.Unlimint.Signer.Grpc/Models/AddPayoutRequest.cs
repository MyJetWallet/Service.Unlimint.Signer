using System.Runtime.Serialization;

namespace Service.Unlimint.Signer.Grpc.Models
{
    [DataContract]
    public class AddPayoutRequest
    {
        [DataMember(Order = 1)] public string BrokerId { get; set; }
        [DataMember(Order = 2)] public string ClientId { get; set; }
        [DataMember(Order = 3)] public string IdempotencyKey { get; set; }
        [DataMember(Order = 4)] public decimal Amount { get; set; }
        [DataMember(Order = 5)] public string Currency { get; set; }
        [DataMember(Order = 7)] public string DestinationId { get; set; }
        [DataMember(Order = 8)] public string DestinationType { get; set; }
    }
}