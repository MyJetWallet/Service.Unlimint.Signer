using System.Runtime.Serialization;

namespace Service.Unlimint.Signer.Grpc.Models
{
    [DataContract]
    public class UnlimintAmount
    {
        [DataMember(Order = 1)] public decimal Amount { get; set; }
        [DataMember(Order = 2)] public string Currency { get; set; }
    }
}