using System.Runtime.Serialization;

namespace Service.Unlimint.Signer.Grpc.Models.BusinessAccount
{
    [DataContract]
    public class CreateDepositAddressRequest
    {
        [DataMember(Order = 1)] public string IdempotencyKey { get; set; }
        [DataMember(Order = 2)] public string Chain { get; set; }
        [DataMember(Order = 3)] public string Currency { get; set; }
    }
}