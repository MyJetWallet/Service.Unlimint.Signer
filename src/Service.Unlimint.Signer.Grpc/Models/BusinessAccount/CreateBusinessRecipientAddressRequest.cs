using System.Runtime.Serialization;

namespace Service.Unlimint.Signer.Grpc.Models.BusinessAccount
{
    [DataContract]
    public class CreateBusinessRecipientAddressRequest
    {
        [DataMember(Order = 1)] public string IdempotencyKey { get; set; }
        [DataMember(Order = 2)] public string Chain { get; set; }
        [DataMember(Order = 3)] public string Currency { get; set; }
        [DataMember(Order = 4)] public string Address { get; set; }
        [DataMember(Order = 5)]public string AddressTag { get; set; }
        [DataMember(Order = 6)] public string Description { get; set; }
    }
}