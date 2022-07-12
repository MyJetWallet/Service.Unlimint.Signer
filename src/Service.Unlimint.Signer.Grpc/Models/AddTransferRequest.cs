using System.Runtime.Serialization;

namespace Service.Unlimint.Signer.Grpc.Models
{
    [DataContract]
    public class AddTransferRequest
    {
        [DataMember(Order = 1)] public string BrokerId { get; set; }
        [DataMember(Order = 2)] public string ClientId { get; set; }
        [DataMember(Order = 3)] public string IdempotencyKey { get; set; }
        [DataMember(Order = 4)] public string SourceId { get; set; }
        [DataMember(Order = 5)] public double Amount { get; set; }
        [DataMember(Order = 6)] public string Currency { get; set; }
        [DataMember(Order = 7)] public string DstAddress { get; set; }
        [DataMember(Order = 8)] public string DstAddressTag { get; set; }
        [DataMember(Order = 9)] public string DstChain { get; set; }
    }
}