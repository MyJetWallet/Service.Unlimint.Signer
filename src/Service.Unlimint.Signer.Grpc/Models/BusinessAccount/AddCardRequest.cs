using System.Runtime.Serialization;

namespace Service.Unlimint.Signer.Grpc.Models.BusinessAccount
{
    [DataContract]
    public class CreateBusinessTransferRequest
    {
        [DataMember(Order = 1)] public string IdempotencyKey { get; set; }
        [DataMember(Order = 2)] public string DestinationId { get; set; }
        [DataMember(Order = 3)] public string Amount { get; set; }
        [DataMember(Order = 4)] public string Currency { get; set; }
    }
}